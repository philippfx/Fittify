using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    //[Route("about")] // explicitly defines controllerName
    //[Route("[controller]")] // pulls ControllerName from className, in this case "About"
    [Route("company/[controller]")] // pulls ControllerName from className, in this case "About" and adds additional route
    public class AboutController
    {
        [Route("")]
        public string Phone()
        {
            return "+1-5555-555-5555";
        }
        [Route("country")]
        public string Country()
        {
            return "USA";
        }

        [Route("[action]")]
        public string Message()
        {
            return "This is Message() from Home Controller";
        }
    }
}
