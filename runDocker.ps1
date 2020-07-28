#
#
if ( -not ( Test-Path  -LiteralPath '..\yawat.settings.json' -PathType Leaf ) ) { 
	Write-Host "We expect the settings file '..\yawat.settings.json' to exist."
	exit -1;
}

Write-Host "Two docker images are build and started via docker-compose."
Write-Host "Use http://localhost:44328/api/TodoItems to access the YawatServer"
Write-Host "Use http://localhost:5001/connect/userinfo to access the IdentityServer"
Write-Host "" 
Write-Host "Note: Within Docker you should use http instead of https" -ForegroundColor DarkGreen -BackgroundColor White
Write-Host "      Update the settings for IdentityServer.IdentityServerUrl to use http instead of https" -ForegroundColor DarkGreen -BackgroundColor White
Write-Host "" 
Write-Host "Press any key to continue"
Read-Host

# now copy the settings  file
COPY ..\yawat.settings.json Yawat.Servers\YawatServer\bin\Release\netcoreapp3.1\publish\yawat.settings.json
COPY ..\yawat.settings.json Yawat.Servers\IdentityServer\bin\Release\netcoreapp3.1\publish\yawat.settings.json

Write-Host "Create docker images ..."
cd Yawat.Servers\YawatServer
docker build -t yawatwerver -f Dockerfile .
cd ..\..

cd Yawat.Servers\IdentityServer
docker build -t yawatidentityserver -f Dockerfile .
cd ..\..

Write-Host "docker-compose"
cd Yawat.Servers
docker-compose up
cd ..
