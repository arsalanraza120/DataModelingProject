using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseContext
{
    public  class DBContext : DbContext
    {

        public DBContext(DbContextOptions options) : base(options)
        {
        
        }
        public DbSet<User> User { get; set; }
        public DbSet<ConfigCredential> ConfigCredentials { get; set; }
    }
}
