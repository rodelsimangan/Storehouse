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
    public class ContentsController : ApiController
    {
        private IStorehouseDBContext _context;
        private IContentRepository _repo;

        public ContentsController()
        {
            _context = new StorehouseDBContext();
            _repo = new ContentRepository(_context);
        }

        // GET: api/Contents
        public IEnumerable<Contents> GetContents(string parentId)
        {
            return _repo.GetContents(parentId);
        }

        // GET: api/Contents/5
        [ResponseType(typeof(Contents))]
        public IHttpActionResult GetContent(string id)
        {
            Contents Content = _repo.GetContent(id);
            if (Content == null)
            {
                return NotFound();
            }

            return Ok(Content);
        }

        // PUT: api/Contents/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutContent(string id, Contents Content)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (new Guid(id) != Content.Id)
            {
                return BadRequest();
            }


            _repo.UpsertContent(Content);


            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Contents
        [ResponseType(typeof(Contents))]
        public IHttpActionResult PostContent(Contents Content)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repo.UpsertContent(Content);

            return CreatedAtRoute("DefaultApi", new { id = Content.Id }, Content);
        }

    }
}