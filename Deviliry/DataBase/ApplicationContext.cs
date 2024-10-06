using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataBase
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Попытка получить строку подключения из переменной окружения
                var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

                if (string.IsNullOrEmpty(connectionString))
                {
                    // Если переменная окружения не задана, используем appsettings.json
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();
                    connectionString = builder.GetConnectionString("DefaultConnection");
                }

                // Настройка DbContext с использованием строки подключения
                optionsBuilder.UseNpgsql(connectionString);
            }
        }
    }
}

