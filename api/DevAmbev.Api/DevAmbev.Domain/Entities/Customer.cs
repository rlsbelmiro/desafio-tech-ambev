using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Domain.Entities
{
    public class Customer : EntityBase
    {
        public Customer()
        {
            this.ListOfError = new List<string>();
            this.Orders = new List<Order>();
        }

        public string Name { get; set; }
        public string Document { get; set; }
        public string Email { get; set; }

        public List<Order> Orders { get; set; }

        public override bool Validate()
        {
            if (string.IsNullOrEmpty(this.Email))
                this.ListOfError.Add("Informe o email do cliente");
            if (string.IsNullOrEmpty(this.Document))
                this.ListOfError.Add("Informe o número do documento do cliente");
            if (string.IsNullOrEmpty(this.Name))
                this.ListOfError.Add("Informe o nome do cliente");

            return this.ListOfError.Count == 0;
        }
    }
}
