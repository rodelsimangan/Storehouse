using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Storehouse.Model;

namespace CMSPortal.Models
{
    public class RobinsonsDBContext : DbContext
    {
        public DbSet<Templates> Templates { get; set; }
        public DbSet<Contents> Contents { get; set; }
        public DbSet<Sliders> Sliders { get; set; }
        public DbSet<HomePageSettings> HomePageSettings { get; set; }
        public DbSet<SocialMediaLinks> SocialMediaLinks { get; set; }
        public DbSet<Lookups> Lookups { get; set; }
        public DbSet<Feedbacks> Feedbacks { get; set; }
        public DbSet<ContentSliders> ContentSliders { get; set; }
        public DbSet<TermsofUse> TermsofUse { get; set; }
    }
}