using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Storehouse.Model;

namespace Storehouse.WebAPI.Repositories
{
    public class HomePageSettingsRepository : IHomePageSettingsRepository
    {
        readonly IStorehouseDBContext db;// = new Storehouse.WebAPIDbContext();
        public HomePageSettingsRepository(IStorehouseDBContext context)
        {
            db = context;
        }
       
        public HomePageSettings GetHomepageSettings(string tenantId, string id)
        {
            try
            {
                var homepagesettings = from h in db.HomePageSettings
                                       where (string.IsNullOrEmpty(tenantId) || h.TenantId == tenantId)
                                       || (string.IsNullOrEmpty(id) || h.Id == new Guid(id))
                                        select h;
                return homepagesettings.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }           
        }

        public void UpsertHomePageSettings(HomePageSettings input)
        {
            try
            {
                if (input.Id == null)
                {
                    input.Id = Guid.NewGuid();
                    db.HomePageSettings.Add(input);
                    db.SaveChanges();
                }
                else
                {
                    db.Entry(input).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}