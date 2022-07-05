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
    public class ProductRepository : IProductRepository
    {
        private readonly DapperContext _context;

        public ProductRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(Product entity)
        {
            bool result = false;
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO Products");
            sql.Append("(Name, Description, Price, Quantity, CreatedAt, UpdatedAt, CreatedBy, UpdatedBy, Active)");
            sql.Append("VALUES(@Name, @Description, @Price, @Quantity, @CreatedAt, @UpdatedAt, @CreatedBy, @UpdatedBy, @Active);");
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
            sql.Append("DELETE FROM Products WHERE Id = @id");

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

        public async Task<Product> GetById(int id)
        {
            Product product = new Product();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM Products WHERE Id = @id");

            using (var dapper = _context.CreateConnection())
            {
                try
                {
                    dapper.Open();
                    product = await dapper.QueryFirstOrDefaultAsync<Product>(sql.ToString(), new
                    {
                        id
                    });
                    if (product == null)
                        product = new Product();
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
            return product;
        }

        public async Task<IEnumerable<Product>> List()
        {
            List<Product> products = new List<Product>();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM Products");

            using (var dapper = _context.CreateConnection())
            {
                try
                {
                    dapper.Open();
                    var result = await dapper.QueryAsync<Product>(sql.ToString());
                    products = result.ToList();
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
            return products;
        }

        public async Task<bool> Update(Product entity)
        {
            bool result = false;
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE Products ");
            sql.Append("SET Name = @Name, ");
            sql.Append("Description = @Description, ");
            sql.Append("Price = @Price, ");
            sql.Append("Quantity = @Quantity, ");
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
