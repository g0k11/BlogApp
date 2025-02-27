using Ardalis.Result;

namespace BlogApp.Common.Responses
{
    public static class ResponseExtensions
    {
        public static Result<T> ToResult<T>(this Response<T> response)
        {
            return response.Flag
                ? Result<T>.Success(response.Data)
                : Result<T>.Error(response.Message);
        }

        public static Response<T> ToResponse<T>(this Result<T> result)
        {
            if (result.IsSuccess)
            {
                return new Response<T>(true, "İşlem başarılı", result.Value);
            }
            
            return new Response<T>(false, string.Join(", ", result.Errors));
        }
        
        public static Response<object> ToResponse(this Result result)
        {
            if (result.IsSuccess)
            {
                return new Response<object>(true, "İşlem başarılı");
            }
            
            return new Response<object>(false, string.Join(", ", result.Errors));
        }
    }

    public record Response<T>(bool Flag = false, string Message = "", T Data = default!);
}
