using MacCrawler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacCrawler.Interface
{
    public interface ISheet1
    {
        public Task<List<Sheet1>> GetSheetData(int pageNumber,int size);
        public Task<List<Sheet1>> GetSheet();
        public Task<List<Sheet1>> GetSheet(int id);
        public Task PostSheet(Sheet1 sheet1);
    }
}
