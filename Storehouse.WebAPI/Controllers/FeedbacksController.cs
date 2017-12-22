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
    public class FeedbacksController : ApiController
    {
        private IStorehouseDBContext _context;
        private IFeedbacksRepository _repo;// = new FeedbackRepository();

        public FeedbacksController()
        {
            _context = new StorehouseDBContext();
            _repo = new FeedbackRepository(_context);
        }

        // GET: api/Feedbacks
        public IEnumerable<Feedbacks> GetFeedbacks(string tenantId, bool isMember)
        {
            return _repo.GetFeedbacks(tenantId, isMember);
        }

        // GET: api/Feedbacks/5
        [ResponseType(typeof(Feedbacks))]
        public IHttpActionResult GetFeedback(string id)
        {
            Feedbacks Feedback = _repo.GetFeedback(id);
            if (Feedback == null)
            {
                return NotFound();
            }

            return Ok(Feedback);
        }

        // PUT: api/Feedbacks/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFeedback(string id, Feedbacks Feedback)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (new Guid(id) != Feedback.Id)
            {
                return BadRequest();
            }


            _repo.UpsertFeedback(Feedback);


            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Feedbacks
        [ResponseType(typeof(Feedbacks))]
        public IHttpActionResult PostFeedback(Feedbacks Feedback)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repo.UpsertFeedback(Feedback);

            return CreatedAtRoute("DefaultApi", new { id = Feedback.Id }, Feedback);
        }

    }
}