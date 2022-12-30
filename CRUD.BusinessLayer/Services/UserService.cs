using CRUD.BusinessLayer.Interfaces;
using CRUD.Dtos.FluentValidation;
using CRUD.PersistenceLayer.Interfaces;
using FluentValidation;
using UserRequest = CRUD.Dtos.Request.UserRequest;
using UserResponse = CRUD.Dtos.Response.User.UserResponse;

namespace CRUD.BusinessLayer.Services;
public class UserService : IUserService
{
	#region Variable Declaration

	private readonly IUserPersistence _userPersistence;

	#endregion

	#region Constructor

	/// <summary>
	/// Constructor - Initialization of private variables
	/// </summary>
	/// <param name="userPersistence"></param>
	public UserService(IUserPersistence userPersistence)
	{
		_userPersistence = userPersistence;
	}

	#endregion

	#region Public Methods (Interface Implementation)

	/// <summary>
	/// Get list of users
	/// </summary>
	/// <returns></returns>
	public async Task<List<UserResponse>> GetAllUser()
	{
		#region Validation

		//UserValidator validator = new();
		//validator.ValidateAndThrow(request);

		#endregion

		// DTO to Entity Convert
		//var attorneyGrid = ConvertAttorneyGridRequest();

		// Call SP to get list 
		var list = await _userPersistence.GetAllUser();

		// Return list of data, Convert Entity to DTO object
		return ConvertUserResponse(list);
	}

	public async Task InsertUpdateDeleteUser(UserRequest userRequest)
	{
		#region Validation

		UserValidator validator = new();
		validator.ValidateAndThrow(userRequest);

		#endregion

		// DTO to Entity Convert
		var user = ConvertUserRequest(userRequest);

		// Call SP to get list 
		 await _userPersistence.InsertUpdateDeleteUser(user);
	}

	#endregion

	#region Private Methods

	/// <summary>
	/// Convert response Entity to DTO
	/// </summary>
	/// <param name="response"></param>
	/// <returns></returns>
	private static List<UserResponse> ConvertUserResponse (List<Entities.Response.User.UserResponse> response)
	{
		List<UserResponse> list = new();

		// ReSharper disable ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
		foreach (var item in response)
		{
			UserResponse userResponse = new()
			{
				Id = item.Id,
				Email = item.Email,
				FirstName = item.FirstName,
				LastName = item.LastName,
				Gender = item.Gender,
				PhoneNumber = item.PhoneNumber
			};

			list.Add(userResponse);
		}
		return list;
	}

	/// <summary>
	/// Convert request object from DTO to entity
	/// </summary>
	/// <param name="userRequest"></param>
	/// <returns></returns>
	private static Entities.Request.UserRequest ConvertUserRequest(UserRequest userRequest)
	{
		return new Entities.Request.UserRequest
		{
			Id = userRequest.Id,
			FirstName = userRequest.FirstName,
			LastName = userRequest.LastName,
			PhoneNumber = userRequest.PhoneNumber,
			Email = userRequest.Email,
			Gender = userRequest.Gender,
			Operation = userRequest.Operation
		};
	}

	#endregion
}
