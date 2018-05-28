/***
 * Filename: UserController.cs
 * Authoer : Duncan Tilley
 * Class   : UserController
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mapper_Api
{
    [Produces("application/json")]
    public class UserController : Controller
    {
        private readonly CourseDb _context;

        public UserController(CourseDb context)
        {
            _context = context;
        }

        // GET: api/users
        [Route("api/users")]
        [HttpGet]
        public IEnumerable<User> GetUser()
        {
            return _context.User;
        }

        // POST: api/Users/Match/
        [Route("api/users/match")]
        [HttpPost]
        public async Task<IActionResult> Match([FromBody] UserView uview)
        {
            var user = await _context.User
                    .SingleOrDefaultAsync(u => u.Email == uview.Email);

            if (user == null) return NotFound("The user does not exist.");

            if (user.Password == uview.Password)
                return Ok(user);
            return BadRequest("Invalid username or password");
        }

        // POST: api/users/create/
        [Route("api/users/create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Create", new {id = user.UserID}, user);
        }

        private bool EmailExists(string email)
        {
            return _context.User.Any(e => e.Email == email);
        }

        private bool UserExists(Guid id)
        {
            return _context.User.Any(e => e.UserID == id);
        }
    }
}
