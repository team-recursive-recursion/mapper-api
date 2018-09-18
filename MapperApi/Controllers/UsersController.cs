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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mapper_Api
{
    [Produces("application/json")]
    public class UsersController : Controller
    {
        private readonly CourseDb _context;

        public UsersController(CourseDb context)
        {
            _context = context;
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

            await _context.Entry(user)
                    .Collection(b => b.Courses)
                    .LoadAsync();

            return Ok(user);
        }

        // POST: api/users/match/
        [Route("api/users/match")]
        [HttpPost]
        public async Task<IActionResult> Match([FromBody] UserView uview)
        {
            var user = await _context.Users
                    .SingleOrDefaultAsync(u => u.Email == uview.Email);

            if (user == null) {
                return NotFound("Invalid username or password");
            }

            if (user.Password == uview.Password) {
                return Ok(user);
            }
            return NotFound("Invalid username or password");
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
