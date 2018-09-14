#!/bin/bash

./dockerStopAllContainers.sh

sudo rm -R ~/RefSite/ui/dist/skd-ref-site/
sudo rm -R ~/RefSite/api/

cp -R ~/deploy/ui/ ~/RefSite/ui/dist/skd-ref-site/
cp -R ~/deploy/api/ ~/RefSite/api/

sudo chmod 755 -R ~/RefSite/

cd ~/RefSite/

docker-compose up --build -d

