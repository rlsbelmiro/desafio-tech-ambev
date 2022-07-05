using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Domain.Entities
{
    public class User : EntityBase
    {
        public User()
        {
            this.ListOfError = new List<string>();
        }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public override bool Validate()
        {
            if (string.IsNullOrEmpty(this.Email))
                this.ListOfError.Add("Informe o email");
            if (string.IsNullOrEmpty(this.Name))
                this.ListOfError.Add("Informe o nome");
            if (string.IsNullOrEmpty(this.Password))
                this.ListOfError.Add("Informe a senha");

            return this.ListOfError.Count > 0;
        }
    }
}
