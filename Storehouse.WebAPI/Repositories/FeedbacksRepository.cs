using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Storehouse.Model;

namespace Storehouse.WebAPI.Repositories
{
    public class FeedbackRepository : IFeedbacksRepository
    {
        readonly IStorehouseDBContext db ;

        public FeedbackRepository(IStorehouseDBContext context)
        {
            db = context;
        }

        public Feedbacks GetFeedback(string id)
        {
            try
            {
                var feedback = from s in db.Feedbacks
                               where s.Id == new Guid(id)
                               select s;

                return feedback.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<Feedbacks> GetFeedbacks(string tenantId, bool isMember)
        {
            try
            {
                var feedbacks = from s in db.Feedbacks
                               where s.IsMember == isMember
                               && s.TenantId == tenantId
                               orderby s.From
                               select s;
                
                return feedbacks.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

      
        public void UpsertFeedback(Feedbacks input)
        {
            try
            {
                if (input.Id == null)
                {
                    input.Id = Guid.NewGuid();
                    db.Feedbacks.Add(input);
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