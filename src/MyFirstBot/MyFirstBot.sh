#!/bin/bash

rm -rf bin obj
if ! dotnet build --configuration Release; then
    echo "Build failed. Exiting."
    exit 1
fi

dotnet run --configuration Release --no-build
