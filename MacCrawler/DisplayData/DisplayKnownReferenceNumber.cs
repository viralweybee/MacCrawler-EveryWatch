using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacCrawler.DisplayData
{
    public class DisplayKnownReferenceNumber
    {
        public int Id { get; set; }
        public string ReferenceNumberText { get; set; }
        public int? ManufactureId { get; set; }
        public int? ModelId { get; set; }
        public bool? IsFromExternal { get; set; }
        public int? ExternalId { get; set; }
        public string? CreatedDate { get; set; }
    }
}
