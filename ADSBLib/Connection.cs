using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using static ADSBLib.Connection;

namespace ADSBLib {

    /*****************************************************************
    * Connection
    * 
    * Main class that provides the overall connection to dump1090
    * 
    * **************************************************************/
    public class Connection {
        private Thread readThread;
        private string address;
        private int port;

        public delegate void MessageCallback(string message);
        private Listener listener;

        private Collector collector;


        /*****************************************************************
        * Connection
        * 
        * Constructor with address and port inputs
        * 
        * **************************************************************/
        public Connection(string address, int port) {
            this.address = address;
            this.port = port;
            
        }

        /*****************************************************************
        * Connect
        * 
        * Initiates the connection with a postback function using a collector
        * 
        * **************************************************************/
        public bool Connect(Action<string> postback, Collector collector) {
            try {
                BeginReading();
                this.collector = collector;
                return true;
            } catch(Exception e) {
                return false;
            }
        }

        /*****************************************************************
        * BeginReading
        * 
        * Internal initiation of multithreaded collection
        * 
        * **************************************************************/
        private void BeginReading() {
            listener = new Listener(this.address, this.port, ProcessMessage);
            ThreadStart threadRef = new ThreadStart(listener.Listen);
            readThread = new Thread(threadRef);
            readThread.Start();
        }

        /*****************************************************************
        * Disconnect
        * 
        * Force disconnect
        * 
        * **************************************************************/
        public void Disconnect() {
            if(readThread.IsAlive) {
                readThread.Abort();
                readThread = null;
            }
        }

        /*****************************************************************
        * ProcessMessage
        * 
        * Handles message conversion to ADSBData and sends to collector
        * 
        * **************************************************************/
        private void ProcessMessage(string message) {
            ADSBData data = new ADSBData(message);
            this.collector.AddData(data);
        }
    }

    
}
