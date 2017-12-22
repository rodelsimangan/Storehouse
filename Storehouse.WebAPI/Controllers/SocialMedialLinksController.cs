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
    public class SocialMediaLinksController : ApiController
    {
        private IStorehouseDBContext _context;
        private ISocialMediaLinksRepository _repo; //= new SocialMediaLinkRepository();

        public SocialMediaLinksController()
        {
            _context = new StorehouseDBContext();
            _repo = new SocialMediaLinksRepository(_context);
        }

        // GET: api/SocialMediaLinks
        public IEnumerable<SocialMediaLinks> GetSocialMediaLinks(string tenantId)
        {
            return _repo.GetSocialMediaLinks(tenantId);
        }

        // GET: api/SocialMediaLinks/5
        [ResponseType(typeof(SocialMediaLinks))]
        public IHttpActionResult GetSocialMediaLink(string id)
        {
            SocialMediaLinks SocialMediaLink = _repo.GetSocialMediaLink(id);
            if (SocialMediaLink == null)
            {
                return NotFound();
            }

            return Ok(SocialMediaLink);
        }

        // PUT: api/SocialMediaLinks/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSocialMediaLink(string id, SocialMediaLinks SocialMediaLink)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (new Guid(id) != SocialMediaLink.Id)
            {
                return BadRequest();
            }

            _repo.UpsertSocialMediaLink(SocialMediaLink);
            
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/SocialMediaLinks
        [ResponseType(typeof(SocialMediaLinks))]
        public IHttpActionResult PostSocialMediaLink(SocialMediaLinks SocialMediaLink)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repo.UpsertSocialMediaLink(SocialMediaLink);

            return CreatedAtRoute("DefaultApi", new { id = SocialMediaLink.Id }, SocialMediaLink);
        }

    }
}