using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MacCrawler.Models
{
    public partial class KnownManufacturer
    {
        public KnownManufacturer()
        {
            KnownModel = new HashSet<KnownModel>();
            KnownReferenceNumber = new HashSet<KnownReferenceNumber>();
        }

        public int Id { get; set; }
        public string ManufacturerText { get; set; }
        public int? ExternalId { get; set; }
        public bool? IsFromExternal { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual ICollection<KnownModel> KnownModel { get; set; }
        public virtual ICollection<KnownReferenceNumber> KnownReferenceNumber { get; set; }
    }
}
