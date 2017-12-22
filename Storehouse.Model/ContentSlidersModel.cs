using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Storehouse.Model
{
    public class ContentSliders
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string TenantId { get; set; }
        public string SlideImage { get; set; }
        public string LeftRight { get; set; }
        public bool IsDeleted { get; set; }
        public int Sequence { get; set; }
        public string Url { get; set; }
    }
}