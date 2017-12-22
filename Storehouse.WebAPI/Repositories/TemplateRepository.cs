using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Storehouse.Model;

namespace Storehouse.WebAPI.Repositories
{
    public class TemplateRepository : ITemplateRepository
    {
        readonly IStorehouseDBContext db;//= new Storehouse.WebAPIDbContext();
        public TemplateRepository(IStorehouseDBContext context)
        {
            db = context;
        }
        public List<Templates> GetTemplates(string tenantId)
        {
            try
            {
                var temp = from s in db.Templates
                           where s.TenantId == tenantId
                           select s;

                return temp.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
       
        }

        public Templates GetTemplate(string id)
        {
            try
            {
                var templates = from s in db.Templates
                                where s.Id == new Guid(id)
                                select s;
                return templates.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
      
        }

        public void UpsertTemplate(Templates input)
        {
            try
            {
                if (input.Id == null)
                {
                    input.Id = Guid.NewGuid();
                    db.Templates.Add(input);
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