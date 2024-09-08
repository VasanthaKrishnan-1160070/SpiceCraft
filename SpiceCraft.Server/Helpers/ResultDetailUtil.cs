namespace SpiceCraft.Server.Helpers;

public class ResultDetailUtil
{
    public ResultDetail<T> Success<T>(T data, string message = "", bool notify = false)
    {
        return new ResultDetail<T>()
        {
            Data = data,
            IsSuccess = true,
            Notify = notify,
            Message = message
        };
    }

    public ResultDetail<IEnumerable<T>> SuccessList<T>(IEnumerable<T> dataList, string message = "", bool notify = false)
    {
        return new ResultDetail<IEnumerable<T>>()
        {
            Data = dataList,
            IsSuccess = true,
            Notify = notify,
            Message = message
        };
    }

    public ResultDetail<T> Error<T>(string message = "", bool notify = true)
    {
        return new ResultDetail<T>() { IsSuccess = false, Notify = notify, Message = message};
    }

    public ResultDetail<T> Error<T>(T? data, string message = "", bool notify = true)
    {
        return new ResultDetail<T>() { IsSuccess = false, Notify = notify, Message = message };
    }

    public ResultDetail ErrorResponse(string message = "")
    {
        return new ResultDetail() { IsSuccess = false, Notify = true, Message = message };
    }
}