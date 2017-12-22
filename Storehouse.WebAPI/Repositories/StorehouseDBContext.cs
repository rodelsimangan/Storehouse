using Storehouse.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using Storehouse.Model;

namespace Storehouse.WebAPI.Repositories
{
    public class StorehouseDBContext : DbContext, IStorehouseDBContext
    {
        public StorehouseDBContext()
            : base("StorehouseDBContext")
        {
        }

        public StorehouseDBContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public StorehouseDBContext(DbConnection connection)
            : base(connection, true)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Templates> Templates { get; set; }
        public DbSet<Contents> Contents { get; set; }
        public DbSet<TermsofUse> TermsofUse { get; set; }
        public DbSet<ContentSliders> ContentSliders { get; set; }
        public DbSet<Sliders> Sliders { get; set; }
        public DbSet<HomePageSettings> HomePageSettings { get; set; }
        public DbSet<SocialMediaLinks> SocialMediaLinks { get; set; }
        public DbSet<Lookups> Lookups { get; set; }
        public DbSet<Feedbacks> Feedbacks { get; set; }

    }
}