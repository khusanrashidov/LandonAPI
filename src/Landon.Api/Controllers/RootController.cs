using Microsoft.AspNetCore.Mvc;

namespace Landon.Api.Controllers
{
    [Route("/")] // Slash or root route.
    [ApiController] // To explicitly tell ASP.NET Core that the controller is built and meant for an API, and to opt in to for features like automatic model validation.
    // Root controller that will server as the starting point for the API.
    public class RootController : ControllerBase // Controller inherits from ControllerBase and allows us to return views and allows us to use razor/blazor features.
    {
        [HttpGet(Name = nameof(GetRoot))] // Marks the endpoint, action method, to support and handle only an incoming HTTP GET verb method request.
        [ProducesResponseType(200)] // Define the response type for OpenAPI Spec.
        public IActionResult GetRoot() // Returning IActionResult as responses from endpoint HTTP verb method gives flexibility to return HTTP status code and/or JSON to requests.
        {
            var response = new
            {
                href = Url.Link(nameof(GetRoot), null), // An abosulute URL.
                rooms = new
                {
                    href = Url.Link(nameof(RoomsController.GetRooms), null) // Don't confuse and think it should be nameof(new RoomsController().GetRooms()).
                }
            };

            // An absolute URL is the full URL, including protocol (http/https), the optional sub-
            // domain (e.g. www), domain (example.com), and path (which includes the directory and slug).
            // Absolute URLs provide all the available information to find the location of a page.

            // In our case, eventually, this root controller essentially will return links to all of the other routes and controllers.

            return Ok(response); // Returns OK status code response with a JSON content body.
        }
    }
}
