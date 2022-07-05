using Barber.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace Barber.Data
{
    public class DataBaseContext:DbContext
    {
        public virtual DbSet<Status> status { get; set; }
        public virtual DbSet<User> user { get; set; }
        public virtual DbSet<Category> category { get; set; }
        public virtual DbSet<Orders> orders { get; set; }
        public virtual DbSet<Place> place { get; set; }
        public virtual DbSet<Services> services { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
             .UseSqlServer("data source=localhost; initial catalog=mydb; persist security info=True;user id=root;password=tatia24");
           
        }
    }
}
