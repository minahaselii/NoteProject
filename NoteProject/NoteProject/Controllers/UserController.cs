using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NoteProject.Context;
using NoteProject.Dto;
using NoteProject.Dto.Common;
using NoteProject.Dto.Note;
using NoteProject.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace NoteProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IDatabaseContext _datbaseContext;

        public object ViewBag { get; private set; }

        public UserController(IDatabaseContext datbaseContext)
        {
            _datbaseContext = datbaseContext;
        }

        [HttpGet]
        public IActionResult GetUser(int id)
        {

            var user = _datbaseContext.Users.Select(u => u).Where(u => u.Id == id).FirstOrDefault();
            if (user != null)
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

            if (userId != 0)
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
            var user = await _datbaseContext.Users
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

            return Ok(new { name = user.FirstName, token = user.Token, exp = user.tokenExp, IsAdmin = user.IsAdmin });


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

        //save profile changes
        [HttpPost]
        public async Task<IActionResult> SetProfile(SetProfileDto request)
        {
            var user = await _datbaseContext.Users
                  .Where(u => u.Token.Equals(request.Token) && u.tokenExp > DateTime.Now)
                  .FirstOrDefaultAsync();
            Boolean CurrentUser;

            if (user == null)
            {
                return BadRequest("کاربری با این مشخصات موجود نیست");
            }
            else
            {
                if(user.Id == request.UserId)
                {
                    CurrentUser = true;
                }
                else
                {
                    CurrentUser = false;
                }
            }
            if (CurrentUser == true)
            {
                var ProfileObj = _datbaseContext.Profile.Where(p => p.UserId == request.UserId).FirstOrDefault();
                if (ProfileObj != null)
                {
                    ProfileObj.Age = request.Age;
                    ProfileObj.Village = request.Village;
                    ProfileObj.City = request.City;
                    ProfileObj.EducationNum = request.EducationNum;
                    ProfileObj.ProfileDesc = request.ProfileDesc;
                    ProfileObj.JobStatus = request.JobStatus;
                    ProfileObj.JobType = request.Jobtype;
                    ProfileObj.Instagram = request.Instagram;
                    ProfileObj.Linkdin = request.Linkdin;
                    /* ProfileObj.BirthTime = request.BirthTime;
                     ProfileObj.IAccept = request.IAccept;*/
                    ProfileObj.User = user;
                    try
                    {
                        await _datbaseContext.SaveChangesAsync();
                        return Ok(new ResultDto<String>
                        {
                            IsSuccess = true,
                            Data = "موفقیت",
                            Message = "عملیات با موفقیت انجام شد"
                        });
                    }
                    catch
                    {
                        return BadRequest(new ResultDto<String>
                        {
                            IsSuccess = false,
                            Data = "موفقیت",
                            Message = "خطا"
                        });
                    }
                }
                else
                {
                    var thisProfile = new Profile()
                    {
                        Age = request.Age,
                        Village = request.Village,
                        City = request.City,
                        EducationNum = request.EducationNum,
                        ProfileDesc = request.ProfileDesc,
                        JobStatus = request.JobStatus,
                        JobType = request.Jobtype,
                        Instagram = request.Instagram,
                        Linkdin = request.Linkdin,
                        /* BirthTime = request.BirthTime,
                         IAccept = request.IAccept,*/
                        User = user,


                    };


                    try
                    {
                        await _datbaseContext.Profile.AddAsync(thisProfile);
                        await _datbaseContext.SaveChangesAsync();

                        return Ok(new ResultDto<String>
                        {
                            IsSuccess = true,
                            Data = "موفقیت",
                            Message = "عملیات با موفقیت انجام شد"
                        });
                    }
                    catch
                    {
                        return BadRequest(new ResultDto<Profile>
                        {
                            IsSuccess = false,
                            Data = thisProfile,
                            Message = "خطا در ویرایش پروفایل "
                        });
                    }
                }

            }
            else if(CurrentUser == false)
            {
                SetProfileDto ShowProfile = null;
                ShowProfile.Age = user.Profile.Age;

                return Ok(new ResultDto<Profile>
                {
                    IsSuccess = true,
                    Data = user.Profile,
                    Message = "نمایش پروفایل به کاربر سایت"
                });
            }
            else
            {
                return BadRequest(new ResultDto<string>
                {
                    IsSuccess = false,
                    Data = "خطا",
                    Message = "خطا در نمایش پروفایل به کاربران سایت  "
                });
            }

        }

        }
    }

