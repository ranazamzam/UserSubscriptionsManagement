using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using UserSubscriptionsManagement.Contracts.DataContracts;

namespace UserSubscriptionsManagement.WebAPI.Filters
{
    public class HandleExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //base.OnException(actionExecutedContext);

            HttpResponseMessage response = null;

            if (actionExecutedContext.Exception is FaultException<DataNotFoundFaultException>)
            {
                response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(((FaultException<DataNotFoundFaultException>)actionExecutedContext.Exception).Detail.Message),
                    ReasonPhrase = "Data Not Found"
                };
            }
            else if (actionExecutedContext.Exception is FaultException<BusinessRuleFaultException>)
            {
                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(((FaultException<BusinessRuleFaultException>)actionExecutedContext.Exception).Detail.Message),
                    ReasonPhrase = "Business rules cannot be violated"
                };
            }
            else if (actionExecutedContext.Exception is FaultException)
            {
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(((FaultException)actionExecutedContext.Exception).Message),
                    ReasonPhrase = "An internal error occured"
                };
            }
            else
            {
                var exceptionMessage = string.Empty;

                if (actionExecutedContext.Exception.InnerException == null)
                {
                    exceptionMessage = actionExecutedContext.Exception.Message;
                }
                else
                {
                    exceptionMessage = actionExecutedContext.Exception.InnerException.Message;
                }

                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("An unhandled exception was thrown by service."),
                    ReasonPhrase = "Internal Server Error."
                };
            }
            actionExecutedContext.Response = response;
        }
    }
}