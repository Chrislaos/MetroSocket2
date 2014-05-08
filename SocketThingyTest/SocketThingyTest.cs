using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

using SocketThingy;
using Windows.Networking.Sockets;
using Windows.Networking;
using Newtonsoft.Json.Linq;

namespace SocketThingyTest
{
    [TestClass]
    public class SocketThingyTest
    {
        [TestMethod]
        public void TestConnection()
        {
           /*
            String message = "Hallaien!";
            
            string mockServerIP = "9876";
            string mockServerIp = "127.0.0.1";

            Object mockServer = CreateMockServer();
            SocketThingy.MainPage page = new SocketThingy.MainPage();
            page.Connect();
            mockServer.Send(message);
            page.ReadFromSocket();
            string recieved = page.RecievedText;
            Assert.AreEqual(message, recieved);
            */ 
        }

        /*
        private Object CreateMockServer() 
        {
            string mockServerPort = "9876";
            string mockServerIp = "127.0.0.1";

            return new Server(mockServerIP, mockServerPort);
        } */
    }
}
