#!/bin/bash

sudo systemctl stop rhizosphere.service

cd /home/p/Rhizosphere/PersonalMushroomComputer/src/Rhizosphere

dotnet publish -r linux-arm64 --sc -c Release -o /home/p/Rhizosphere/Service

sudo cp /home/p/Rhizosphere/PersonalMushroomComputer/src/Rhizosphere/rhizosphere.service /etc/systemd/system/rhizosphere.service

sudo systemctl daemon-reload

cd /home/p/Rhizosphere/Service

chmod +x Rhizosphere

sudo systemctl start rhizosphere.service

# sudo systemctl status rhizosphere.service

sudo systemctl enable rhizosphere.service

sudo journalctl -u rhizosphere.service -f