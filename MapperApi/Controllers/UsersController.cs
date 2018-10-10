/***
 * Filename: UsersController.cs
 * Authoer : Duncan Tilley
 * Class   : UsersController
 *
 *     Contains the API controller componenet that handles queries related
 *     to users, login and registration.
 ***/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapper_Api.Context;
using Mapper_Api.Models;
using Mapper_Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mapper_Api
{
    [Produces("application/json")]
    public class UsersController : Controller
    {
        private IUserService _userService;
        private readonly IZoneService _zoneService;
        private LocationService _locationService;

        public UsersController(IUserService userService, IZoneService zoneServiceI, LocationService locationService)
        {
            _zoneService = zoneServiceI;
            _userService = userService;
            _locationService =  locationService;
        }

        // POST: api/users
        [Route("api/users")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                user = await _userService.CreateUserAsync(user);
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { error = e.Message });
            }
            return CreatedAtAction("Create", new { id = user.UserID }, user);
        }


// GET: api/courses
        [Route("api/courses/test/{CourseID}")]
        [HttpGet]
        public async Task<IEnumerable<LiveLocation>> getLiveLocation([FromRoute]String CourseID)
        {
            return await _locationService.getRecentPlayerLocation(CourseID); //TODO test this
        }

        [AllowAnonymous]
        [Route("api/users/match")]
        [HttpPost()]
        public async Task<IActionResult> Authenticate([FromBody]User userParam)
        {
            var user = await _userService.Authenticate(userParam.Email, userParam.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }
    }
}
