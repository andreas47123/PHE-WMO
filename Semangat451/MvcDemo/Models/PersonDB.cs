using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MvcDemo.Models
{
    public class PersonDB
    {
        public int ID { get; set; }
        public string Nama { get; set; }
        public string Email { get; set; }
        public string Nope { get; set; }

    }
    public class PersonDBContext : DbContext
    {
        public DbSet<PersonDB> Persons { get; set; }
    }
}