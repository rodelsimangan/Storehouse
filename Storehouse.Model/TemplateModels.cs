using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Storehouse.Model
{
    public class Templates
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string TenantId { get; set; }
        public string Name { get; set; }
        public string ControllerName { get; set; }
        public string Type { get; set; }
        public bool IncludeInMenu { get; set; }
        public int Sequence { get; set; }
        public string RSSFeedFileName { get; set; }
    }
}