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
    public class IMKnownManufacures : IKnownManufactures
    {
        private readonly IConfiguration _configuration;
        public IMKnownManufacures(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<List<DisplayKnownManufactures>> display()
        {
            List<DisplayKnownManufactures> displayKnownManufactures = new List<DisplayKnownManufactures>();

            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("display", connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;
                       await using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DisplayKnownManufactures display = new DisplayKnownManufactures();
                                int id = reader.GetInt32(0);
                                string manufacturerText = reader.GetString(1);
                                int externalId = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                                bool isFromExternal = reader.GetBoolean(3);
                                DateTime? createdDate = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4);

                                display.Id = id;
                                display.knownManfacturesText = manufacturerText;
                                display.ExternalId = externalId;
                                display.IsFromExternal = isFromExternal;
                                display.CreatedDate = createdDate?.ToString();
                                displayKnownManufactures.Add(display);
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }

            }
            var count = displayKnownManufactures.Count;
            return displayKnownManufactures;
        }
    }
}
