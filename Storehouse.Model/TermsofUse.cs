using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Storehouse.Model
{
    public class TermsofUse
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public Guid Id { get; set; }
        public string TenantId { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Markup { get; set; }  
    }
}
