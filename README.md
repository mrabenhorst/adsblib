# adsblib
C# Library for collecting and processing ADS-B data from a Dump1090 port 30003 socket

# How to use (Standalone):
1. Open the ADSBCLI Project
2. Build
3. Run ADSBCLI.exe
4. A file will be created with the format "flights-YYYYMMDD.csv" will all collected ADS-B data from localhost Socket 30003

# How to use (As .NET library):
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
