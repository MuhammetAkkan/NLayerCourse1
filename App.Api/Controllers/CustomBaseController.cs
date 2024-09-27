using App.Services.ServiceResult;
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
        public IActionResult CustomActionResult<T>(ServiceResult<T> result)
        {
            if(result.Status == HttpStatusCode.NoContent)
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


        [NonAction] //end point olmasını istemiyorum.
        public IActionResult CustomActionResult(ServiceResultEmpty result)
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
