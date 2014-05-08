using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.Networking.NetworkOperators;
using Windows.Networking.BackgroundTransfer;
using Newtonsoft.Json;
using System.IO;

namespace SocketThingy
{
    public interface INetworking
    {
        event DisconnectedHandler Disconnected;
        event MessageReceivedHandler MessageReceived;
        event MessageSubmittedHandler MessageSubmitted;

        public delegate void MessageReceivedHandler(Networking N, String Msg);
        public delegate void MessageSubmittedHandler(Networking N, String Msg);
        public delegate void DisconnectedHandler(Networking N);

        async void StartClient();
        void StartClient(String Hostname, String Port);
        bool IsConnected();
        async void Receive();
        void Send(String msg, bool close);
        void close();

    }

    public class Networking : INetworking
    {
        private StreamSocket _socket;
        private static HostName _localHost;
        private HostName _remoteHost;
        private String _serviceName;
        private EndpointPair _endpointpair;


        public Networking()
        {
            _socket = new StreamSocket();
            _localHost = new HostName("192.168.1.113");
            _remoteHost = new HostName("192.168.1.145");
            _serviceName = "1337";
            _endpointpair = new EndpointPair(_localHost, _serviceName, _remoteHost, _serviceName);

        }

        public event INetworking.DisconnectedHandler Disconnected;

        public event INetworking.MessageReceivedHandler MessageReceived;

        public event INetworking.MessageSubmittedHandler MessageSubmitted;

        async public void StartClient()
        {
           await _socket.ConnectAsync(_endpointpair);
        }

        public void StartClient(string Hostname, string Port)
        {
            throw new NotImplementedException();
        }

        public bool IsConnected()
        {
            throw new NotImplementedException();
        }

        public void Receive()
        {
                   
        }

        public void Send(string msg, bool close)
        {
            throw new NotImplementedException();
        }

        public void close()
        {
            throw new NotImplementedException();
        }
    }
}
