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

namespace ContentSliderManagement.Controllers
{
    public class ContentSlidersController : ApiController
    {
        private IStorehouseDBContext _context;
        private IContentSlidersRepository _repo; //= new ContentSliderRepository();

        public ContentSlidersController()
        {
            _context = new StorehouseDBContext();
            _repo = new ContentSlidersRepository(_context);
        }

        // GET: api/ContentSliders
        public IEnumerable<ContentSliders> GetContentSliders(string tenantId)
        {
            return _repo.GetContentSliders(tenantId);
        }

        // GET: api/ContentSliders/5
        [ResponseType(typeof(ContentSliders))]
        public IHttpActionResult GetContentSlider(string id)
        {
            ContentSliders ContentSlider = _repo.GetContentSlider(id);
            if (ContentSlider == null)
            {
                return NotFound();
            }

            return Ok(ContentSlider);
        }

        // PUT: api/ContentSliders/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutContentSlider(string id, ContentSliders ContentSlider, string MoveUpId, string MoveDownId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (new Guid(id) != ContentSlider.Id)
            {
                return BadRequest();
            }

            if (!string.IsNullOrEmpty(MoveUpId))
                _repo.MoveUpContentSlider(MoveUpId);
            else if (!string.IsNullOrEmpty(MoveDownId))
                _repo.MoveDownContentSlider(MoveDownId);
            else
                _repo.UpsertContentSlider(ContentSlider);


            return StatusCode(HttpStatusCode.NoContent);
        }


        // POST: api/ContentSliders
        [ResponseType(typeof(ContentSliders))]
        public IHttpActionResult PostContentSlider(ContentSliders ContentSlider)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repo.UpsertContentSlider(ContentSlider);

            return CreatedAtRoute("DefaultApi", new { id = ContentSlider.Id }, ContentSlider);
        }

    }
}