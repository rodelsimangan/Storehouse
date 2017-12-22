using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Storehouse.Model;

namespace Storehouse.WebAPI.Repositories
{
    public class ContentSlidersRepository : IContentSlidersRepository
    {
        readonly IStorehouseDBContext db;

        public ContentSlidersRepository(IStorehouseDBContext context)
        {
            db = context;
        }

        public List<ContentSliders> GetContentSliders(string tenantId)
        {
            try
            {
                var contentsliders = from s in db.ContentSliders
                                     where s.IsDeleted == false
                                     && s.TenantId == tenantId
                                     orderby s.Sequence
                                     select s;
                return contentsliders.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ContentSliders GetContentSlider(string id)
        {
            try
            {
                var Id = Guid.Parse(id);
                var contentsliders = from s in db.ContentSliders
                                     where s.IsDeleted == false
                                     && s.Id == Id
                                     select s;
                return contentsliders.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpsertContentSlider(ContentSliders input)
        {
            try
            {
                if (input.Id == null)
                {
                    int seqCtr = 0;
                    var contentslidersList = (from s in db.ContentSliders
                                              where s.IsDeleted == false
                                              select s.Sequence).ToList();
                    if (contentslidersList.Count > 0)
                        seqCtr = contentslidersList.Max();

                    input.Id = Guid.NewGuid();
                    input.Sequence = seqCtr + 1;

                    db.ContentSliders.Add(input);
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

        public void MoveUpContentSlider(string id)
        {
            try
            {
                var contentsliders = (from c in db.ContentSliders
                                      where c.Id == new Guid(id)
                                      select c).First();
                if (contentsliders.Sequence > 1)
                {

                    var higherSlider = (from h in db.ContentSliders
                                        where h.Sequence < contentsliders.Sequence && h.IsDeleted == false
                                        orderby h.Sequence descending
                                        select h).First();

                    int higherSeq = higherSlider.Sequence;
                    higherSlider.Sequence = contentsliders.Sequence;
                    db.Entry(higherSlider).State = EntityState.Modified;
                    db.SaveChanges();

                    contentsliders.Sequence = higherSeq;
                    db.Entry(contentsliders).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void MoveDownContentSlider(string id)
        {
           try
            {
                var slidcontentsliderser = (from c in db.ContentSliders
                                            where c.Id == new Guid(id)
                                            select c).First();
                if (slidcontentsliderser.Sequence < db.ContentSliders.Count())
                {
                    var lowerSlider = (from l in db.Sliders
                                       where l.Sequence > slidcontentsliderser.Sequence && l.IsDeleted == false
                                       orderby l.Sequence
                                       select l).First();

                    int lowerSeq = lowerSlider.Sequence;
                    lowerSlider.Sequence = slidcontentsliderser.Sequence;
                    db.Entry(lowerSlider).State = EntityState.Modified;
                    db.SaveChanges();

                    slidcontentsliderser.Sequence = lowerSeq;
                    db.Entry(slidcontentsliderser).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

    }
}