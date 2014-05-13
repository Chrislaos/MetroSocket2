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
using System.Collections.ObjectModel;

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
        public SequencePage(Panel tempPanel, String procedureName, PDU pdu)
        {
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
            foreach (Procedure pro in tempList.ProcedureList)
            {
                if (pro.Name == procedure)
                {
                    _executionCollection = pro.Executionlist;
                }
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
            openSteps(stringer);
        }
        private void openSteps(string executionString)
        {
            _stepCollection.Clear();
            foreach (Execution exe in _executionCollection)
            {
                if (exe.Description == executionString)
                {
                    foreach (Step step in exe.CurrentSequence.StepList)
                    {
                        _stepCollection.Add(step);
                    }
                    
                }
            }

        }
    }
}
