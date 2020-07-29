#
#
if ( -not (Test-Path -Path 'C:\NuGet.local' -PathType Container) ) { 
	Write-Host "We assume that the Yawat NuGet packages are to be created to and restored from the local Nuget repository C:\NuGet.local"
	exit -1;
}

$VERSION = "0.0.14"

Write-Host "Build Yawat for version $VERSION"

cd Yawat
dotnet restore  --verbosity detailed
dotnet build  /p:Version=$VERSION /p:AssemblyVersion=$VERSION --configuration Release
dotnet pack  /p:Version=$VERSION /p:AssemblyVersion=$VERSION --configuration Release
Copy-Item src\bin\Release\*$VERSION.nupkg -Destination C:\NuGet.local
cd ..


Write-Host Build Yawat.Authenticators
cd Yawat.Authenticators
Write-Host Build ...update Yawat package
cd BasicAuthentication
dotnet add package Yawat
cd ../IdentityServer
dotnet add package Yawat
cd ../Okta
dotnet add package Yawat
cd ..

dotnet restore  --verbosity detailed
dotnet build /p:Version=$VERSION /p:AssemblyVersion=$VERSION --configuration Release
dotnet pack /p:Version=$VERSION /p:AssemblyVersion=$VERSION --configuration Release
Copy-Item BasicAuthentication\bin\Release\*$VERSION.nupkg -Destination  C:\NuGet.local
Copy-Item IdentityServer\bin\Release\*$VERSION.nupkg -Destination  C:\NuGet.local
Copy-Item Okta\bin\Release\*$VERSION.nupkg -Destination  C:\NuGet.local
cd ..


Write-Host Build Yawat.Samples
cd Yawat.Samples
Write-Host Build ...update Yawat package
cd Authenticated
dotnet add package Yawat
cd ..\Unrestricted
dotnet add package Yawat
cd ..
cd ..



Write-Host Build Yawat.Servers
   
cd Yawat.Servers
Write-Host Build ...update Yawat package
cd YawatServer
dotnet add package Yawat
cd ..

dotnet restore  --verbosity detailed
dotnet build /p:Version=$VERSION /p:AssemblyVersion=$VERSION --configuration Release
dotnet publish /p:Version=$VERSION /p:AssemblyVersion=$VERSION --configuration Release
cd ..

