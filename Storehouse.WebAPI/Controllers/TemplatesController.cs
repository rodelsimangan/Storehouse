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
    public class TemplatesController : ApiController
    {
        private IStorehouseDBContext _context;
        private ITemplateRepository _repo; //= new TemplateRepository();

        public TemplatesController()
        {
            _context = new StorehouseDBContext();
            _repo = new TemplateRepository(_context);
        }

        // GET: api/Templates
        public IEnumerable<Templates> GetTemplates(string tenantId)
        {
            return _repo.GetTemplates(tenantId);
        }

        // GET: api/Templates/5
        [ResponseType(typeof(Templates))]
        public IHttpActionResult GetTemplate(string id)
        {
            Templates Template = _repo.GetTemplate(id);
            if (Template == null)
            {
                return NotFound();
            }

            return Ok(Template);
        }

        // PUT: api/Templates/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTemplate(string id, Templates Template)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (new Guid(id) != Template.Id)
            {
                return BadRequest();
            }
            
            _repo.UpsertTemplate(Template);
            
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Templates
        [ResponseType(typeof(Templates))]
        public IHttpActionResult PostTemplate(Templates Template)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repo.UpsertTemplate(Template);

            return CreatedAtRoute("DefaultApi", new { id = Template.Id }, Template);
        }

    }
}