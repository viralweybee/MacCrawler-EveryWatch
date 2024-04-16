using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacCrawler.Implementation
{
    public class DataTableResponse<T>
    {
        public List<T> Data { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
    }
}
