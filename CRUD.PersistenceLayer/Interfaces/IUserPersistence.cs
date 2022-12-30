using CRUD.Entities.Request;
using CRUD.Entities.Response.User;

namespace CRUD.PersistenceLayer.Interfaces;
public interface IUserPersistence
{
	Task<List<UserResponse>> GetAllUser();

	Task InsertUpdateDeleteUser(UserRequest userRequest);
}
