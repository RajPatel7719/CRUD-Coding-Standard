namespace CRUD.Dtos.Request;

public class UserRequest : BaseRequest
{
	public int Id { get; set; }

	public string FirstName { get; set; } = null!;

	public string LastName { get; set; } = null!;

	public string PhoneNumber { get; set; } = null!;

	public string Email { get; set; } = null!;

	public bool Gender { get; set; }

	public int Operation { get; set; }
}
