using Dapper;
using DevAmbev.Domain.Entities;
using DevAmbev.Infra.Data;
using DevAmbev.Infra.Repositories.Contracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Infra.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;

        public UserRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(User entity)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO DevAmbev.Users");
            sql.Append("(Name, Email, Password, CreatedAt, UpdatedAt, CreatedBy, UpdatedBy, Active)");
            sql.Append("VALUES(@Name, @Email, @Password, @CreatedAt, @UpdatedAt, @CreatedBy, @UpdatedBy, @Active);");

            using (var dapper = _context.CreateConnection())
            {
                var user = await dapper.ExecuteAsync(sql.ToString(), entity);
                return user > 0;
            }
        }

        public async Task<bool> Delete(int id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("DELETE FROM Users WHERE Id = @id");

            using (var dapper = _context.CreateConnection())
            {
                var user = await dapper.ExecuteAsync(sql.ToString(), new
                {
                    id
                });
                return user > 0;
            }
        }

        public async Task<User> GetById(int id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM Users WHERE Id = @id");

            using (var dapper = _context.CreateConnection())
            {
                var user = await dapper.QueryFirstAsync<User>(sql.ToString(), new {
                    id
                });
                return user;
            }
        }

        public async Task<IEnumerable<User>> List()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM Users");

            using(var dapper = _context.CreateConnection())
            {
                var users = await dapper.QueryAsync<User>(sql.ToString());
                return users.ToList();
            }
        }

        public async Task<User> Login(string email, string password)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM Users WHERE Email = @email AND Password = @password");

            using (var dapper = _context.CreateConnection())
            {
                var user = await dapper.QueryFirstAsync<User>(sql.ToString(), new
                {
                    email,
                    password
                });
                return user;
            }
        }

        public async Task<bool> Update(User entity)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE DevAmbev.Users ");
            sql.Append("SET Name = @Name, ");
            sql.Append("Email = @Email, ");
            sql.Append("Password = @Password, ");
            sql.Append("CreatedAt = @CreatedAt, ");
            sql.Append("UpdatedAt = @UpdatedAt, ");
            sql.Append("CreatedBy = @CreatedBy, ");
            sql.Append("UpdatedBy = @UpdatedBy, ");
            sql.Append("Active = @Active ");
            sql.Append("WHERE Id = @Id; ");


            using (var dapper = _context.CreateConnection())
            {
                var user = await dapper.ExecuteAsync(sql.ToString(), entity);
                return user > 0;
            }
        }
    }
}
