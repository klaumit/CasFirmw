#!/bin/sh

dotnet build binary.slnx

dotnet publish -o dist/net40 -f net40 -c Release Hexer.Lib

