using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ADSBLib;
using System.Threading;
using System.Diagnostics;

namespace ADSBLibUnitTests {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void TestMethod1() {

            Collector collector = new Collector();
            Connection con = new Connection("127.0.0.1", 30003);
            con.Connect(LogLine, collector);

            while(true) {

            }

            con.Disconnect();
        }

        private void LogLine(string line) {
            Debug.WriteLine(line);
        }
    }
}
