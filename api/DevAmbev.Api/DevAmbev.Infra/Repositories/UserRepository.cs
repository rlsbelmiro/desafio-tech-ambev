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
            bool result = false;
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO DevAmbev.Users");
            sql.Append("(Name, Email, Password, CreatedAt, UpdatedAt, CreatedBy, UpdatedBy, Active)");
            sql.Append("VALUES(@Name, @Email, @Password, @CreatedAt, @UpdatedAt, @CreatedBy, @UpdatedBy, @Active);");
            sql.Append("SELECT LAST_INSERT_ID();");

            using (var dapper = _context.CreateConnection())
            {
                dapper.Open();
                using (var transaction = dapper.BeginTransaction())
                {
                    try
                    {
                        var user = await dapper.ExecuteScalarAsync<int>(sql.ToString(), entity);
                        transaction.Commit();
                        entity.Id = user;
                        result = user > 0;
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    finally
                    {
                        dapper.Close();
                    }

                }
            }
            return result;
        }

        public async Task<bool> Delete(int id)
        {
            bool result = false;
            StringBuilder sql = new StringBuilder();
            sql.Append("DELETE FROM Users WHERE Id = @id");

            using (var dapper = _context.CreateConnection())
            {
                try
                {
                    dapper.Open();
                    using (var transaction = dapper.BeginTransaction())
                    {
                        var user = await dapper.ExecuteAsync(sql.ToString(), new
                        {
                            id
                        });
                        result = user > 0;
                        transaction.Commit();
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    dapper.Close();
                }
            }

            return result;
        }

        public async Task<User> GetById(int id)
        {
            User user = new User();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM Users WHERE Id = @id");

            using (var dapper = _context.CreateConnection())
            {
                try
                {
                    dapper.Open();
                    user = await dapper.QueryFirstOrDefaultAsync<User>(sql.ToString(), new
                    {
                        id
                    });
                    if (user == null)
                        user = new User();
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    dapper.Close();
                }
            }
            return user;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            User user = new User();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM Users u WHERE u.Email= @email");

            using (var dapper = _context.CreateConnection())
            {
                try
                {
                    dapper.Open();
                    user = await dapper.QueryFirstOrDefaultAsync<User>(sql.ToString(), new
                    {
                        email
                    });
                    if (user == null)
                        user = new User();
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    dapper.Close();
                }
            }
            return user;
        }

        public async Task<IEnumerable<User>> List()
        {
            List<User> users = new List<User>();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM Users");

            using (var dapper = _context.CreateConnection())
            {
                try
                {
                    dapper.Open();
                    var result = await dapper.QueryAsync<User>(sql.ToString());
                    users = result.ToList();
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    dapper.Close();
                }

                
            }
            return users;
        }

        public async Task<User> Login(string email, string password)
        {
            try
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
            catch (Exception ex)
            {
                return new User();
            }
        }

        public async Task<bool> Update(User entity)
        {
            bool result = false;
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
                try
                {
                    dapper.Open();
                    using (var transaction = dapper.BeginTransaction())
                    {
                        var user = await dapper.ExecuteAsync(sql.ToString(), entity);
                        result = user > 0;
                        transaction.Commit();
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    dapper.Close();
                }
            }
            return result;
        }
    }
}
