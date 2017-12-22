using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Storehouse.Model;
using Storehouse.WebAPI.Repositories;

namespace Storehouse.WebAPI.Controllers
{
    public class SlidersController : ApiController
    {
        private IStorehouseDBContext _context;
        private ISlidersRepository _repo; //= new SliderRepository();

        public SlidersController()
        {
            _context = new StorehouseDBContext();
            _repo = new SlidersRepository(_context);
        }

        // GET: api/Sliders
        public IEnumerable<Sliders> GetSliders(string tenantId)
        {
            return _repo.GetSliders(tenantId);
        }

        // GET: api/Sliders/5
        [ResponseType(typeof(Sliders))]
        public IHttpActionResult GetSlider(string id)
        {
            Sliders Slider = _repo.GetSlider(id);
            if (Slider == null)
            {
                return NotFound();
            }

            return Ok(Slider);
        }

        // PUT: api/Sliders/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSlider(string id, Sliders Slider)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (new Guid(id) != Slider.Id)
            {
                return BadRequest();
            }

            _repo.UpsertSlider(Slider);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Sliders
        [ResponseType(typeof(Sliders))]
        public IHttpActionResult PostSlider(Sliders Slider)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repo.UpsertSlider(Slider);

            return CreatedAtRoute("DefaultApi", new { id = Slider.Id }, Slider);
        }

    }
}