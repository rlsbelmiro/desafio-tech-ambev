using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Domain.Entities
{
    public class Order : EntityBase
    {
        public Order()
        {
            this.ListOfError = new List<string>();
            this.Customer = new Customer();
            this.OrderItems = new List<OrderItem>();

        }

        public decimal Amount { get; set; }
        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        public List<OrderItem> OrderItems { get; set; }

        public override bool Validate()
        {
            if (this.Amount <= 0)
                this.ListOfError.Add("Informe o valor do pedido");
            if (this.CustomerId <= 0)
                this.ListOfError.Add("Informe o cliente do pedido");
            if (this.OrderItems == null || this.OrderItems.Count == 0)
                this.ListOfError.Add("Informe os itens do pedido");

            return this.ListOfError.Count == 0;
        }
    }
}
