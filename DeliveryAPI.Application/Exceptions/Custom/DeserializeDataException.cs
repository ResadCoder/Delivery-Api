using System.Net;
using DeliveryAPI.Application.Exceptions.Base;

namespace DeliveryAPI.Application.Exceptions.Common;
    public class DeserializeDataException(
        string mess = "Could not deserialize response!",
        HttpStatusCode status = HttpStatusCode.InternalServerError)
        : BaseException(mess, status);

