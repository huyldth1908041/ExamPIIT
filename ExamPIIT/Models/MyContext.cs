using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ExamPIIT.Models
{
    public class MyContext : DbContext 
    {
        public MyContext() : base("myContext")
        {

        }
        public DbSet<User> Users { get; set; }
    }
}