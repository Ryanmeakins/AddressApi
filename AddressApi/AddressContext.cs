using AddressApi.Schemas;
using Microsoft.EntityFrameworkCore;
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
    }
}
