using Ardalis.Result;

namespace BlogApp.Common.Responses
{
    public static class ResponseExtensions
    {
        public static Result<T> ToResult<T>(this Response<T> response)
        {
            return response.Flag
                ? Result.Success(response.Data)
                : Result.Error(response.Message);
        }
    }

    public record Response<T>(bool Flag = false, string Message = "", T Data = default!);
}
