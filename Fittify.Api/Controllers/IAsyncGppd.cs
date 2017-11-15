using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Controllers
{
    public interface IAsyncGppd
    {
        [HttpGet("id")]
        IActionResult Get();

        [HttpPost]
        IActionResult Post();

        [HttpPut]
        IActionResult Put();

        [HttpDelete]
        IActionResult Delete();
    }
}
