using CRUD.BusinessLayer.Interfaces;
using CRUD.BusinessLayer.Services;
using CRUD.PersistenceLayer.Interfaces;
using CRUD.PersistenceLayer.Persistence;

namespace CRUD.API;
public static class DependencyConfiguration
{
	public static void DependencySetting(this IServiceCollection services)
	{
		services.AddTransient<IUserService, UserService>();
		services.AddTransient<IUserPersistence, UserPersistence>();
	}
}
