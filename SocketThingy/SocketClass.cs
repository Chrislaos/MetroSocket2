using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Networking;
using Newtonsoft.Json.Linq;
using Demo.Protocol;
using Windows.Storage.Streams;
using System.Collections.ObjectModel;
using Windows.Networking.Connectivity;

namespace SocketThingy
{
    class SocketClass
    {
        StreamSocket socket = new StreamSocket();
        static HostName localHost;
        static HostName remoteHost = new HostName("10.0.0.8");
        static string socketString = "1337";
        PDU pdu2;
        static string status;

        public void CurrentIPAddress()
             {
            ConnectionProfile profile = NetworkInformation.GetInternetConnectionProfile();

            if (profile != null && profile.NetworkAdapter != null)
            {
                HostName hostname = getHostName(profile);
                            
                if (hostname != null)
                {
                    // the ip address
                    localHost = hostname.CanonicalName;
                }
            }
        }

        private HostName getHostName(ConnectionProfile profile)
        {
            HostName hostname =  NetworkInformation.GetHostNames().SingleOrDefault(
                            hn => (
                                hn.IPInformation != null 
                                && hn.IPInformation.NetworkAdapter != null
                                && hn.IPInformation.NetworkAdapter.NetworkAdapterId
                                == profile.NetworkAdapter.NetworkAdapterId
                            )
            );

            return hostname;
        }
    

        public PDU pduCreator(CommandMessageID IDmsg, string tempDescription, JObject obj)
        {
            PDU pdu = new PDU()
            {
                MessageID = (int)IDmsg,
                MessageDescription = tempDescription,
                MessageType = "Command",
                Source = "Demo.Client",
                Data = obj
            };
            return pdu;
        }
        public async void nowConnecting()
        {
            bool connected = false;
            int remotePort = 1337;
            while (!connected)
            {

                try{
                    EndpointPair connection = new EndpointPair(localHost, remotePort.ToString(), remoteHost, socketString);
                    await socket.ConnectAsync(connection);
                    connected = true;

                    status = "Welcome, you are now connected";
                }
                catch (Exception ex)
                {
                    connected = false;
                    remotePort++;
                    status = ("Changed to free port: " + remotePort);
                }
            }
            sendData();
        }
        private async void sendData(CommandMessageID cmdID, string tempString, JObject obj)
        {
            PDU k = pduCreator(cmdID, tempString, obj);
            

            var dr = new DataWriter(socket.OutputStream);

            //var len = dr.MeasureString(pdu.ToJson());
            String message = k.ToJson();
            //dr.WriteInt32((int)len);
            dr.WriteString(k.ToJson());
            var ret = await dr.StoreAsync();

            recieveData();
            dr.DetachStream();
        }
        async private void recieveData()
        {
            StreamSocketListener listener = new StreamSocketListener();
            DataReader dr = new DataReader(socket.InputStream);
            dr.InputStreamOptions = InputStreamOptions.Partial;
            string msg = null;
            var count = await dr.LoadAsync(8192);

            if (count > 0)
                msg = dr.ReadString(count);

            dr.DetachStream();
            dr.Dispose();

            try
            {
                pdu2 = new PDU(msg);
                ListOfProcedures temp = pdu2.Data.ToObject(typeof(ListOfProcedures));
                foreach (Procedure pro in temp.ProcedureList)
                {
                    _kakecollection.Add(pro);
                }
            }
            catch (Exception e) { recievedMessage.Text = "Failed to cast incomming message to usable type: PDU"; }
        }
    }
}
