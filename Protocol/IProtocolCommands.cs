using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.Protocol
{
    /**
     *  From the protocol reference's list of commands: 
     *      https://pm.salmon.im/projects/seagull/wiki/Protocol#Command-messages
     *  
     *  The commands that the Client in operator mode can send to the server.
     *  Note, the server doesn't reply in responses to a command. But one or more events may be triggered
     *  and sent back to the client as an effect of running a command.
     * 
     * */
    public interface ICommands
    {
        void LoadSequenceFile(String seqfile);
        void StartExecution(String seqfile);
        void ResumeExecution(String seqfile);
        void PauseExecution(String seqfile);
        void StopExecution(String seqfile);
        void SendAllProcedures();

    }


     /**
      *  From the protocol reference's list of Events
      *       https://pm.salmon.im/projects/seagull/wiki/Protocol#Event-messages
      *
      *    
      * 
      * */
     public interface IProtocolEvents {

         event ProtocolEventHandler SequenceFileLoaded;
         event ProtocolErrorEventHandler SequenceFileLoadFailed;
         event ProtocolEventHandler ExecutionStarted;
         event ProtocolErrorEventHandler ExecutionStartFailed;
         event ProtocolEventHandler ExecutionPaused;
         event ProtocolErrorEventHandler ExecutionPauseFailed;
         event ProtocolEventHandler ExecutionResumed;
         event ProtocolErrorEventHandler ExecutionResumeFailed;
         event ProtocolEventHandler ExecutionStopped;
         event ProtocolErrorEventHandler ExecutionStopFailed;
        
        //general error.
         event ProtocolErrorEventHandler ExecutionFailed;
        
    }

    /**
     *  General Purpose 
     * */
    public delegate void ProtocolErrorEventHandler(object source, string Message);
    public delegate void ProtocolEventHandler(object source, string Message);
    public delegate void NetworkErrorHandler(object source, PDU Message, Exception e);
}

