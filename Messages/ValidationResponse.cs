namespace myfancyproj.Messages
{
    public class ValidationResponse
    {
        public ValidationResponse(int code, string message)
        {
            StatusCode = code;
            StatusMessage = message;
        }

        public int StatusCode { get; }

        public string StatusMessage { get; }
    }
}