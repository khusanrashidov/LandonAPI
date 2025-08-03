using Microsoft.AspNetCore.Mvc;
using System;

namespace Landon.Api.Controllers
{
    [Route("/[controller]")] // The route template pattern that maps HTTP requests to the controller. In our case, "/[controller]" is the route template. The [controller] placeholder is replaced by the name of the controller class, excluding the "Controller" suffix. Example: /Rooms.
    [ApiController]
    public class RoomsController
    {
        [HttpGet(Name = nameof(GetRooms))]
        public IActionResult GetRooms()
        {
            throw new NotImplementedException();
        }
    }
}
