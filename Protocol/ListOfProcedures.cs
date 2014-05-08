using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Demo.Protocol
{
    public class ListOfProcedures : DomainObject
    {
        private ObservableCollection<Procedure> _procedureList = new ObservableCollection<Procedure>();
        public ObservableCollection<Procedure> ProcedureList
        {
            get
            {
                return _procedureList;
            }

            private set
            {
                _procedureList = value;
                OnPropertyChanged("ProcedureList");
            }
        }

        public ListOfProcedures(bool test)
        {
            if (test)
            {
                ProcedureList = new ObservableCollection<Procedure>();
                ProcedureList.Add(new Procedure(true) {Name = "Test1", Description = "This is the first lol procedure", Date = DateTime.Now});
                ProcedureList.Add(new Procedure(true) {Name = "test2", Description = "This is the second lol procedure", Date = DateTime.Now});


            }
        }

    }
}
