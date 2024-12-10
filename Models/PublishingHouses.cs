using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CityCRUD.Models
{
    public partial class PublishingHouses
    {
        public PublishingHouses()
        {
            Books = new HashSet<Books>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int IdCity { get; set; }

        public virtual Cities IdCityNavigation { get; set; }
        public virtual ICollection<Books> Books { get; set; }
    }
}
