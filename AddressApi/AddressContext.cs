using AddressApi.Schemas;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressApi
{
    public class AddressContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
        public AddressContext(DbContextOptions<AddressContext> options) : base(options) { }

        public DbSet<Address> addresses { get; set; }

        public string ConnectionString { get; set; }

        public AddressContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }


        public List<Address> GetAllAdresses()
        {
            List<Address> list = new List<Address>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from country_data", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Address()
                        {
                            Country = reader["COUNTRY"].ToString(),
                            Street = reader["STREET"].ToString(),
                            Unit = reader["UNIT"].ToString(),
                            City = reader["CITY"].ToString(),
                            District = reader["DISTRICT"].ToString(),
                            Region = reader["REGION"].ToString(),
                            PostalCode = reader["POSTCODE"].ToString()
                        }); ;
                    }
                }
            }
            return list;
        }
    }
}
