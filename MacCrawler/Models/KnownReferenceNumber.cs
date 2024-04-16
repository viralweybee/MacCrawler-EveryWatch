using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MacCrawler.Models
{
    public partial class KnownReferenceNumber
    {
        public int Id { get; set; }
        public string ReferenceNumberText { get; set; }
        public int? ManufacturerId { get; set; }
        public int? ModelId { get; set; }
        public bool? IsFromExternal { get; set; }
        public int? ExternalId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual KnownManufacturer Manufacturer { get; set; }
        public virtual KnownModel Model { get; set; }
    }
}
