using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacCrawler.DisplayData
{
    public class DisplayKnownManufactures
    {
        public int Id { get; set; }
        public string knownManfacturesText { get; set; }
        public int? ExternalId { get; set; }
        public bool? IsFromExternal { get; set; }
        public string? CreatedDate { get; set; }
    }
}
