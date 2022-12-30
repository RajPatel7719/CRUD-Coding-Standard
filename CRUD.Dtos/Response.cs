using CRUD.Utility.Enums;

namespace CRUD.Dtos;
public class Response<T>
{
	// ReSharper disable UnusedAutoPropertyAccessor.Global
	public T Result { get; set; } = default!;

	public ResponseStatus Status { get; set; }

	public string ResponseStatus => Status.ToString();

	public ExceptionResponse Exception { get; set; } = null!;
}