using FilmsCatalog.Data;
using FilmsCatalog.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FilmsCatalog.Helpers
{
	public static class UserHelpers
	{
		public static Guid? GetUserIdIfRegister(ClaimsPrincipal user, UserManager<User> userManager)
		{
			var isRegistered = Guid.TryParse(userManager.GetUserId(user), out var userId);

			return isRegistered ? userId : null;
		}
	}
}
