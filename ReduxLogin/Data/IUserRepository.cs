using System;
using ReduxLogin.Models;

namespace ReduxLogin.Data
{
	public interface IUserRepository
	{
		User Create(User user);
		User GetByUserName(string userName);
        User GetById(int id);
		User Update(int id);
    }
}

