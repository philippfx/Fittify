using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quantus.IDP.Helpers.ObjectResults;
using Quantus.IDP.Services;

namespace Quantus.IDP.Controllers
{
    [Route("resetquantusidpdatabase")]
    public class ResetDatabaseApiController : Controller
    {
        private IDbResetter _dbResetter;

        public ResetDatabaseApiController(IDbResetter dbResetter)
        {
            _dbResetter = dbResetter;
        }

        [HttpGet]
        public IActionResult ResetDatabaseForm()
        {
            return new ContentResult()
            {
                ContentType = "text/html",
                Content =
                    @"
					    <!DOCTYPE html>
                        <html>
                            <body>
                                <form action=""resetquantusidpdatabase"" method=""post"">
                                      <input type=""submit"" value=""Reset!"">
                                 </form>
                            </body>
                        </html>
                    "
            };
        }

        [HttpPost]
        public async Task<IActionResult> ResetDatabase()
        {
            if (_dbResetter != null)
            {
                var isDbResetted = await _dbResetter.ResetDb();

                if (isDbResetted)
                {
                    return Ok("OK database resetted!");
                }

                ModelState.AddModelError("unknownError", "Due to an unknown error, the database has not been resetted.");

                return new InternalServerErrorObjectResult(ModelState);
            }

            ModelState.AddModelError("environment", "You are in 'Production' environment. Database cannot be resetted here.");

            return new UnprocessableEntityObjectResult(ModelState);
        }
    }
}
