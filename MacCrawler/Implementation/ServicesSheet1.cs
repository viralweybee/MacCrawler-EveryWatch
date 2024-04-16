using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MacCrawler.Models;
using MacCrawler.DisplayData;
using MacCrawler.Implementation;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using MacCrawler.Interface;

namespace MacCrawler.Implementation
{
    public class ServicesSheet1 : ISheet1
    {
        private readonly string _connectionString;
        public ServicesSheet1(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<List<Sheet1>> GetSheetData(int pageNumber,int size)
        {
            var data = new List<Sheet1>();
            
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("pagination", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@pageNumber", pageNumber));
                        command.Parameters.Add(new SqlParameter("@size", size));

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {

                                var sheet1 = new Sheet1
                                {
                                    Id = Convert.ToInt32(reader.GetDouble(0)),
                                    Name = reader.GetString(1),
                                    Pid = reader.GetInt32(3),
                                    Count = reader.IsDBNull(2) ? "" : reader.GetString(2)
                                };

                                data.Add(sheet1);
                            } 
                        }
                    }
                }
            
            return data;
        }
        public async Task<List<Sheet1>> GetSheet()
        {
            var sheets = new List<Sheet1>();
            using(var connection =new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "select * from Sheet1$";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {

                            var sheet1 = new Sheet1
                            {
                                Id = Convert.ToInt32(reader.GetDouble(0)),
                                Name = reader.GetString(1),
                                Pid = reader.GetInt32(3),
                                Count = reader.IsDBNull(2) ? "" : reader.GetString(2)
                            };

                            sheets.Add(sheet1);
                        }
                    }
                }
            }
            return sheets;
        }
        public async Task<List<Sheet1>> GetSheet(int id)
        {
            var sheets = new List<Sheet1>();
            using(var connection=new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "select * from Sheet1$ where pid=@id";
               
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@id", id));
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {

                            var sheet1 = new Sheet1
                            {
                                Id = Convert.ToInt32(reader.GetDouble(0)),
                                Name = reader.GetString(1),
                                Pid = reader.GetInt32(3),
                                Count = reader.IsDBNull(2) ? "" : reader.GetString(2)
                            };

                            sheets.Add(sheet1);
                        }
                    }
                }
            }
            return sheets;
        }
        public async Task PostSheet(Sheet1 sheet1)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "INSERT INTO Sheet1$ (id, name, count) VALUES (@id, @name, @count)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@id", sheet1.Id));
                    command.Parameters.Add(new SqlParameter("@name", sheet1.Name));
                    command.Parameters.Add(new SqlParameter("@count", sheet1.Count));             
                    await command.ExecuteNonQueryAsync();
                }
            } 
        }
    }
}
