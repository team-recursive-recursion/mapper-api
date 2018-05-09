/***
 * Filename: ApiEndpointsController.cs
 * Author : ebendutoit
 * Class   : ApiEndpointsController
 *
 *      Website entrypoint for api discovery
 ***/
using Microsoft.AspNetCore.Mvc;

namespace Mapper_Api.Controllers
{
    public class ApiEndpointsController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return
                    View();
        }
    }
}