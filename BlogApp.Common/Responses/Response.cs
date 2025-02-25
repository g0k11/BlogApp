namespace BlogApp.Common.Responses
{
        public record Response(bool Flag = false, string Message = null!);
        public record Response<T>(bool Flag = false, string Message = null!, T Data = default!);
}
