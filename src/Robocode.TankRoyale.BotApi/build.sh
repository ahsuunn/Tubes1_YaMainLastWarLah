#!/bin/bash

rm -rf bin obj
dotnet nuget locals all --clear
dotnet restore
if ! dotnet build --configuration Release; then
    echo "Build failed. Exiting."
    exit 1
fi

nupkg=$(find bin/Release -name "*.nupkg" | head -n 1)
if [ -z "$nupkg" ]; then
    echo "No .nupkg file found. Exiting."
    exit 1
fi

for dir in ../*Bot/; do
    if [[ -d "$dir" && "$dir" =~ Bot$ ]]; then
        rm -rf "$dir/packages"
        cp -f "$nupkg" "$dir/packages/"
        echo "Copied $nupkg to $dir/packages/"
    fi
done

echo "Done."
