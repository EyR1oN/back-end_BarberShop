using Barber.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Logging;

namespace Barber.Data
{


    public class DataBaseContext:DbContext
    {

        public virtual DbSet<Status> status { get; set; }
        public virtual DbSet<User> user { get; set; }
        public virtual DbSet<Place> place { get; set; }
        public virtual DbSet<Category> category { get; set; }
        public virtual DbSet<Services> services { get; set; }
        public virtual DbSet<Orders> orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;user id=root;password=password;database=barberShop")
            .UseLoggerFactory(LoggerFactory.Create(b => b
                .AddConsole()
                .AddFilter(level => level >= LogLevel.Information)))
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();

        }
    }
}
