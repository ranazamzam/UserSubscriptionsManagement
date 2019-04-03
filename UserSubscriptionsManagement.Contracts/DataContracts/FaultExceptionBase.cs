using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UserSubscriptionsManagement.Contracts.DataContracts
{
    [DataContract]
    public class FaultExceptionBase
    {
        /// <summary>
        /// Display the result of the request
        /// </summary>
        [DataMember]
        public bool Result { get; set; }

        /// <summary>
        /// Display a user friendly message
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// Displays a more detailed exception message
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
