using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;


namespace DataBase
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        //public ApplicationContext() : base()
        //{

        //    Database.EnsureCreated();
        //}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                    .Build();
                var connectionString = builder.GetConnectionString("DefaultConnection");
                optionsBuilder

                    .UseNpgsql(connectionString);
            }
        }
    }
}
