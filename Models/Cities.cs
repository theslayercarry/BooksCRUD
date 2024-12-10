using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CityCRUD.Models
{
    public partial class Cities
    {
        public Cities()
        {
            DeliveryCompanies = new HashSet<DeliveryCompanies>();
            PublishingHouses = new HashSet<PublishingHouses>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<DeliveryCompanies> DeliveryCompanies { get; set; }
        public virtual ICollection<PublishingHouses> PublishingHouses { get; set; }
    }
}
