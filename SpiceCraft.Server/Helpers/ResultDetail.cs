namespace SpiceCraft.Server.Helpers;

public class ResultDetail<T>
{
    public string Message { get; set; }
    public T Data { get; set; }
    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }
    public bool Notify { get; set; }
}