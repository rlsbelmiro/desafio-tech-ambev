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
    public class OrderRepository : IOrderRepository
    {
        private readonly DapperContext _context;

        public OrderRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(Order entity)
        {
            bool result = false;
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO Orders");
            sql.Append("(Amount, CustomerId, CreatedAt, UpdatedAt, CreatedBy, UpdatedBy, Active)");
            sql.Append("VALUES(@Amount, @CustomerId, @CreatedAt, @UpdatedAt, @CreatedBy, @UpdatedBy, @Active);");
            sql.Append("SELECT LAST_INSERT_ID();");

            StringBuilder sql2 = new StringBuilder();
            sql2.Append("INSERT INTO OrderItems");
            sql2.Append("(UnityPrice, Quantity, TotalPrice, OrderId, ProductId, CreatedAt, UpdatedAt, CreatedBy, UpdatedBy)");
            sql2.Append("VALUES(@UnityPrice, @Quantity, @TotalPrice, @OrderId, @ProductId, @CreatedAt, @UpdatedAt, @CreatedBy, @UpdatedBy);");
            sql2.Append("SELECT LAST_INSERT_ID();");

            using (var dapper = _context.CreateConnection())
            {
                dapper.Open();
                using (var transaction = dapper.BeginTransaction())
                {
                    try
                    {
                        var id = await dapper.ExecuteScalarAsync<int>(sql.ToString(), entity);
                        
                        entity.Id = id;
                        result = id > 0;

                        foreach(var item in entity.OrderItems)
                        {
                            item.OrderId = id;
                            item.CreatedAt = DateTime.Now;
                            item.CreatedBy = entity.CreatedBy;
                            var idItem = await dapper.ExecuteScalarAsync<int>(sql2.ToString(), item);
                        }

                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
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
            sql.Append("DELETE FROM Orders WHERE Id = @id");

            using (var dapper = _context.CreateConnection())
            {
                try
                {
                    dapper.Open();
                    using (var transaction = dapper.BeginTransaction())
                    {
                        var order = await dapper.ExecuteAsync(sql.ToString(), new
                        {
                            id
                        });
                        result = order > 0;
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

        public async Task<Order> GetById(int id)
        {
            Order? orders = new Order();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT o.Id, o.Amount, o.CustomerId, i.Id, i.ProductId, i.UnityPrice, i.Quantity, i.TotalPrice, p.Name, c.Name FROM Orders o ");
            sql.Append("INNER JOIN OrderItems i on i.OrderId = o.Id ");
            sql.Append("INNER JOIN Products p on p.Id = i.ProductId ");
            sql.Append("INNER JOIN Customers c on c.Id = o.CustomerId ");
            sql.Append("WHERE o.Id = @id");

            using (var dapper = _context.CreateConnection())
            {
                try
                {
                    dapper.Open();
                    var orderDic = new Dictionary<int, Order>();
                    var result = await dapper.QueryAsync<Order, OrderItem, Product, Customer, Order>(sql.ToString(),
                    map: (o, i, p, c) =>
                    {
                        Order orderEntry;

                        if(!orderDic.TryGetValue(o.Id, out orderEntry))
                        {
                            orderEntry = o;
                            orderEntry.OrderItems = new List<OrderItem>();
                            orderDic.Add(o.Id, o);
                        }

                        i.Product = p;
                        orderEntry.Customer = c;
                        orderEntry.OrderItems.Add(i);
                        return orderEntry;
                    },
                    param: new
                    {
                        id
                    },
                    splitOn: "Id, Name, Name");
                    orders = result != null ? result.FirstOrDefault() : new Order();
                    if (orders == null)
                        orders = new Order();
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
            return orders;
        }

        public async Task<IEnumerable<Order>> List()
        {
            List<Order> orders = new List<Order>();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT o.*, c.Name FROM Orders o ");
            sql.Append(" INNER JOIN Customers c ON o.CustomerId = c.Id");

            using (var dapper = _context.CreateConnection())
            {
                try
                {
                    dapper.Open();
                    var result = await dapper.QueryAsync<Order, Customer, Order>(sql.ToString(),
                    map: (o, c) =>
                    {
                        o.Customer = c;
                        return o;
                    },
                    splitOn: "Name"
                    );
                    orders = result.ToList();
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
            return orders;
        }

        public async Task<bool> Update(Order entity)
        {
            bool result = false;
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE Orders ");
            sql.Append("SET Amount = @Amount, ");
            sql.Append("Email = @Email, ");
            sql.Append("CustomerId = @CurstomerId, ");
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
