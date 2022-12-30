using System.IdentityModel.Tokens.Jwt;

namespace CRUD.Utility.ExtensionMethod;

// ReSharper disable UnusedType.Global
public static class JwtTokenHelper
{
	public static bool DecodeJwtToken(this string token, string partnerId)
	{
		var handler = new JwtSecurityTokenHandler();
		var jwtSecurityToken = handler.ReadJwtToken(token);
		var name = jwtSecurityToken.Claims.First(claim => claim.Type == "unique_name").Value;
		return name == partnerId;
	}
}
