#!/bin/bash
# Test script to verify .NET 9 is properly installed in the dev container

echo "Testing .NET 9 installation..."

# Check .NET version
echo "Checking .NET version:"
dotnet --version

# Check .NET info
echo -e "\nChecking .NET info:"
dotnet --info

# Try to create a simple console app to test the SDK
echo -e "\nTesting .NET SDK by creating a test project:"
mkdir -p /tmp/test-dotnet
cd /tmp/test-dotnet
dotnet new console -n TestApp -f net9.0

if [ $? -eq 0 ]; then
    echo "✅ .NET 9 SDK is working properly - console app created successfully"
    
    # Try to build the project
    cd TestApp
    dotnet build
    
    if [ $? -eq 0 ]; then
        echo "✅ .NET 9 build is working properly"
    else
        echo "❌ .NET 9 build failed"
    fi
else
    echo "❌ .NET 9 SDK test failed"
fi

# Clean up
rm -rf /tmp/test-dotnet

echo -e "\n✅ Dev container .NET 9 verification complete"