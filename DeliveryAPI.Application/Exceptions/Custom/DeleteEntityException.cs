using System.Net;
using DeliveryAPI.Application.Exceptions.Base;

namespace DeliveryAPI.Application.Exceptions.Common;
    public class DeleteEntityException(string mess, HttpStatusCode status = HttpStatusCode.BadRequest)
        : BaseException(mess, status);

