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
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DapperContext _context;

        public CustomerRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(Customer entity)
        {
            bool result = false;
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO Customers");
            sql.Append("(Name, Email, Document, CreatedAt, UpdatedAt, CreatedBy, UpdatedBy, Active)");
            sql.Append("VALUES(@Name, @Email, @Document, @CreatedAt, @UpdatedAt, @CreatedBy, @UpdatedBy, @Active);");
            sql.Append("SELECT LAST_INSERT_ID();");

            using (var dapper = _context.CreateConnection())
            {
                dapper.Open();
                using (var transaction = dapper.BeginTransaction())
                {
                    try
                    {
                        var id = await dapper.ExecuteScalarAsync<int>(sql.ToString(), entity);
                        transaction.Commit();
                        entity.Id = id;
                        result = id > 0;
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
            sql.Append("DELETE FROM Customers WHERE Id = @id");

            using (var dapper = _context.CreateConnection())
            {
                try
                {
                    dapper.Open();
                    using (var transaction = dapper.BeginTransaction())
                    {
                        var costumer = await dapper.ExecuteAsync(sql.ToString(), new
                        {
                            id
                        });
                        result = costumer > 0;
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

        public async Task<Customer> GetByDocument(string document)
        {
            Customer customer = new Customer();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM Customers WHERE Document = @document");

            using (var dapper = _context.CreateConnection())
            {
                try
                {
                    dapper.Open();
                    customer = await dapper.QueryFirstOrDefaultAsync<Customer>(sql.ToString(), new
                    {
                        document
                    });
                    if (customer == null)
                        customer = new Customer();
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
            return customer;
        }

        public async Task<Customer> GetById(int id)
        {
            Customer customer = new Customer();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM Customers WHERE Id = @id");

            using (var dapper = _context.CreateConnection())
            {
                try
                {
                    dapper.Open();
                    customer = await dapper.QueryFirstOrDefaultAsync<Customer>(sql.ToString(), new
                    {
                        id
                    });
                    if (customer == null)
                        customer = new Customer();
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
            return customer;
        }

        public async Task<IEnumerable<Customer>> List()
        {
            List<Customer> customers = new List<Customer>();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM Customers");

            using (var dapper = _context.CreateConnection())
            {
                try
                {
                    dapper.Open();
                    var result = await dapper.QueryAsync<Customer>(sql.ToString());
                    customers = result.ToList();
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
            return customers;
        }

        public async Task<bool> Update(Customer entity)
        {
            bool result = false;
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE Customers ");
            sql.Append("SET Name = @Name, ");
            sql.Append("Email = @Email, ");
            sql.Append("Document = @Document, ");
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
                        var customer = await dapper.ExecuteAsync(sql.ToString(), entity);
                        result = customer > 0;
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
