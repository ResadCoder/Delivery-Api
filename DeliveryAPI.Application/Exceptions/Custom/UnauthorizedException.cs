using System.Net;
using DeliveryAPI.Application.Exceptions.Base;

namespace DeliveryAPI.Application.Exceptions.Common;
public class UnauthorizedException(
        string mess = "Unauthorized",
        HttpStatusCode status = HttpStatusCode.Unauthorized)
        : BaseException(mess, status);

