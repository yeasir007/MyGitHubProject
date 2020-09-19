using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGitHubProject.Enums
{
    public enum LogLevel
    {
        DEBUG,  //Additional information about application behavior for cases when that information is necessary to diagnose problems
        INFO,   //Application events for general purposes
        WARN,   //Application events that may be an indication of a problem
        ERROR,  //Typically logged in the catch block a try/catch block, includes the exception and contextual data
        FATAL,  //A critical error that results in the termination of an application
        TRACE   //Used to mark the entry and exit of functions, for purposes of performance profiling
    }
}
