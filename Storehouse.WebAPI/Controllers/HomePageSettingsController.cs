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
    public class HomePageSettingsController : ApiController
    {
        private IStorehouseDBContext _context;
        private IHomePageSettingsRepository _repo; //= new HomePageSettingRepository();

        public HomePageSettingsController()
        {
            _context = new StorehouseDBContext();
            _repo = new HomePageSettingsRepository(_context);
        }

             // GET: api/HomePageSettings/5
        [ResponseType(typeof(HomePageSettings))]
        public IHttpActionResult GetHomePageSetting(string tenantId, string id)
        {
            HomePageSettings HomePageSetting = _repo.GetHomepageSettings(tenantId, id);
            if (HomePageSetting == null)
            {
                return NotFound();
            }

            return Ok(HomePageSetting);
        }

        // PUT: api/Contents/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutContent(string id, HomePageSettings HomePageSetting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (new Guid(id) != HomePageSetting.Id)
            {
                return BadRequest();
            }
            
            _repo.UpsertHomePageSettings(HomePageSetting);
            
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}