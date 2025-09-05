using System.Net;
using DeliveryAPI.Application.Exceptions.Base;

namespace DeliveryAPI.Application.Exceptions.Common;

 public class  ForbiddenException(string mess) : BaseException(mess, HttpStatusCode.Conflict);