using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.DataModelRepositories.Repository.Sport;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Fittify.Api.Controllers.Sport
{
    [Route("api")]
    public class CategoryController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            var repo = new CategoryRepository();
            var result = repo.GetAll().ToList();
            //return new string[] { "value1: 1", "value2" };
            return new JsonResult(result);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
