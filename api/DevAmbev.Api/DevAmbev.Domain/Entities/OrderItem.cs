using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Domain.Entities
{
    public class OrderItem : EntityBase
    {
        public OrderItem()
        {
            this.ListOfError = new List<string>();
            this.Order = new Order();
            this.Product = new Product();
        }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnityPrice { get; set; }
        public decimal TotalPrice { 
            get
            {
                return this.UnityPrice * this.Quantity;
            }
        }

        public Order Order { get; set; }
        public Product Product { get; set; }

        public override bool Validate()
        {
            if (this.OrderId <= 0)
                this.ListOfError.Add("Informe o id do pedido");
            if (this.ProductId <= 0)
                this.ListOfError.Add("Informe o id do produto");
            if (this.Quantity <= 0)
                this.ListOfError.Add("Informe a quantidade comprada");
            if (this.UnityPrice <= 0)
                this.ListOfError.Add("Informe o preço unitário do produto");

            return this.ListOfError.Count == 0;

        }
    }
}
