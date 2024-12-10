using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CityCRUD.Models
{
    public partial class DeliveryCompanies
    {
        public DeliveryCompanies()
        {
            Purchases = new HashSet<Purchases>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string NameOfResponsiblePerson { get; set; }
        public string Address { get; set; }
        public int IdCity { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Inn { get; set; }

        public virtual Cities IdCityNavigation { get; set; }
        public virtual ICollection<Purchases> Purchases { get; set; }
    }
}
