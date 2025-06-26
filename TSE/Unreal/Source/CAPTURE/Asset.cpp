// Fill out your copyright notice in the Description page of Project Settings.


#include "Asset.h"
#include <cmath>

// Sets default values
AAsset::AAsset()
{
    PrimaryActorTick.bCanEverTick = false;
    ListenSocket = nullptr;
    ClientSocket = nullptr;

}

// Called when the game starts or when spawned
void AAsset::BeginPlay()
{
	Super::BeginPlay();

    StartTCPServer();
}

void AAsset::EndPlay(const EEndPlayReason::Type EndPlayReason)
{
    StopTCPServer();
    Super::EndPlay(EndPlayReason);
}

void AAsset::StartTCPServer()
{
    ISocketSubsystem* SocketSubsystem = ISocketSubsystem::Get(PLATFORM_SOCKETSUBSYSTEM);
    TSharedRef<FInternetAddr> Addr = SocketSubsystem->CreateInternetAddr();

    bool bIsValid;
    Addr->SetIp(TEXT("127.0.0.1"), bIsValid);
    Addr->SetPort(23);

    ListenSocket = SocketSubsystem->CreateSocket(NAME_Stream, TEXT("TCP_LISTEN_SOCKET"), false);
    ListenSocket->SetReuseAddr(true);
    ListenSocket->SetNonBlocking(true);

    bool bBind = ListenSocket->Bind(*Addr);
    bool bListen = ListenSocket->Listen(1);

    if (bBind && bListen)
    {
        UE_LOG(LogTemp, Log, TEXT("[TCPServer] Listening on 127.0.0.1:23"));
        bIsRunning = true;

        GetWorld()->GetTimerManager().SetTimer(
            AcceptClientTimerHandle,
            this,
            &AAsset::AcceptClient,
            0.1f,
            true
        );
    }
    else
    {
        UE_LOG(LogTemp, Error, TEXT("[TCPServer] Failed to bind or listen"));
        StopTCPServer();
    }
}

void AAsset::AcceptClient()
{
    if (!ListenSocket) return;

    TSharedRef<FInternetAddr> ClientAddr = ISocketSubsystem::Get(PLATFORM_SOCKETSUBSYSTEM)->CreateInternetAddr();
    ClientSocket = ListenSocket->Accept(*ClientAddr, TEXT("TCP_CLIENT_SOCKET"));

    if (ClientSocket)
    {
        UE_LOG(LogTemp, Log, TEXT("[TCPServer] Client connected!"));
        GetWorld()->GetTimerManager().ClearTimer(AcceptClientTimerHandle);

        // Run client handling on background thread
        Async(EAsyncExecution::Thread, [this]() { HandleClient(); });
    }
}

void AAsset::HandleClient()
{
    uint32 DataSize;
    TArray<uint8> ReceivedData;

    if (bIsRunning && ClientSocket && ClientSocket->Wait(ESocketWaitConditions::WaitForRead, FTimespan::FromSeconds(1.0)))
    {
        if (ClientSocket->HasPendingData(DataSize))
        {
            ReceivedData.SetNumUninitialized(DataSize);
            int32 BytesRead = 0;

            if (ClientSocket->Recv(ReceivedData.GetData(), ReceivedData.Num(), BytesRead))
            {
                FString Message = FString(ANSI_TO_TCHAR(reinterpret_cast<const char*>(ReceivedData.GetData())));
                UE_LOG(LogTemp, Log, TEXT("[TCPServer] Received: %s"), *Message);

                // Get actor position
                FVector Position = GetActorLocation();
                UE_LOG(LogTemp, Log, TEXT("Asset location retrieved [X = %f, Y = %f, Z = %f]"), Position.X, Position.Y, Position.Z);
                FPositionData PosData = GetAssetCoordinateData(Position.X, Position.Y, Position.Z);
                FString CoordinateDataString = FString::Printf(
                    TEXT("SeqNum: %d, Lat: %f, Lon: %f, Alt: %f"),
                    PosData.SeqNum,
                    PosData.Lat,
                    PosData.Lon,
                    PosData.Alt
                );
                UE_LOG(LogTemp, Log, TEXT("[TCPServer] Sending asset location: %s"), *CoordinateDataString);

                // Convert and send if socket is still valid
                if (ClientSocket->GetConnectionState() == ESocketConnectionState::SCS_Connected)
                {
                    // 1. Create a buffer with the exact layout the client expects
                    TArray<uint8> Buffer;
                    Buffer.SetNumZeroed(sizeof(double) * 4); // 4 "double-sized" blocks

                    // 2. Copy seqNum into first 4 bytes, leaving 4 zero-padding bytes after
                    FMemory::Memcpy(Buffer.GetData(), &PosData.SeqNum, sizeof(int32));

                    // 3. Copy doubles into next locations
                    FMemory::Memcpy(Buffer.GetData() + sizeof(double), &PosData.Lat, sizeof(double));
                    FMemory::Memcpy(Buffer.GetData() + sizeof(double) * 2, &PosData.Lon, sizeof(double));
                    FMemory::Memcpy(Buffer.GetData() + sizeof(double) * 3, &PosData.Alt, sizeof(double));

                    // Send position back to client as raw bytes
                    int32 BytesSent = 0;
                    if (ClientSocket->Send(Buffer.GetData(), Buffer.Num(), BytesSent))
                    {
                        UE_LOG(LogTemp, Log, TEXT("[TCPServer] Sent asset location: %s"), *CoordinateDataString);
                    }
                    else
                    {
                        UE_LOG(LogTemp, Warning, TEXT("[TCPServer] Failed to send data."));
                    }
                }
                else
                {
                    UE_LOG(LogTemp, Warning, TEXT("[TCPServer] Client disconnected before sending response."));
                }
            }
            else
            {
                UE_LOG(LogTemp, Warning, TEXT("[TCPServer] Failed to receive data or no bytes read."));
            }
        }
    }

    // Cleanup socket safely
    if (ClientSocket)
    {
        if (ClientSocket->GetConnectionState() != ESocketConnectionState::SCS_NotConnected)
        {
            ClientSocket->Close();
        }

        ISocketSubsystem::Get(PLATFORM_SOCKETSUBSYSTEM)->DestroySocket(ClientSocket);
        ClientSocket = nullptr;
    }

    UE_LOG(LogTemp, Log, TEXT("[TCPServer] Finished handling client, ready for next."));

    AsyncTask(ENamedThreads::GameThread, [this]()
        {
            if (bIsRunning && ListenSocket)
            {
                GetWorld()->GetTimerManager().SetTimer(
                    AcceptClientTimerHandle,
                    this,
                    &AAsset::AcceptClient,
                    0.1f,
                    true
                );
            }
        });
}

void AAsset::StopTCPServer()
{
    bIsRunning = false;

    if (ClientSocket)
    {
        ClientSocket->Close();
        ISocketSubsystem::Get(PLATFORM_SOCKETSUBSYSTEM)->DestroySocket(ClientSocket);
        ClientSocket = nullptr;
    }

    if (ListenSocket)
    {
        ListenSocket->Close();
        ISocketSubsystem::Get(PLATFORM_SOCKETSUBSYSTEM)->DestroySocket(ListenSocket);
        ListenSocket = nullptr;
    }
}

FPositionData AAsset::GetAssetCoordinateData(const float XPosition, const float YPosition, const float ZPosition)
{
    // UAV origin LLA on demonstration field.
    const double OriginLat = 38.75080920; // decimal degrees
    const double OriginLon = -77.49733298; // decminal degrees
    const double OriginAlt = 78.6; // meters

    // Unreal Engine positional vector of UAV starting point.
    double OriginX = 0.0;
    double OriginY = 0.0;
    double OriginZ = 0.0;

    // In Unreal Engine 1 unit = 1 cm, so divide deltas by 100 to get the deltas in meters.
    double dX = (XPosition - OriginX) / 100.0;
    double dY = (YPosition - OriginY) / 100.0;
    double dZ = (ZPosition - OriginZ) / 100.0;

    // WGS-84 constants.
    const double LatMetersPerDeg = 111320.0;
    const double LonMetersPerDeg = FMath::Cos(FMath::DegreesToRadians(OriginLat)) * 111320.0;

    // Calculate asset LLA.
    double OutLatitude = OriginLat + (dX / LatMetersPerDeg);
    double OutLongitude = OriginLon + (dY / LonMetersPerDeg);
    double OutAltitude = OriginAlt;

    FPositionData PosData;
    PosData.SeqNum = 1;
    PosData.Lat = OutLatitude;
    PosData.Lon = OutLongitude;
    PosData.Alt = OutAltitude;

    return PosData;
}