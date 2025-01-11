using ADSBLib;
using System;
using System.Diagnostics;
using System.IO;

namespace ADSBCLI {

    /*****************************************************************
    * Program
    * 
    * Main program for collecting dump1090 TCP 30003 strings to ADS-B
    * lines for use in 3D plotting
    * 
    * **************************************************************/
    class Program {

        /*****************************************************************
        * Main
        * 
        * **************************************************************/
        static void Main(string[] args) {

            Collector collector = new Collector(holdingPostback:HoldingPostback, currentPostback:CurrentPostback);
            Connection con = new Connection("127.0.0.1", 30003);
            con.Connect(LogLine, collector);

            while(true) {

            }

            con.Disconnect();

        }

        /*****************************************************************
        * HoldingPostback
        * 
        * **************************************************************/
        private static void HoldingPostback(ADSBData data) {
            LogADSBData("Holding", data);
        }

        /*****************************************************************
        * CurrentPostback
        * 
        * Records data to file
        * 
        * **************************************************************/
        private static void CurrentPostback(ADSBData data) {
            LogADSBData("Current", data);

            File.AppendAllText($"flights-{String.Format("{0:yyyyMMdd}", DateTime.Now)}.csv", data.Readout() + Environment.NewLine);
        }

        /*****************************************************************
        * LogLine
        * 
        * Sends line to debug log
        * 
        * **************************************************************/
        private static void LogLine(string line) {
            Debug.WriteLine(line);
        }

        /*****************************************************************
        * LogADSBData
        * 
        * Sends ADSBData to debug log
        * 
        * **************************************************************/
        private static void LogADSBData(string message, ADSBData data) {
            Debug.WriteLine(message + ": " + data.Readout());
        }
    }
}
