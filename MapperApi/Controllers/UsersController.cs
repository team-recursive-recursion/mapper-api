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
        private readonly CourseDb _context;

        public UsersController(CourseDb context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // GET: api/users
        [Route("api/users")]
        [HttpGet]
        public IEnumerable<User> GetUser()
        {
            return _context.Users;
        }

        // POST: api/users
        [Route("api/users")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            user.Password = null;
            return CreatedAtAction("Create", new {id = user.UserID}, user);
        }

        // GET: api/users/{id}
        [Route("api/users/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetGolfCourse([FromRoute] Guid id)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var user = await _context.Users
                    .Include(m => m.Courses)
                    .SingleOrDefaultAsync(m => m.UserID == id);

            if (user == null) {
                return NotFound();
            }
            user.Password = null;

            await _context.Entry(user)
                    .Collection(b => b.Courses)
                    .LoadAsync();

            return Ok(user);
        }

        [AllowAnonymous]
        [Route("api/users/match")]
        [HttpPost()]
        public IActionResult Authenticate([FromBody]User userParam)
        {
            var user = _userService.Authenticate(userParam.Email, userParam.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }
        private bool EmailExists(string email)
        {
            return _context.Users.Any(e => e.Email == email);
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }
    }
}
