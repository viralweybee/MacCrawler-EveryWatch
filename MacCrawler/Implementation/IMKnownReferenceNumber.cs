using MacCrawler.DisplayData;
using MacCrawler.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MacCrawler.Implementation
{
    public class IMKnownReferenceNumber : IKnownReferenceNumbers
    {
        private readonly IConfiguration _configuration;
        public IMKnownReferenceNumber(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<List<DisplayKnownReferenceNumber>> display()
        {
            List<DisplayKnownReferenceNumber> displayKnownReferenceNumbers = new List<DisplayKnownReferenceNumber>();
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("displayKnownReference", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        await using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DisplayKnownReferenceNumber displayKnownReference = new DisplayKnownReferenceNumber();
                                displayKnownReference.Id = reader.GetInt32(0);
                                displayKnownReference.ReferenceNumberText = reader.GetString(1);
                                displayKnownReference.ManufactureId = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2);
                                displayKnownReference.ModelId = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3);
                                displayKnownReference.IsFromExternal = reader.IsDBNull(4) ? (bool?)null : reader.GetBoolean(4);
                                displayKnownReference.ExternalId = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5);
                                DateTime? date = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6);
                                displayKnownReference.CreatedDate = date.ToString();
                                displayKnownReferenceNumbers.Add(displayKnownReference);
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return displayKnownReferenceNumbers;
        }
    }
}
