using MacCrawler.DisplayData;
using MacCrawler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacCrawler.Implementation
{
    public class Fuzzy
    {
        public int CalculateLevenshteinDistance(string str1, string str2)
        {
            int[,] dp = new int[str1.Length + 1, str2.Length + 1];

            for (int i = 0; i <= str1.Length; i++)
            {
                for (int j = 0; j <= str2.Length; j++)
                {
                    if (i == 0)
                    {
                        dp[i, j] = j;
                    }
                    else if (j == 0)
                    {
                        dp[i, j] = i;
                    }
                    else
                    {
                        int cost = (str1[i - 1] == str2[j - 1]) ? 0 : 1;
                        dp[i, j] = Math.Min(
                            Math.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1),
                            dp[i - 1, j - 1] + cost
                        );
                    }
                }
            }

            return dp[str1.Length, str2.Length];
        }

        public bool fuzzySearch(string query, string inputstring, double threshold)
        {
            string normalizedQuery = query.ToLower();
            string inputQuery = inputstring.ToLower();
            double distance = CalculateLevenshteinDistance(normalizedQuery, inputQuery);
            double similarity = 1.0 - (double)(distance / (double)Math.Max(normalizedQuery.Length, inputQuery.Length));
            return similarity >= threshold;
        }
        public List<ManufactureName> Comparedata(List<string> incomingdata, List<Sheet1> sheets)
        {
            int maxId = Convert.ToInt32(sheets.Max(x => x.Id));
            maxId++;
            List<ManufactureName> manufactureNames = new List<ManufactureName>();
            for (int i = 0; i < incomingdata.Count; i++)
            {
                bool flag = false;
                bool alreadyExists = false;
                int tempId = maxId;              
                for (int j = 0; j < sheets.Count; j++)
                {
                    if (sheets[j].Name == incomingdata[i])
                    {
                        alreadyExists = true;
                        break;
                    }
                    else if (fuzzySearch(sheets[j].Name, incomingdata[i], 0.7))
                    {
                        flag = true;
                        tempId = Math.Min(tempId, Convert.ToInt32(sheets[j].Id));
                    }
                }
                if (!alreadyExists)
                {
                    if (!flag)
                    {
                        manufactureNames.Add(new ManufactureName
                        {
                            Id = tempId,
                            Name = incomingdata[i],
                            IsVaraintFound = false,
                            Alreadyexists = false
                        });
                        maxId++;
                    }
                    else
                    {
                        manufactureNames.Add(new ManufactureName
                        {
                            Id = tempId,
                            Name = incomingdata[i],
                            IsVaraintFound = true,
                             Alreadyexists = false
                        });
                    }
                }
                else
                {
                    manufactureNames.Add(new ManufactureName
                    {
                        Id = tempId,
                        Name = incomingdata[i],
                        IsVaraintFound = true,
                        Alreadyexists = true
                    }) ;
                }
            }
            return manufactureNames;
        }
    }
}
