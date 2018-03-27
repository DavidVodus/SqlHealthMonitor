using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHealthMonitor.BLL.Models.WebPages
{

    //
    // Summary:
    //     Encapsulates information for handling an error that was thrown by an action method.
    public class ErrorPageViewModel:PageViewModelBase
    {
        public ErrorPageViewModel(Exception exception, string controllerName=null, string actionName=null)
        {
            this.Exception = exception;ControllerName=  controllerName ?? this.ControllerName; this.StartActionName = actionName ?? this.StartActionName; }
      
        //
        // Summary:
        //     Gets or sets the exception object.
        //
        // Returns:
        //     The exception object.
        public Exception Exception { get; }
    }
}
