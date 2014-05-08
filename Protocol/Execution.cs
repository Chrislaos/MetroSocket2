using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.Protocol
{
    public class Execution : DomainObject
    {

        public enum ExecutionStates { LOADED, STARTED, RESUMED, PAUSED, FINISHED, TERMINATED, CAKE = 1337 }

        private Step _currentStep = null;

        public Execution(bool test, String desc)
        {
            if (test)
            {
                CurrentStep = null;
                CurrentSequence = new Sequence(test);
                Description = desc;
                State = ExecutionStates.LOADED;


            }

        }
        public Execution() { }

        public Step CurrentStep
        {
            get
            {
                return _currentStep;
            }
            set
            {
                _currentStep = value;
                OnPropertyChanged("CurrentStep");
            }

        }

        private Sequence _currentSequence = new Sequence(false);
        public Sequence CurrentSequence
        {
            get
            {
                return _currentSequence;
            }
            set
            {
                _currentSequence = value;
                OnPropertyChanged("CurrentSequence");
            }

        }

        private ExecutionStates _state = ExecutionStates.CAKE;
        public ExecutionStates State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                OnPropertyChanged("State");
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

    }
}
