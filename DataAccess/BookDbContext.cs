using BookStore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.DataAccess
{
    
        public class PartDbContext : DbContext
        {
            public PartDbContext(DbContextOptions<PartDbContext> options) : base(options)
            {

            }


            public DbSet<Users> Users { get; set; }
            public DbSet<Books> Books { get; set; }
            public DbSet<Orders> Orders { get; set; }
        }
    
}
