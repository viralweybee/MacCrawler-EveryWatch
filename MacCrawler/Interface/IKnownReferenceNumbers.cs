using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MacCrawler.DisplayData;
namespace MacCrawler.Interface
{
    public interface IKnownReferenceNumbers
    {
        public Task<List<DisplayKnownReferenceNumber>> display();
    }
}
