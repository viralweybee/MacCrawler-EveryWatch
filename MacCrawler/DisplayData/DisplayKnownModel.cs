using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacCrawler.DisplayData
{
    public class DisplayKnownModel
    {
        public int Id { get; set; }
        public string ModelText { get; set; }
        public int? ManufacturerId { get; set; }
        public bool? IsFromExternal { get; set; }
        public int? ExternalId { get; set; }
        public string? CreatedDate { get; set; }
    }
}
