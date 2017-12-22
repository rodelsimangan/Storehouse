using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Storehouse.Model;

namespace Storehouse.WebAPI.Repositories
{
    public class SocialMediaLinksRepository : ISocialMediaLinksRepository
    {
        readonly IStorehouseDBContext db;

        public SocialMediaLinksRepository(IStorehouseDBContext context)
        {
            db = context;
        }
        public List<SocialMediaLinks> GetSocialMediaLinks(string tenantId)
        {
            try
            {
                var socialmedia = from s in db.SocialMediaLinks
                                  where s.IsDeleted == false
                                  && s.TenantId == tenantId
                                  select s;

                return socialmedia.ToList();
            }
            catch(Exception ex)
            {
                throw ex;
            }
       
        }

        public SocialMediaLinks GetSocialMediaLink(string id)
        {
            try
            {
                var socialmedia = from s in db.SocialMediaLinks
                                   where s.Id == new Guid(id)
                                   select s;
                return socialmedia.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
      
        }
        
        public void UpsertSocialMediaLink(SocialMediaLinks input)
        {
            try
            {
                if (input.Id == null)
                {
                    input.Id = Guid.NewGuid();
                    db.SocialMediaLinks.Add(input);
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