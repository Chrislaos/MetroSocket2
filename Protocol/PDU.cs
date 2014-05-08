using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Demo.Protocol
{
   public class PDU
    {

       /**
        *   Instantiates a PDU object from a json encoded string.
        *   Could be used to create a PDU object from received network message for instance.
        * */
       public PDU(String jsonStr) {
           dynamic jay = JsonConvert.DeserializeObject(jsonStr);
           
           this.MessageID = (int)jay.MessageID;
           this.MessageDescription = (String)jay.MessageDescription;
           this.MessageType = (String)jay.MessageType;

           this.Data = (JObject) jay.Data;
           
           this.TimeStamp = (DateTime)jay.TimeStamp;
           //this.HASH = (String)jay.HASH;
       }

       public PDU()
       {
          
       }
       public override String ToString()
       {
           // Returning serialized json of this object.
           return (String)JsonConvert.SerializeObject(this, Formatting.Indented);
       }

       /**
        *   Explicit method for converting to json. 
        **/
       public String ToJson()
       {
           return ToString();
       }

       public int MessageID
       {
           get;
           set;
       }

       public String MessageDescription
       {
           get;
           set;
       }

       public String MessageType
       {
           get;
           set;
       }

       private String _source = null;
        public String Source
        {
            get {
                if (_source == null)
                    _source = "Unknown Source";
                return  _source;
            }
            set {
                _source = value;
            }
            
        }

        public dynamic Data
        {
            get;
            set;
        }

        private DateTime _timestamp;
        public DateTime TimeStamp
        {
            get {
                
                    _timestamp = DateTime.Now;

                return _timestamp; }
            set{
                _timestamp = value;

                //format here.
            }
        }

        //private String _HASHIS = null;
        //public String HASH
        //{
        //    get
        //    {
        //        //  The hash may have been set from a remote object deserialization.
        //        // Therefore, 
        //        if (_HASHIS == null)
        //            HashMe();
                
        //        return _HASHIS;
        //    }

        //    set 
        //    {
        //        this._HASHIS = value;
        //    }

        //}

       /**
        *   Hashes the pdu. 
        *   Can be used to explicitly hash the pdu. It is automatically used by the HASH property when retrieving a non existent HASH.
        **/
        //public void HashMe()
        //{
        //    using (MD5 md5Hash = MD5.Create())
        //    {
        //        String toHash = this.MessageDescription + this.MessageID + this.Data;
        //        _HASHIS = Utilitites.GetMd5Hash(md5Hash, toHash);
        //    }
        //}

        //public bool StateDiffersFrom(PDU otherPDU) {
        //    // Comparing the two hash strings
        //    return this.HASH.Equals(((PDU)otherPDU).HASH);
        //}


        //public override bool Equals(object otherPDU)
        //{
        //    // fix, : http://stackoverflow.com/a/17819805

        //    var other = otherPDU as PDU;

        //    if (other == null)
        //        return false;
        //    // Comparing the two hash strings
        //    var typeMatches = GetType().Equals(other.GetType());
        //    var valueMatches = HASH.Equals(other.HASH);

        //    return typeMatches && valueMatches;

        //}



 
       /**
        *       Used by Equals to compare to PDU object value equality.
        *       Returns the generated HASH of each.
        **/
        //public override int GetHashCode()
        //{
        //    return this.HASH.GetHashCode();
        //}

       /**
        *   Returns json formated serilazation of a PDU object. 
        **/
    }
}
