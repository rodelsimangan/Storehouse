using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Storehouse.Model
{
    public class Contents
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string TenantId { get; set; }
        public string TemplateId { get; set; }
        public string ParentId { get; set; }
        public bool IsParent { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Markup { get; set; }
        public string Icon { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishedDate { get; set; }
        public bool AddSocialLinks { get; set; }
        public bool PushToNewsFeed { get; set; }
        public string MetaKeyword { get; set; }
        public string MetaDescription { get; set; }
        public int Sequence { get; set; }
        public bool IsDeleted { get; set; }
        public bool IncludeInHomePage { get; set; }
        public string CreatedById { get; set; }
        public DateTime? DateCreated { get; set; }
        public string ModifiedById { get; set; }
        public DateTime? DateModified { get; set; }
        public string TempName { get; set; }
    }
}