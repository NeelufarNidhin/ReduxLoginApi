using System;
using ReduxLogin.Models;

namespace ReduxLogin.Data
{
	public class UserRepository : IUserRepository
	{
      private readonly  ApplicationDbContext _db;
		public UserRepository(ApplicationDbContext db)
		{
            _db = db;
		}

        public User Create(User user)
        {
            _db.Users.Add(user);
            user.UserId = _db.SaveChanges();
            return user;
        }

        public User GetById(int id)
        {
            var userFromDb = _db.Users.FirstOrDefault(x => x.UserId == id);


            return userFromDb!;
        }

        public User GetByUserName(string userName)
        {
            var userFromDb = _db.Users.FirstOrDefault(x => x.UserName == userName);


                return userFromDb!;
        }

        public User Update(int id)
        {
            throw new NotImplementedException();
        }
    }
}

