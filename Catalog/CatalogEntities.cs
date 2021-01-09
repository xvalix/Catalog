namespace Catalog
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CatalogEntities : DbContext
    {
        public CatalogEntities()
            : base("name=CatalogEntities")
        {
        }

        public virtual DbSet<Materii> Materiis { get; set; }
        public virtual DbSet<Profesori> Profesoris { get; set; }
        public virtual DbSet<Studenti> Studentis { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
