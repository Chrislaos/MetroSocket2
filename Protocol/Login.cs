using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Protocol
{
    public class Login
    {

        private String _username = null;
        public String Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
            }
        }

        private String _password = null;
        public String Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        private bool _auth = false;
        public bool Auth
        {
            get
            {
                return _auth;
            }
            set
            {
                _auth = value;
            }
        }

    }
}
