using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CityCRUD.Models
{
    public partial class Purchases
    {
        public int Id { get; set; }
        public int IdBook { get; set; }
        public int IdDeliveryCompany { get; set; }
        public DateTime TimeOfPurchase { get; set; }
        public int Amount { get; set; }

        public virtual Books IdBookNavigation { get; set; }
        public virtual DeliveryCompanies IdDeliveryCompanyNavigation { get; set; }
    }
}
