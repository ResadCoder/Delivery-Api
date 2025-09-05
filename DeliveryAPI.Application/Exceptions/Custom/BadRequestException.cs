
using System.Net;
using DeliveryAPI.Application.Exceptions.Base;

namespace DeliveryAPI.Application.Exceptions.Common;
    public class BadRequestException(string message = "Bad Request", HttpStatusCode status = HttpStatusCode.BadRequest)
        : BaseException(message, status);

