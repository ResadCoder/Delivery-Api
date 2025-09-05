using System.Net;
using DeliveryAPI.Application.Exceptions.Base;

namespace DeliveryAPI.Application.Exceptions.Common;




public class NotPendingException(string mess) : BaseException(mess, HttpStatusCode.Conflict);