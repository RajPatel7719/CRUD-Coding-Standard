using System.Data;
using System.Data.SqlClient;
using CRUD.Utility.CommonHelper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace CRUD.PersistenceLayer;
public class SqlHelper
{
	private readonly IConfiguration _config;

	public SqlHelper(IConfiguration config)
	{
		_config = config;
	}

	private string ConnectionString => _config.GetConnectionString(Constant.CrudConnectionString)!;

	public async Task<IEnumerable<T>> GetDataViaStoredProcedure<T>(string storedProcedureName, DynamicParameters parameters = null!)
	{
		await using var connection = new SqlConnection(ConnectionString);
		await connection.OpenAsync().ConfigureAwait(false);

		var result = await connection.QueryAsync<T>(storedProcedureName, parameters, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
		return result;
	}

	public async Task<long> InsertData(string storedProcedureName, DynamicParameters parameters)
	{
		await using var connection = new SqlConnection(ConnectionString);
		await connection.OpenAsync().ConfigureAwait(false);

		return await connection.ExecuteScalarAsync<long>(storedProcedureName, parameters, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
	}

	public async Task InsertUpdateData(string storedProcedureName, DynamicParameters parameters)
	{
		await using var connection = new SqlConnection(ConnectionString);
		await connection.OpenAsync().ConfigureAwait(false);

		await connection.ExecuteAsync(storedProcedureName, parameters, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
	}

	public async Task UpdateById<T>(string storedProcedureName, long id, T model)
	{
		await using var connection = new SqlConnection(ConnectionString);
		await connection.OpenAsync().ConfigureAwait(false);

		await connection.ExecuteAsync(storedProcedureName, new { model, Id = id }, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
	}

	public async Task DeleteById(string storedProcedureName, long id)
	{
		await using var connection = new SqlConnection(ConnectionString);
		await connection.OpenAsync().ConfigureAwait(false);
		await connection.ExecuteAsync(storedProcedureName, id, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
	}

	public async Task<T> GetFirstDataViaStoredProcedure<T>(string storedProcedureName, DynamicParameters parameters = null!) where T : class
	{
		await using var connection = new SqlConnection(ConnectionString);
		await connection.OpenAsync().ConfigureAwait(false);

		var result = await connection.QueryAsync<T>(storedProcedureName, parameters, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
		return result.FirstOrDefault()!;
	}
}
