

dotnet publish -r linux-arm64 --sc -c Release

sudo cp /home/p/Rhizosphere/PersonalMushroomComputer/src/Rhizosphere/rhizosphere.service /etc/systemd/system/rhizosphere.service

sudo systemctl daemon-reload

sudo systemctl start rhizosphere.service

cd /home/p/Rhizosphere/PersonalMushroomComputer/src/Rhizosphere/bin/Release/net6.0/linux-arm64/publish/

chmod +x Rhizosphere