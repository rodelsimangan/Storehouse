using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Storehouse.Model
{
    public class Feedbacks
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string TenantId { get; set; }
        public string MemberId { get; set; }
        public string From { get; set; }
        public string Name { get; set; }
        public string ContactNumber { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsMember { get; set; }
        public bool IsRead { get; set; }
    }
}