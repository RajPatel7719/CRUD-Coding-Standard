using CRUD.Dtos.Request;
using CRUD.Dtos.Response.User;

namespace CRUD.BusinessLayer.Interfaces;
public interface IUserService
{
	Task<List<UserResponse>> GetAllUser();

	Task InsertUpdateDeleteUser(UserRequest userRequest);
}
