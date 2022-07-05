using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Domain.Entities
{
    public class Product : EntityBase
    {
        public Product()
        {
            this.ListOfError = new List<string>();
            this.Orders = new List<OrderItem>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set;}
        public string Photo { get; set; }
        public int Quantity { get; set; }

        public List<OrderItem> Orders { get; set; }

        public override bool Validate()
        {
            if (string.IsNullOrEmpty(this.Name))
                this.ListOfError.Add("Informe o nome do produto");
            if (string.IsNullOrEmpty(this.Description))
                this.ListOfError.Add("Informe a descrição do produto");
            if (this.Price <= 0)
                this.ListOfError.Add("Informe o preço do produto");

            return this.ListOfError.Count == 0;
        }


    }
}
