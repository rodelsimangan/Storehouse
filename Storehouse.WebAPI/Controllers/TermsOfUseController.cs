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
    public class TermsOfUsesController : ApiController
    {
        private IStorehouseDBContext _context;
        private ITermsOfUseRepository _repo; //= new TermsOfUseRepository();

        public TermsOfUsesController()
        {
            _context = new StorehouseDBContext();
            _repo = new TermsOfUseRepository(_context);
        }


        // GET: api/TermsOfUses/5
        [ResponseType(typeof(TermsofUse))]
        public IHttpActionResult GetTermsOfUse(string id)
        {
            TermsofUse TermsOfUse = _repo.GetTermsOfUse(id);
            if (TermsOfUse == null)
            {
                return NotFound();
            }

            return Ok(TermsOfUse);
        }

        // PUT: api/TermsOfUses/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTermsOfUse(string id, TermsofUse TermsOfUse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (new Guid(id) != TermsOfUse.Id)
            {
                return BadRequest();
            }

            _repo.UpsertTermsOfUse(TermsOfUse);
            
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/TermsOfUses
        [ResponseType(typeof(TermsofUse))]
        public IHttpActionResult PostTermsOfUse(TermsofUse TermsOfUse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repo.UpsertTermsOfUse(TermsOfUse);

            return CreatedAtRoute("DefaultApi", new { id = TermsOfUse.Id }, TermsOfUse);
        }

    }
}