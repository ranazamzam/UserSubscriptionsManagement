using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace UserSubscriptionsManagement.Utility
{
    public class GlobalErrorHandler : IErrorHandler
    {
        /// <summary>
        // HandleError. Log an error, then allow the error to be handled as usual.  
        // Return true if the error is considered as already handled  
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool HandleError(Exception error)
        {
            return true;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            var exception = new FaultException(
               string.Format(@"An exception occured {0}Method: {1}{2}Message: {3}",
                                        Environment.NewLine, error.TargetSite.Name, 
                                        Environment.NewLine, error.Message));

            var msgFault = exception.CreateMessageFault();
            fault = Message.CreateMessage(version, msgFault, exception.Action);
        }
    }
}
