using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Storehouse.Model
{
    public class Sliders
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string TenantId { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string SlideImage { get; set; }
        public string SlideBackground { get; set; }
        public int Sequence { get; set; }
        public bool IsDeleted { get; set; }
        public string Url { get; set; }
    }
}