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
using Microsoft.AspNetCore.Mvc;
using Mapper_Api.Context;
using Mapper_Api.Models;

namespace Mapper_Api
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UserController : Controller
    {
        private readonly CourseDb _context;

        public UserController(CourseDb context)
        {
            _context = context;
        }

        // GET: api/Users
        public string Match(string email, string password)
        {
            // TODO
            if (email == "gmail.com" && password == "hello") {
                return "correct login";
            } else {
                return "incorrect login";
            }
        }

        // TODO post for registration

    }
}
