// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Kismet/BlueprintFunctionLibrary.h"
#include "Sockets.h"
#include "SocketSubsystem.h"
#include "Networking.h"
#include "AssetPositionStream.generated.h"

/**
 * 
 */
UCLASS()
class CAPTURE_API UAssetPositionStream : public UBlueprintFunctionLibrary
{
	GENERATED_BODY()

public:
	// Save a string to a file
	UFUNCTION(BlueprintCallable, Category = "File IO")
	static void SendPostionToMP(const float& XPosition, const float& YPosition, const float& ZPosition);
};
