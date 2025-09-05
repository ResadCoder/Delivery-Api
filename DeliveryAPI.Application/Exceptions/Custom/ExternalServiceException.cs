using System.Net;
using DeliveryAPI.Application.Exceptions.Base;

namespace DeliveryAPI.Application.Exceptions.Common;

    public class ExternalServiceException(string mess, HttpStatusCode status = HttpStatusCode.InternalServerError)
        : BaseException(mess, status);

