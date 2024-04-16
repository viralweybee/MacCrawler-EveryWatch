using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacCrawler.DisplayData
{
    public class ManufactureName
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsVaraintFound { get; set; }
        public bool Alreadyexists { get; set; }
    }
}
