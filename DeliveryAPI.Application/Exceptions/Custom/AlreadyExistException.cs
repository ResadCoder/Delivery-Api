
using System.Net;
using DeliveryAPI.Application.Exceptions.Base;


namespace DeliveryAPI.Application.Exceptions.Common;

    public class AlreadyExistException(string mess) : BaseException(mess, HttpStatusCode.BadRequest);

