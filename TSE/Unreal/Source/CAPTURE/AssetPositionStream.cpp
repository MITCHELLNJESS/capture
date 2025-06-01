// Fill out your copyright notice in the Description page of Project Settings.


#include "AssetPositionStream.h"
#include "Misc/FileHelper.h"
#include "Misc/Paths.h"

void UAssetPositionStream::SendPostionToMP(const float& XPosition, const float& YPosition, const float& ZPosition)
{
	const float EarthRadius = 6378137.0f; // in meters
	const float DegPerRad = 180.0f / PI;

	// Covnert Unreal units to meters
	const float XPositionMeters = XPosition / 100.0f;
	const float YPositionMeters = YPosition / 100.0f;
	const float ZPositionMeters = ZPosition / 100.0f;

	// Origin: your baseline
	const float OriginLat = 38.75080920f;
	const float OriginLon = -77.49733298f;
	const float OriginElev = 78.6f;

	// Unreal: Y is North, X is East
	float dLat = (YPositionMeters / EarthRadius) * DegPerRad;
	float dLon = (XPositionMeters / (EarthRadius * FMath::Cos(FMath::DegreesToRadians(OriginLat)))) * DegPerRad;

	float OutLatitude = OriginLat + dLat;
	float OutLongitude = OriginLon + dLon;
	float OutElevation = OriginElev; // always assume asset is on the ground

	FString FullPath = FPaths::ProjectDir() + "/assetPosition.txt";
    FString CoordStr = FString::Printf(
		TEXT("Lat: %f, Lon: %f, Alt: %f\n"),
		OutLatitude,
		OutLongitude,
		OutElevation
	);
	FFileHelper::SaveStringToFile(CoordStr, *FullPath, FFileHelper::EEncodingOptions::AutoDetect, &IFileManager::Get(), FILEWRITE_Append);
}