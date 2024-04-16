using MacCrawler.DisplayData;
using MacCrawler.Interface;
using MacCrawler.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MacCrawler.Implementation
{
    public class IMKnownModel : IKnownModel
    {
        private readonly IConfiguration _configuration;
        public IMKnownModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<List<DisplayKnownModel>> displays()
        {
            List<DisplayKnownModel> knownModels = new List<DisplayKnownModel>();
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("displayKnownModel", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        await using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DisplayKnownModel knownModel = new DisplayKnownModel();
                                knownModel.Id = reader.GetInt32(0);
                                knownModel.ModelText = reader.GetString(1);
                                knownModel.ManufacturerId = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                                knownModel.IsFromExternal = reader.IsDBNull(3) ? (bool?)null : reader.GetBoolean(3);
                                knownModel.ExternalId = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                                DateTime? date = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5);
                                knownModel.CreatedDate = date.ToString();
                                knownModels.Add(knownModel);
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return knownModels;
        }
    }
}
