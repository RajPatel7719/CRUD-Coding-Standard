// ReSharper disable IdentifierTypo
namespace CRUD.Dtos;

public class ExceptionResponse : Exception
{
	public ExceptionResponse(string exceptionMessage)
	{
		ExceptionMessage = exceptionMessage;
	}

	public ExceptionResponse(Exception exception, bool isWarning, string exceptionMessage)
	{
		StatusCode = exception.HResult;
		ExceptionMessage = exceptionMessage + "Original Message - " + exception.Message;
		ExceptionStackTrace = exception.StackTrace;
		IsWarning = isWarning;
	}

	public int StatusCode { get; set; }

	public string ExceptionMessage { get; set; }

	public string? ExceptionStackTrace { get; set; }

	public bool IsWarning { get; set; }
}