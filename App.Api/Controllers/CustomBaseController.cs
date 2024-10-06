using App.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {

        //her seferinde yazacağım kodu base hale getirdim.

        [NonAction] //end point olmasını istemiyorum.
        public IActionResult CustomActionResult<T>(ServiceResult<T> serviceResultesult)
        {
            if (serviceResultesult.Status == HttpStatusCode.NoContent)
                return NoContent();


            if(serviceResultesult.Status == HttpStatusCode.Created)
            {
                return Created(serviceResultesult.UrlAsCreated ,serviceResultesult);
            }   


            return new ObjectResult(serviceResultesult)
            {
                StatusCode = serviceResultesult.Status.GetHashCode()
            };  

        }


        [NonAction] //end point olmasını istemiyorum.
        public IActionResult CustomActionResult(ServiceResult result)
        {
            if (result.Status == HttpStatusCode.NoContent)
            {
                return new ObjectResult(null)
                {
                    StatusCode = result.Status.GetHashCode()
                };
            }

            return new ObjectResult(result)
            {
                StatusCode = result.Status.GetHashCode()
            };

        }

    }

}
