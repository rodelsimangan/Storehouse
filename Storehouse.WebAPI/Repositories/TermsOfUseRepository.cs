using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Storehouse.Model;

namespace Storehouse.WebAPI.Repositories
{
    public class TermsOfUseRepository : ITermsOfUseRepository
    {
        readonly IStorehouseDBContext db;
        public TermsOfUseRepository(IStorehouseDBContext context)
        {
            db = context;
        }      

        public TermsofUse GetTermsOfUse(string tenantId)
        {
            try
            {
                var termsofuse = from t in db.TermsofUse
                                  where t.Id != null
                                  select t;
                return termsofuse.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
      
        }

        public void UpsertTermsOfUse(TermsofUse input)
        {
            try
            {
                if (input.Id == null)
                {
                    input.Id = Guid.NewGuid();
                    db.TermsofUse.Add(input);
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