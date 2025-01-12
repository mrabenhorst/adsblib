# ADSBLib
C# Library for collecting and processing ADS-B data from a Dump1090 port 30003 socket

## Purpose:
Dump 1090 Socket 30003 delivers pieces of partially complete ADS-B information as it is read OTA. These records might not include all metadata previously broadcasted. This library collects all data coming in and makes sure to deliver records using as much metadata as is known about the aircraft at the time of delivery. Most importantly, messages will not be delivered unless location (Lat/Lng/Alt) is known.

## How to use (Standalone):
1. Open the ADSBCLI Project
2. Build
3. Run ADSBCLI.exe
4. A file will be created with the format "flights-YYYYMMDD.csv" will all collected ADS-B data from localhost Socket 30003

## How to use (As .NET library):
1. Open the ADSBLib project
2. Build
3. Import ADSBLib.dll into your project
4. Look at the code in ADSBCLI Program.cs file for example use

```
Collector collector = new Collector(holdingPostback:HoldingPostback, currentPostback:CurrentPostback);
Connection con = new Connection("127.0.0.1", 30003);
con.Connect(LogLine, collector);

while(true) { }

con.Disconnect();
```
