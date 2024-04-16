using MacCrawler.DisplayData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacCrawler.Interface
{
    public interface IKnownModel
    {
        public Task<List<DisplayKnownModel>> displays();
    }
}
