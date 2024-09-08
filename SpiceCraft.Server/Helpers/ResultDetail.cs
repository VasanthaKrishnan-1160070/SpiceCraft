using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace SpiceCraft.Server.Helpers;

public class ResultDetail<T>
{
    public string Message { get; set; } = "";
    public T? Data { get; set; }
    public IEnumerable<T>? DataList { get; set; }
    public bool IsSuccess { get; set; }    
    public bool Notify { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}

public class ResultDetail : ResultDetail<bool>
{

}