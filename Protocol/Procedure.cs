using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Protocol
{
    public class Procedure : DomainObject
    {
        private ObservableCollection<Execution> _executionlist = new ObservableCollection<Execution>();
        public ObservableCollection<Execution> Executionlist
        {
            get
            {
                return _executionlist;
            }
            set
            {
                _executionlist = value;
                OnPropertyChanged("ExecutionList");
            }
        }

        public Procedure(bool test)
        {
            if (test)
            {
                Executionlist = new ObservableCollection<Execution>();
                Executionlist.Add(new Execution(test, "First Execution"));
                Executionlist.Add(new Execution(test, "Second Execution"));
                Name = "TestProcedure.pro";
                Description = "This procedure is generated for testing purposes";
                Date = DateTime.Now;





            }
        }

        private String _name = null;
        public String Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private String _description = null;
        public String Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        private DateTime _date = DateTime.Today;
        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
                OnPropertyChanged("Date");
            }

        }
    }
}
