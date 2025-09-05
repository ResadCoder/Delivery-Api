using System.Net;
using DeliveryAPI.Application.Exceptions.Base;
namespace DeliveryAPI.Application.Exceptions.Common;

    public class InvalidIdException(string mess) : BaseException(mess, HttpStatusCode.BadRequest);

