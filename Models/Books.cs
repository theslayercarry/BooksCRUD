using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CityCRUD.Models
{
    public partial class Books
    {
        public Books()
        {
            Purchases = new HashSet<Purchases>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int Pages { get; set; }
        public int IdAuthor { get; set; }
        public int IdPublishingHouse { get; set; }
        public int Cost { get; set; }

        public virtual Authors IdAuthorNavigation { get; set; }
        public virtual PublishingHouses IdPublishingHouseNavigation { get; set; }
        public virtual ICollection<Purchases> Purchases { get; set; }
    }
}
