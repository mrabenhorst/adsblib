using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using static ADSBLib.Connection;

namespace ADSBLib {

    /*****************************************************************
    * Listener
    * 
    * Class that listens to dump1090 TCP 30003 and collects the messages
    * 
    * **************************************************************/
    public class Listener {
        private TcpClient tcp;
        private byte[] readBuffer;
        private NetworkStream stream;
        public MessageCallback messageCallback;

        /*****************************************************************
        * Listener
        * 
        * Constructor with address and port input with callback
        * 
        * **************************************************************/
        public Listener(string address, int port, MessageCallback messageCallback) {
            this.messageCallback = messageCallback;
            tcp = new TcpClient();
            tcp.Connect(address, port);
        }

        /*****************************************************************
        * Listen
        * 
        * Handles getting the string from the tcp stream
        * 
        * **************************************************************/
        public void Listen() {
            using(stream = tcp.GetStream()) {
                while(true) {
                    string line = ReadOut();
                    if(!string.IsNullOrEmpty(line) && messageCallback != null) {
                        messageCallback(line);
                    }
                }
            }
        }

        /*****************************************************************
        * ReadOut
        * 
        * Processes the tcp message
        * 
        * **************************************************************/
        private string ReadOut() {
            string output = null;

            if(stream.CanRead) {
                readBuffer = new byte[tcp.ReceiveBufferSize];
                stream.Read(readBuffer, 0, tcp.ReceiveBufferSize);

                output = System.Text.Encoding.ASCII.GetString(readBuffer).Trim();
            }

            return output;
        }
    }
}
