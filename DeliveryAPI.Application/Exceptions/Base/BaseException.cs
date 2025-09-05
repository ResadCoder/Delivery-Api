using System.Net;

namespace DeliveryAPI.Application.Exceptions.Base;


    public class BaseException(string mess, HttpStatusCode status) : Exception(mess)
    {
        public HttpStatusCode Status { get; } = status;
    }
