using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Storehouse.Model;

namespace Storehouse.WebAPI.Repositories
{
    public class SlidersRepository : ISlidersRepository
    {
        readonly IStorehouseDBContext db;//= new Storehouse.WebAPIDbContext();

        public SlidersRepository(IStorehouseDBContext context)
        {
            db = context;
        }

        public List<Sliders> GetSliders(string tenantId)
        {
            try
            {
                var sliders = from s in db.Sliders
                              where s.IsDeleted == false
                              && s.TenantId == tenantId
                              select s;
                return sliders.ToList();
            }
            catch(Exception ex)
            {
                throw ex;
            }
       
        }

        public Sliders GetSlider(string id)
        {
            try
            {
                var slider = from s in db.Sliders
                              where s.Id == new Guid(id)
                              select s;
                return slider.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
      
        }

        public void UpsertSlider(Sliders input)
        {
            try
            {
                if (input.Id == null)
                {
                    input.Id = Guid.NewGuid();
                    db.Sliders.Add(input);
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