using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DAL
{
    
    public class CarDBContext : DbContext
    {
        public DbSet<Cars> Cars { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<CarTypes> CarTypes { get; set; }
        public DbSet<Locations> Locations { get; set; }
        //public DbSet<Roles> Roles { get; set; }
        //public DbSet<Genders> Genders { get; set; }

        public CarDBContext():base("name=CarCn")
        {

        }
        
    }

    

    //public class CarDBContextInitializer : DropCreateDatabaseIfModelChanges<CarDBContext>
    //{
    //}
}
