using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Demo.Protocol;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using Windows.Networking.Sockets;
using Windows.Networking;
using Windows.Storage.Streams;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SocketThingy
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SequencePage : Page
    {
        static public ObservableCollection<Execution> _executionCollection = new ObservableCollection<Execution>();
        static public ObservableCollection<Step> _stepCollection = new ObservableCollection<Step>();
        public Panel panel2;
        public PDU tempPDU;
        string procedure;
        public Procedure localProcedure = new Procedure(false);
        static HostName localHost = new HostName("192.168.1.250");
        static HostName remoteHost = new HostName("192.168.1.123");
        static string socketString = "1337";
        StreamSocket newSocket = new StreamSocket();
        PDU pdu2 = new PDU();
        JObject runFile;
        
        PDU askForSteps = new PDU()
        {
            MessageID = (int)CommandMessageID.StartExecution,
            MessageDescription = "Server please, find my unique",
            MessageType = "Command",
            Source = "Demo.Client",
            Data = new JObject()
        };
                  
        public SequencePage(Panel tempPanel, String procedureName, PDU pdu, Procedure tempProcedure, StreamSocket tempSocket)
        {
            newSocket = tempSocket;
            localProcedure = tempProcedure;
            procedure = procedureName;
            tempPDU = pdu;
            panel2 = tempPanel;
            this.InitializeComponent();
            procedureTextBlock.Text = procedureName;

            loadSequences();
        }
        private void loadSequences()
        {
            ListOfProcedures tempList = tempPDU.Data.ToObject(typeof(ListOfProcedures));
            _executionCollection.Clear();
            foreach (Execution exe in localProcedure.Executionlist)
            {
                _executionCollection.Add(exe);
            }
        }
        public ObservableCollection<Execution> executionCollection
        {
            get { return _executionCollection; }
        }
        public ObservableCollection<Step> stepCollection
        {
            get { return _stepCollection; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            panel2.Children.Remove(this);
        }

        private void StackPanel_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var mySP = sender as StackPanel;
            
            TextBlock tempString = mySP.Children[0] as TextBlock;
            string stringer = tempString.Text;

            foreach (Execution exe in localProcedure.Executionlist)
            {
                if (exe.Description == stringer)
                {
                    askForSteps.Data.SequenceFileName = exe.CurrentSequence.SequenceFileName;
                }
            }

            sendData(askForSteps);
            
        }
        
            
        
        private async void sendData(PDU k)
        {
            var dr = new DataWriter(newSocket.OutputStream);
            
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
            DataReader dr = new DataReader(newSocket.InputStream);
            dr.InputStreamOptions = InputStreamOptions.Partial;
            string msg = null;
            try
            {
                var count = await dr.LoadAsync(8192);
                if (count > 0)
                    msg = dr.ReadString(count);
            }
            catch { }
            dr.DetachStream();
            dr.Dispose();

            try
            {
                pdu2 = new PDU(msg);
                Execution temp = pdu2.Data.ToObject(typeof(Execution));
                _stepCollection.Clear();
                foreach (Step steps in temp.CurrentSequence.StepList)
                {
                    _stepCollection.Add(steps);
                }
            }

            catch (Exception e) { }
            Execution temp2 = pdu2.Data.ToObject(typeof(Execution));
            if (temp2.State == Execution.ExecutionStates.FINISHED || temp2.State == Execution.ExecutionStates.TERMINATED)
            {
                PDU pdu3 = new PDU(){
                    MessageID = (int)CommandMessageID.ResetTS,
                    MessageDescription = "Server please, reset TS",
                    MessageType = "Command",
                    Source = "Demo.Client",
                    Data = new JObject()
                };
                sendData(pdu3);
            } else { recieveData(); }
            
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            PDU runSteps = new PDU()
            {
                MessageID = (int)CommandMessageID.StartExecution,
                MessageDescription = "Server please, start execution",
                MessageType = "Command",
                Source = "Demo.Client",
                Data = new JObject()
            };

        }
    }
}
