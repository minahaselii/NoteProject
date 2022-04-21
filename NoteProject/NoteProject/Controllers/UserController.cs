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
using NoteProject.PicServiice.Commands.DeletePic;
using NoteProject.PicServiice.Commands.UploadPic;
using NoteProject.PicServiice.FacadPicManager;

namespace NoteProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IDatabaseContext _datbaseContext;
        private readonly IFacadPic _facadPic;

        public object ViewBag { get; private set; }

        public UserController(IDatabaseContext datbaseContext, IFacadPic facadPic)
        {
            _datbaseContext = datbaseContext;
            _facadPic = facadPic;
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

            return Ok(new { name = user.FirstName, token = user.Token, exp = user.tokenExp, IsAdmin = user.IsAdmin ,Id = user.Id});


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
        public async Task<IActionResult> SetProfile([FromForm] SetProfileDto request)
        {
            var user = await _datbaseContext.Users
                  .Where(u => u.Token.Equals(request.Token) && u.tokenExp > DateTime.Now)
                  .FirstOrDefaultAsync();

            if (user != null)
            {
                if (user.HasProfile)
                {
                    var ProfileObj = _datbaseContext.Profile.Where(p => p.UserId == request.UserId).FirstOrDefault();
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

                    if (!string.IsNullOrWhiteSpace(ProfileObj.ProfileImage))
                    {
                        var deleteUpload = _facadPic.DeletePicService().DeletePic(new DeletePicDto
                        {
                            picAddress = ProfileObj.ProfileImage
                        });
                    }
                    var uploadResult = await _facadPic.UploadPicService().UploadFile(new PicInsertDto
                    {
                        height = request.height,
                        quality = request.quality,
                        width = request.width,
                        pic_file = request.pic_file,
                        uploader_id = 0,
                        EntityName = "ProfilePics"
                    });

                    if (!uploadResult.IsSuccess)
                    {
                        return BadRequest(new ResultDto
                        {
                            IsSuccess = false,
                            Message = "خطا در آپلود عکس "
                        });
                    }

                    ProfileObj.ProfileImage = uploadResult.Data;
                    try
                    {
                        await _datbaseContext.SaveChangesAsync();
                        return Ok(new ResultDto
                        {
                            IsSuccess = true,
                            Message = "عملیات با موفقیت انجام شد"
                        });
                    }
                    catch
                    {
                        return BadRequest(new ResultDto
                        {
                            IsSuccess = false,
                            Message = "خطا"
                        });
                    }
                }
                else
                {
                    var ProfileObj = new Profile()
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
                        UserId = user.Id
                    };

                    var uploadResult = await _facadPic.UploadPicService().UploadFile(new PicInsertDto
                    {
                        height = request.height,
                        quality = request.quality,
                        width = request.width,
                        pic_file = request.pic_file,
                        uploader_id = 0,
                        EntityName = "ProfilePics"
                    });

                    if (!uploadResult.IsSuccess)
                    {
                        return BadRequest(new ResultDto
                        {
                            IsSuccess = false,
                            Message = "خطا در آپلود عکس "
                        });
                    }

                    ProfileObj.ProfileImage = uploadResult.Data;
                    user.HasProfile = true;

                    try
                    {
                        await _datbaseContext.Profile.AddAsync(ProfileObj);
                        await _datbaseContext.SaveChangesAsync();

                        return Ok(new ResultDto
                        {
                            IsSuccess = true,
                            Message = "عملیات با موفقیت انجام شد"
                        });
                    }
                    catch (Exception)
                    {
                        return BadRequest(new ResultDto
                        {
                            IsSuccess = false,
                            Message = "خطا در ویرایش پروفایل "
                        });
                    }
                }

            }

            else
            {
                return BadRequest(new ResultDto<string>
                {
                    IsSuccess = false,
                    Message = "کاربر با این مشخصات یافت نشد"
                });
            }

        }
        //showProfile
        [HttpPost]
        public async Task<IActionResult> ShowProfile(ShowProfileDto request)
        {
            var user = await _datbaseContext.Users
                  .Where(u => u.Token.Equals(request.Token) && u.tokenExp > DateTime.Now)
                  .FirstOrDefaultAsync();

            if (user != null)
            {
               
                var requestedUser = _datbaseContext.Users.Where(u => u.Id == request.UserId).FirstOrDefault();
                if (requestedUser.HasProfile == true){
                    var ProfileObj = _datbaseContext.Profile.Where(p => p.UserId == request.UserId).FirstOrDefault();
                    ProfileResultDto result = new ProfileResultDto
                    {
                        Age = ProfileObj.Age,
                        City = ProfileObj.City,
                        Village = ProfileObj.Village,
                        JobStatus = ProfileObj.JobStatus,
                        JobTitle = ProfileObj.JobTitle,
                        JobType = ProfileObj.JobType,
                        Instagram = ProfileObj.Instagram,
                        Linkedin = ProfileObj.Linkdin,
                        EducationNum = ProfileObj.EducationNum,
                        ProfileDesc = ProfileObj.ProfileDesc,
                        IMGSrc = _facadPic.GetPicUrlService().GetUrl(ProfileObj.ProfileImage).Data,

                    };
                    
                    return Ok(new ResultDto<ProfileResultDto>
                    {
                        IsSuccess = true,
                        Data = result,
                        Message = "عملیات با موفقیت انجام شد"
                    });
                }
                else
                {
                   
                    return BadRequest(new ResultDto
                    {
                        IsSuccess = false,
                        Message = "کاربر مورد نظر پروفایل ندارد "
                    });
                }
            }
            else
            {
                return BadRequest(new ResultDto
                {
                    IsSuccess = false,
                    Message = "کاربر موجود نیست "
                });
            }
        }
    }
    }

