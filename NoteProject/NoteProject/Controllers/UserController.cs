using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteProject.Context;
using NoteProject.Dto;
using NoteProject.Entity;
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
        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterUserDto request)
        {
            int userId = await _datbaseContext.Users.
                Where(u => u.Phone == request.Phone)
                .Select(u => u.Id)
                .FirstOrDefaultAsync();

            if (userId == 0)
            {
                return BadRequest("کاربری با این شماره موجود است");
            }

            var user = new User
            {
                FirstName = request.FirstName,
                LastNmae = request.LastNmae,
                Password = request.Password,
                Phone = request.Phone
            };
            _datbaseContext.Users.Add(user);

            try
            {
                await _datbaseContext.SaveChangesAsync();
            }
            catch
            {
                return BadRequest("خطا در ثبت اطلاعات");
            }
            return Ok("ثبت اطلاعات با موفقیت انجام شد");


        }

        public async Task<IActionResult> Login(LoginDto request)
        {
            var user =await _datbaseContext.Users
                .Where(u => u.Phone == request.Phone && u.Password == request.Password)
                .Select(u => u)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return BadRequest("کاربری با این مشخصات موجود نیست");
            }

            string token = "";
            do
            {
                token = Guid.NewGuid().ToString();
            }
            while (_datbaseContext.Users.Any(u => u.Token == token && u.Id != user.Id));

            user.Token = token;
            user.tokenExp = DateTime.Now.AddMinutes(120);

            try
            {
                await _datbaseContext.SaveChangesAsync();
            }
            catch
            {
                return BadRequest("خطا در ثبت اطلاعات");
            }

            return Ok(new {name=user.FirstName,token=user.Token,exp=user.tokenExp ,IsAdmin=user.IsAdmin});


        }
        //new
        //logout
        public IActionResult Logout(LogoutDto request)
        {

            var user = _datbaseContext.Users.Select(u => u).Where(u => u.Token == request.Token).FirstOrDefault();
            user.tokenExp = DateTime.Now.AddMinutes(-1);


            try
            {
                _datbaseContext.SaveChanges();
                return Ok(new { name = user.FirstName, token = user.Token, exp = user.tokenExp });
            }
            catch
            {
                return BadRequest("خطا در ثبت اطلاعات");
            }


        }
    }
}
