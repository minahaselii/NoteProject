using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoteProject.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IDatabaseContext _datbaseContext;
        public UserController(IDatabaseContext datbaseContext)
        {
            _datbaseContext = datbaseContext;
        }

        [HttpGet]
        public IActionResult GetUser(int id)
        {
            
            var user= _datbaseContext.Users.Select(u => u).Where(u=>u.Id==id). FirstOrDefault();
            if(user != null)
            {
                return Ok(user);
            }
            return BadRequest("user not found");
            
        }
        public IActionResult RegisterUser()
        {
            return Ok();

        }
    }
}
