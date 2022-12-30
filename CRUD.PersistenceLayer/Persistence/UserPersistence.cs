using CRUD.Entities.Request;
using CRUD.Entities.Response.User;
using CRUD.PersistenceLayer.Interfaces;
using CRUD.Utility.CommonHelper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace CRUD.PersistenceLayer.Persistence;

public class UserPersistence : IUserPersistence
{
	#region Variable Declaration

	private readonly SqlHelper _sqlHelper;

	#endregion

	#region Constructor

	/// <summary>
	/// Constructor - initialization of private variables
	/// </summary>
	/// <param name="configuration"></param>
	public UserPersistence(IConfiguration configuration)
	{
		_sqlHelper = new SqlHelper(configuration);
	}

	#endregion

	#region Public Methods (Interface Implementation)

	public async Task<List<UserResponse>> GetAllUser()
	{

		var result = await _sqlHelper.GetDataViaStoredProcedure<UserResponse>(StoredProcedureName.GetAllUsers);
		return result.ToList();
	}

	public async Task InsertUpdateDeleteUser(UserRequest userRequest)
	{
		var queryParameter = new DynamicParameters();
		
		queryParameter.Add("@ID", userRequest.Id);
		queryParameter.Add("@FirstName", userRequest.FirstName);
		queryParameter.Add("@LastName", userRequest.LastName);
		queryParameter.Add("@PhoneNumber", userRequest.PhoneNumber);
		queryParameter.Add("@Email", userRequest.Email);
		queryParameter.Add("@Gender", userRequest.Gender);
		queryParameter.Add("@Operation", userRequest.Operation);

		await _sqlHelper.InsertUpdateData(StoredProcedureName.InsertUpdateDeleteUsers, queryParameter);
	}

	#endregion
}
