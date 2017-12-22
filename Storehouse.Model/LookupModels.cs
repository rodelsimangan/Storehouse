using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Storehouse.Model
{
    public class Lookups
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string TenantId { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

    }
}