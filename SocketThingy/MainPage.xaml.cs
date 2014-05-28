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
using Windows.Networking.Connectivity;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SocketThingy
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary> 158.37.230.132
    public sealed partial class MainPage : Page
    {
        StreamSocket socket = new StreamSocket();
        static HostName localHost;
        static HostName remoteHost = new HostName("198.168.1.123");
        static string socketString = "1337";
        PDU pdu2;
        public Login _user;
        ListOfProcedures temp = new ListOfProcedures(false);
        public Procedure tempProcedure = new Procedure(false);
        
        static PDU pdu = new PDU()
        {
            MessageID = (int)CommandMessageID.SendAllProcedures,
            MessageDescription = "Server Please, send me all the procedures.",
            MessageType = "Command",
            Source = "Demo.Client",
            Data = new JObject()
        };
        public void CurrentIPAddress()
        {
            ConnectionProfile profile = NetworkInformation.GetInternetConnectionProfile();

            if (profile != null && profile.NetworkAdapter != null)
            {
                HostName hostname = getHostName(profile);

                if (hostname != null)
                {
                    // the ip address
                    localHost = new HostName(hostname.CanonicalName);
                }
            }
        }

        private HostName getHostName(ConnectionProfile profile)
        {
            HostName hostname = NetworkInformation.GetHostNames().SingleOrDefault(
                            hn => (
                                hn.IPInformation != null
                                && hn.IPInformation.NetworkAdapter != null
                                && hn.IPInformation.NetworkAdapter.NetworkAdapterId
                                == profile.NetworkAdapter.NetworkAdapterId
                            )
            );

            return hostname;
        }
        public static bool DEBUG = true;

        // StreamSocketListener tcpListener = new StreamSocketListener();
        // private List<StreamSocket> _connections = new List<StreamSocket>();
        
        public MainPage()
        {
            
            this.InitializeComponent();
            CurrentIPAddress();
            recievedMessage.Text = "Please login with valid username and password";
            

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
                temp = pdu2.Data.ToObject(typeof(ListOfProcedures));
                foreach (Procedure pro in temp.ProcedureList)
                {
                    _kakecollection.Add(pro);
                }
                LoginButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                LoginButton.IsEnabled = false;
            }
            catch (Exception e) { recievedMessage.Text = "Failed to cast incomming message to usable type: PDU"; }
        }

        private async void sendData(PDU k)
        {
            var dr = new DataWriter(socket.OutputStream);
            
            //var len = dr.MeasureString(pdu.ToJson());
            String message = k.ToJson();
            //dr.WriteInt32((int)len);
            dr.WriteString(k.ToJson());
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


        public void ListViewItem_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var MySP = sender as StackPanel;
            var MyTextBlock = MySP.Children[2] as TextBlock;
            string tempName = MyTextBlock.Text;
            
            foreach (Procedure pro in temp.ProcedureList)
            {
                if (pro.Description == tempName)
                {
                    tempProcedure = pro;
                }
            }

            SequencePage Seq = new SequencePage(gridMainPage, tempName, pdu2, tempProcedure, socket);
            gridMainPage.Children.Add(Seq);
        }

        private void sendText_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
        


        
        public async void nowConnecting()
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
                    
                    recievedMessage.Text = "Welcome, you are now connected";
                }
                catch (Exception ex)
                {

                    connected = false;
                    remotePort++;
                    recievedMessage.Text = "Changed to free port: " + remotePort;

                }
            }
            
            PDU authenticatePDU = new PDU()
            {
                MessageID = (int)CommandMessageID.LoginAttempt,
                MessageDescription = "Server Please, check and authenticate this user",
                MessageType = "Command",
                Source = "Demo.Client",
                Data = JObject.FromObject(_user)
            };
            sendData(authenticatePDU);
        }
        private void loginUser()
        {
            _user = new Login() { Username = usernameText.Text, Password = passwordText.Password };
            try { nowConnecting();
            }
            catch(Exception e) { recievedMessage.Text = e.ToString(); }
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            loginUser();
        }

        private void changeIPWindow(object sender, RoutedEventArgs e)
        {
            ipPopup.Visibility = Windows.UI.Xaml.Visibility.Visible;
            IPBorder.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        private void IPChange_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                remoteHost = new HostName(IPText.Text);
                ipPopup.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                IPBorder.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            catch (Exception r) { IPrecieveMessage.Text = r.ToString(); }
        }
        }
    }

