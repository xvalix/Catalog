namespace Catalog
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Materii")]
    public partial class Materii
    {
        [Key]
        [StringLength(50)]
        public string Numematerie { get; set; }

        public int Nrmatricol { get; set; }

        public int idp { get; set; }

        public int Nrcredite { get; set; }

        public virtual Profesori Profesori { get; set; }

        public virtual Studenti Studenti { get; set; }
    }
}
