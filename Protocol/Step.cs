using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.Protocol
{
    public class Step : DomainObject
    {
        public enum StatusTypes { PASSED, FAILED, DONE, SKIPPED, CAKE = 1337 } ;

        public Step() { }

        public Step(bool test, int index)
        {
            if (test)
            {
                SequenceRef = null;
                Type = "pass/fail";
                Status = StatusTypes.CAKE;
                Name = "testStep";
                Description = "kakakakakakakakakak";
                Settings = "blahaha";
                Index = index;
                Data = null;

            }

        }

        private Sequence _sequenceRef = null;
        public Sequence SequenceRef
        {
            get
            {
                return _sequenceRef;
            }
            set
            {
                _sequenceRef = value;
                OnPropertyChanged("SequenceRef");
            }
        }

        private String _type = String.Empty;
        public String Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
                OnPropertyChanged("Type");
            }
        }

        private StatusTypes _status = StatusTypes.CAKE;
        public StatusTypes Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        private String _name = String.Empty;
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

        private String _settings = String.Empty;
        public String Settings
        {
            get
            {
                return _settings;

            }
            set
            {
                _settings = value;
                OnPropertyChanged("Settings");
            }
        }

        private object _data = null;
        public object Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }

        private int _index = -1;
        public int Index
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
                OnPropertyChanged("Index");
            }
        }
    }
}
