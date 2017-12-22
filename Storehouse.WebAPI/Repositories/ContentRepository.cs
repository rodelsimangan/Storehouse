using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Storehouse.Model;

namespace Storehouse.WebAPI.Repositories
{
    public class ContentRepository : IContentRepository
    {
        readonly IStorehouseDBContext db;
        public ContentRepository(IStorehouseDBContext context)
        {
            db = context;
        }

        public List<Contents> GetContents(string parentId)
        {
            try
            {
                var contents = from c in db.Contents
                               where (string.IsNullOrEmpty(parentId) || c.ParentId == parentId) && c.IsDeleted == false
                               select c;
                return contents.ToList();
            }
            catch(Exception ex)
            {
                throw ex;
            }
       
        }

        public Contents GetContent(string id)
        {
            try
            {
                var content = from c in db.Contents
                               where c.Id == new Guid(id)
                               select c;
                return content.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
      
        }

        public void UpsertContent(Contents input)
        {
            try
            {
                if (input.Id == null)
                {
                    input.Id = Guid.NewGuid();
                    db.Contents.Add(input);
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