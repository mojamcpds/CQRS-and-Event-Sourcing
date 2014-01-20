using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using CQRS.Model;

namespace CQRS.Reporting
{
    public class ReportingDbContext:DbContext
    {
        public ReportingDbContext()
            : base(nameOrConnectionString: "DefaultConnection") { }

        public virtual DbSet<Person> Persons { get; set; }
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // NOTE: ToTable used for all entities so that we can prepend 
            // the schema name. This allows all pieces of the application 
            // to be deployed to a single Windows Azure SQL Database, yet avoid 
            // table name collisions, while reducing the deployment costs.

            modelBuilder.Entity<Person>().ToTable("Persons");

        }


    }
}
