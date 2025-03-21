namespace testudv.Application.Exceptions;

public class VkApiException : Exception
{
    public int ErrorCode { get; }
    public string ErrorMessage { get; }

    public VkApiException(int errorCode, string errorMessage)
        : base($"VK API Error: {errorMessage} (Code: {errorCode})")
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }
}