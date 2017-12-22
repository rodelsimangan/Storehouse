using Storehouse.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Storehouse.Model;

namespace Storehouse.WebAPI.Repositories
{
    public interface IStorehouseDBContext //: IDisposable
    {
        // DbSet<TEntity> Set<TEntity>() where TEntity : class;
        // DbSet Set(Type entityType);
        DbSet<Templates> Templates { get; set; }
        DbSet<Contents> Contents { get; set; }
        DbSet<TermsofUse> TermsofUse { get; set; }
        DbSet<ContentSliders> ContentSliders { get; set; }
        DbSet<Sliders> Sliders { get; set; }
        DbSet<HomePageSettings> HomePageSettings { get; set; }
        DbSet<SocialMediaLinks> SocialMediaLinks { get; set; }
        DbSet<Lookups> Lookups { get; set; }
        DbSet<Feedbacks> Feedbacks { get; set; }

        int SaveChanges();
        IEnumerable<DbEntityValidationResult> GetValidationErrors();
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        DbEntityEntry Entry(object entity);
        Database Database { get; }

    }
}
