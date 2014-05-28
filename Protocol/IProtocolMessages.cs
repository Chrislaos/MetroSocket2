using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.Protocol
{
  

    public enum CommandMessageID
    {
        LoadSequenceFile = 1501,
        StartExecution = 1502,
        StopExecution = 1503,
        PauseExecution = 1504,
        ResumeExecution = 1505,
        SendAllProcedures = 1601,
        LoginAttempt = 1801,
        ResetTS = 1901
    }

    public enum EventMessageID
    {
        SequenceFileLoaded = 1301,
        ExecutionStarted = 1302,
        ExecutionStopped = 1303,
        ExecutionPaused = 1304,
        ExecutionResumed = 1305,
        TestPassed = 1306,
        TestFailed = 1307,
        ExecutionChanged = 1308,
        ProceduresSent = 1701,
        AuthAccept = 1702,
        AuthDenied = 1703
    };
}
