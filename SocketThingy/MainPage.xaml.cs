using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Networking.Sockets;
using Windows.Networking;
using Newtonsoft.Json.Linq;
using Demo.Protocol;
using Windows.Storage.Streams;
using System.Collections.ObjectModel;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SocketThingy
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        StreamSocket socket = new StreamSocket();
        static HostName localHost = new HostName("158.37.233.3");
        static HostName remoteHost = new HostName("158.37.230.147");
        static string socketString = "1337";
        PDU pdu2;
        
        PDU pdu = new PDU()
        {
            MessageID = (int)CommandMessageID.SendAllProcedures,
            MessageDescription = "Server Please, load the sequencefile I specified as part of this message.",
            MessageType = "Command",
            Source = "Demo.Client",
            Data = new JObject()
        };
        public static bool DEBUG = true;

        // StreamSocketListener tcpListener = new StreamSocketListener();
        // private List<StreamSocket> _connections = new List<StreamSocket>();
        private bool connecting = false;
        private String recievedText;
        public String RecievedText
        {
            get
            {
                return recievedText;
            }
        }

        public MainPage()
        {
            this.InitializeComponent();

            

        }
        static public ObservableCollection<Procedure> _kakecollection = new ObservableCollection<Procedure>();
        // message fikk -> PDU object, 
        public ObservableCollection<Procedure> kakecollection
        {
            get { return _kakecollection; }
        }
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
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

        private async void sendData()
        {
            var dr = new DataWriter(socket.OutputStream);
            
            //var len = dr.MeasureString(pdu.ToJson());
            String message = pdu.ToJson();
            //dr.WriteInt32((int)len);
            dr.WriteString(pdu.ToJson());
            var ret = await dr.StoreAsync();
            
            recieveData();


            dr.DetachStream();
          

            
        }


        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            if (DEBUG)
            {
                socket.Dispose();
                Application.Current.Exit();
            }
            else
            {
                // do something legal
            }
        }



        



        private void ListViewItem_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var MySP = sender as StackPanel;
            var MyTextBlock = MySP.Children[1] as TextBlock;
            string tempName = MyTextBlock.Text;

            SequencePage Seq = new SequencePage(gridMainPage, tempName, pdu2);
            gridMainPage.Children.Add(Seq);
        }

        private void sendText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void Coonnect3_Click(object sender, RoutedEventArgs e)
        {
            bool connected = false;
            int remotePort = 1337;
            while (!connected)
            {
                
                try
                {
                    EndpointPair connection = new EndpointPair(localHost, remotePort.ToString(), remoteHost, socketString);
                    await socket.ConnectAsync(connection);
                    connected = true;
                    Coonnect3.IsEnabled = false;
                    Coonnect3.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
                catch (Exception ex)
                {
                    
                    connected = false;
                    remotePort++;
                    recievedMessage.Text = "Changed to free port: " + remotePort;
                    
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            sendData();
        }

        private void SendData_Click(object sender, RoutedEventArgs e)
        {
            sendData();
        }
        }
    }

