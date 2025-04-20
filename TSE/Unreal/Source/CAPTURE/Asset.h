// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "Sockets.h"
#include "SocketSubsystem.h"
#include "Networking.h"
#include "Asset.generated.h"

#pragma pack(push, 1) // Disable padding
struct FPositionData
{
	int32 SeqNum;
	double Lat;
	double Lon;
	double Alt;
};
#pragma pack(pop)

UCLASS()
class CAPTURE_API AAsset : public AActor
{
	GENERATED_BODY()
	
public:	
	AAsset();
	virtual void BeginPlay() override;
	virtual void EndPlay(const EEndPlayReason::Type EndPlayReason) override;

private:
	FSocket* ListenSocket;
	FSocket* ClientSocket;
	FTimerHandle AcceptClientTimerHandle;
	FThreadSafeBool bIsRunning;

	void StartTCPServer();
	void AcceptClient();
	void HandleClient();
	void StopTCPServer();
	FPositionData GetAssetCoordinateData(const float XPosition, const float YPosition, const float ZPosition);
};
