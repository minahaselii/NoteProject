using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteProject.Context;
using NoteProject.Dto.Common;
using NoteProject.Dto.Note;
using NoteProject.Entity;

namespace NoteProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        IDatabaseContext _datbaseContext;
        public NoteController(IDatabaseContext datbaseContext)
        {
            _datbaseContext = datbaseContext;
        }

        [HttpPost]
        public async Task<IActionResult> AddNote(AddNoteDto addNoteDto)
        {
            var user = await _datbaseContext.Users
                .Where(u => u.Token == addNoteDto.Token && u.tokenExp > DateTime.Now)
                .Select(u => u)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return BadRequest("توکن نامعتبراست ");
            }
            var note = new Note
            {
                Desc = addNoteDto.Desc,
                Title = addNoteDto.Title,
                Category = addNoteDto.Category,
                InsertTime = DateTime.Now,
                User = user
            };
            await _datbaseContext.Notes.AddAsync(note);
            try
            {
                await _datbaseContext.SaveChangesAsync();
                return Ok(new ResultDto<int>
                {
                    IsSuccess = true,
                    Data = note.Id,
                    Message = "عملیات با موفقیت انجام شد"
                });
            }
            catch
            {
                return BadRequest(new ResultDto<int>
                {
                    IsSuccess = false,
                    Message = "خطا"
                });
            }

        }

        [HttpPost]
        public async Task<IActionResult> GetNote(GetNoteDto request)
        {
            var noteIQuryable = _datbaseContext.Notes.Where(n => n.Id == request.Id);
            Entity.User user = null;
            // isclintSide----> for loged in and loged out user
            if (!request.IsClientSide)
            {
                user = await GetUserIQueryable(request)
                    .Select(u => u)
                    .FirstOrDefaultAsync();
                if (user == null)
                {
                    return BadRequest("توکن نامعتبراست ");
                }
                if (!user.IsAdmin)
                {
                    noteIQuryable = noteIQuryable.Where(n => n.UserId == user.Id);
                }
            }
            else
            {
                noteIQuryable = noteIQuryable.Where(n => n.IsConfirmed == true);
            }
            var note = await noteIQuryable.FirstOrDefaultAsync();
            return Ok(new ResultDto<Note>
            {
                IsSuccess = true,
                Data = note
            });
        }

        [HttpPost]
        public async Task<IActionResult> GetNoteList(GetNoteListDto request)
        {
            if (!request.IsClientSide)
            {
                var adminuser = await _datbaseContext.Users
                    .Where(u => u.Token == request.Token && u.tokenExp > DateTime.Now)
                    .Where(u => u.IsAdmin == true)
                    .Select(u => u)
                    .FirstOrDefaultAsync();

                if (adminuser == null)
                {
                    return BadRequest("توکن نامعتبراست ");
                }
            }

            IQueryable<User> users = _datbaseContext.Users;
            IQueryable<Note> notes = _datbaseContext.Notes;
            IQueryable<GetNoteResultDto> noteresults = null;

            if (request.IsClientSide)
            {
                notes = notes.Where(n => n.IsConfirmed);
            }
            if (!string.IsNullOrWhiteSpace(request.Category))
            {
                //notes.Where(n => n.Category.Contains(request.Category));
                //notes.Where(n => n.Category.StartsWith(request.Category));
                notes = notes.Where(n => n.Category.Equals(request.Category));
            }
            noteresults = from n in notes
                          join u in users
                          on n.UserId equals u.Id
                          select new GetNoteResultDto
                          {
                              Id = n.Id,
                              UserId = u.Id,
                              Category = n.Category,
                              Desc = n.Desc,
                              FirstName = u.FirstName,
                              LastName = u.LastNmae,
                              InsertTime = n.InsertTime,
                              IsConfirmed = n.IsConfirmed,
                              Title = n.Title,

                          };
            var finalResult = await noteresults.ToListAsync();
            return Ok(new ResultDto<List<GetNoteResultDto>>
            {
                IsSuccess = true,
                Data = finalResult

            });

        }

        //new 
        [HttpPost]
        public async Task<IActionResult> GetNoteListForEachUser(GetNoteListForEachUserDto request)
        {

            var user = await _datbaseContext.Users
                    .Where(u => u.Token == request.Token && u.tokenExp > DateTime.Now)
                    .Select(u => u)
                    .FirstOrDefaultAsync();

            if (user == null)
            {
                return BadRequest("توکن نامعتبراست ");
            }

            IQueryable<User> users = _datbaseContext.Users;
            IQueryable<Note> notes = _datbaseContext.Notes;
            IQueryable<GetNoteResultDto> noteresults = null;


            noteresults = from n in notes
                          join u in users
                          on n.UserId equals u.Id
                          where u.Id == user.Id
                          select new GetNoteResultDto
                          {
                              Id = n.Id,
                              UserId = u.Id,
                              Category = n.Category,
                              Desc = n.Desc,
                              FirstName = u.FirstName,
                              LastName = u.LastNmae,
                              InsertTime = n.InsertTime,
                              IsConfirmed = n.IsConfirmed,
                              Title = n.Title,

                          };


            var finalResult = await noteresults.ToListAsync();
            return Ok(new ResultDto<List<GetNoteResultDto>>
            {
                IsSuccess = true,
                Data = finalResult

            });

        }

        private IQueryable<User> GetUserIQueryable(TokenDto request)
        {
            IQueryable<User> userIqueryable = _datbaseContext.Users;
            return userIqueryable = userIqueryable
                    .Where(u => u.Token == request.Token && u.tokenExp > DateTime.Now);
        }

        //update note
        [HttpPost]
        public async Task<IActionResult> UpdateNote(UpdateNoteDto request)
        {
            var user = await GetUserIQueryable(request)
                    .Select(u => u)
                    .FirstOrDefaultAsync();
            if (user == null)
            {
                return BadRequest("توکن نامعتبراست ");
            }


            IQueryable<Note> notesIQueryable = _datbaseContext.Notes
                .Where(n => n.Id == request.noteId);

            if (!request.IsClientSide)
            {
                if (!user.IsAdmin)
                {
                    return BadRequest("توکن نامعتبراست ");
                }
            }
            else
            {
                notesIQueryable = notesIQueryable
                    .Where(n => n.IsConfirmed && n.UserId == user.Id);
            }

            var note = await notesIQueryable.FirstOrDefaultAsync();
            if (note == null)
            {
                return BadRequest("نوت نامعتبر است");
            }

            note.Desc = request.Desc;
            note.Title = request.Title;
            note.Category = request.Category;

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
        //delete Note
        [HttpPost]
        public async Task<IActionResult> DeleteNote(UpdateNoteDto request)
        {
            var user = await GetUserIQueryable(request)
                    .Select(u => u)
                    .FirstOrDefaultAsync();
            if (user == null)
            {
                return BadRequest("توکن نامعتبراست ");
            }


            IQueryable<Note> notesIQueryable = _datbaseContext.Notes
                .Where(n => n.Id == request.noteId);

            if (!request.IsClientSide)
            {
                if (!user.IsAdmin)
                {
                    return BadRequest("توکن نامعتبراست ");
                }
            }
            else
            {
                notesIQueryable = notesIQueryable
                    .Where(n => n.IsConfirmed && n.UserId == user.Id);
            }

            var note = await notesIQueryable.FirstOrDefaultAsync();
            if (note == null)
            {
                return BadRequest("نوت نامعتبر است");
            }

            _datbaseContext.Notes.Remove(note);
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
        //AdminCheckNote
        [HttpPost]
        public async Task<IActionResult> AdminSetNoteStatus(AdminSetNoteStatusDto request)
        {
            var user = await GetUserIQueryable(request)
                    .Select(u => u)
                    .FirstOrDefaultAsync();
            if (user == null)
            {
                return BadRequest("توکن نامعتبراست ");
            }


            IQueryable<Note> notesIQueryable = _datbaseContext.Notes
                .Where(n => n.Id == request.noteId);

            if (!request.IsClientSide)
            {
                if (!user.IsAdmin)
                {
                    return BadRequest("توکن نامعتبراست ");
                }
            }
            else
            {
                notesIQueryable = notesIQueryable
                    .Where(n => n.UserId == user.Id);
            }

            var note = await notesIQueryable.FirstOrDefaultAsync();
            if (note == null)
            {
                return BadRequest("نوت نامعتبر است");
            }
            if (note.IsConfirmed)
            {
                note.IsConfirmed = false;
            }
            else
            {
                note.IsConfirmed = true;
            }



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

        //Like
        [HttpPost]
        public async Task<IActionResult> LikeNote(LikeDto request)
        {
            var user = await _datbaseContext.Users
                   .Where(u => u.Token == request.Token && u.tokenExp > DateTime.Now)
                   .Select(u => u)
                   .FirstOrDefaultAsync();
            var likeobj = await _datbaseContext.Likes.FirstOrDefaultAsync(l => l.NoteId == request.NoteId && l.UserId == user.Id);

            if (user == null)
            {
                return BadRequest("توکن نامعتبراست ");
            }

            if (likeobj != null)
            {
                if (likeobj.IsLiked == true)
                {

                    //return Ok(_datbaseContext.Users.Find(request.noteId));
                    try
                    {
                        return Ok(new ResultDto<String>
                        {
                            IsSuccess = true,
                            Data = "موفقیت",
                            Message = "این پست قبلا لایک شده بود قرمزش کن"
                        });
                    }
                    catch
                    {
                        return BadRequest(new ResultDto<String>
                        {
                            IsSuccess = false,
                            Data = "موفقیت",
                            Message = "خطا در عملیات لایک"
                        });
                    }
                }
                else if (likeobj.IsLiked == false)
                {
                    _datbaseContext.Likes.FirstOrDefault(l => l.NoteId == request.NoteId && l.UserId == user.Id && l.IsLiked==true);
                    //await _datbaseContext.SaveChangesAsync();
                    //return Ok(_datbaseContext.Users.Find(request.noteId));
                    try
                    {
                        await _datbaseContext.SaveChangesAsync();
                        return Ok(new ResultDto<String>
                        {
                            IsSuccess = true,
                            Data = "موفقیت",
                            Message = "لایک با موفقیت انجام شد"
                        });
                    }
                    catch
                    {
                        return BadRequest(new ResultDto<String>
                        {
                            IsSuccess = false,
                            Data = "موفقیت",
                            Message = "خطا در عملیات لایک"
                        });
                    }

                }
                else
                {
                    return BadRequest(new ResultDto<String>
                    {
                        IsSuccess = false,
                        Data = "موفقیت",
                        Message = "خطا در عملیات لایک"
                    });
                }
            }
            else
            {
                Like like = new Like()
                {
                    NoteId = request.NoteId,
                    UserId = user.Id,
                    IsLiked = true
                };
                _datbaseContext.Likes.Add(like);
                //await _datbaseContext.SaveChangesAsync();
                //return Ok(_datbaseContext.Notes.Find(request.noteId));
                try
                {
                    await _datbaseContext.SaveChangesAsync();
                    return Ok(new ResultDto<String>
                    {
                        IsSuccess = true,
                        Data = "موفقیت",
                        Message = "لایک با موفقیت انجام شد"
                    });
                }
                catch
                {
                    return BadRequest(new ResultDto<String>
                    {
                        IsSuccess = false,
                        Data = "موفقیت",
                        Message = "خطا در عملیات لایک"
                    });
                }

            }

            
        }

        //LikeList
        [HttpPost]
        public async Task<IActionResult> LikeList(GetNoteListDto request)
        {
             
            var user = await _datbaseContext.Users
                  .Where(u => u.Token.Equals(request.Token) && u.tokenExp > DateTime.Now)
                  .FirstOrDefaultAsync();

           
            IQueryable<Note> notes = _datbaseContext.Notes;
            IQueryable<Like> likes = _datbaseContext.Likes;
            IQueryable<GetLikeListResultDto> noteIDresults = null;
            IQueryable<LikeDto>likeresult = null; 
            IQueryable<GetLikeCheckListDto> likeCheckList = null;



            if (user == null)
            {
                return BadRequest();
            }
                likes = likes.Where(l => l.UserId == user.Id);
                likeresult = from l in likes
                             select new LikeDto
                             {
                                 NoteId = l.NoteId,
                                 IsLiked = l.IsLiked,
                                 UserId = user.Id,
                             };

            if (request.IsClientSide)
            {
                notes = notes.Where(n => n.IsConfirmed);
            }
            if (!string.IsNullOrWhiteSpace(request.Category))
            {
                
                notes = notes.Where(n => n.Category.Equals(request.Category));
            }
            noteIDresults = from n in notes 
                          select new GetLikeListResultDto
                          {
                              Id = n.Id,
                              IsConfirmed = n.IsConfirmed,
                          };
            var finalResultNoteId = await noteIDresults.ToListAsync();
            var finalResultLike = await likeresult.ToListAsync();
            List<GetLikeCheckListDto> finalChecktLike = new List<GetLikeCheckListDto>();
            foreach (var item in finalResultNoteId)
            {
                if (finalResultLike.Any(f => f.NoteId == item.Id && f.IsLiked == true))
                {
                    finalChecktLike.Add(new GetLikeCheckListDto(item.Id, 1)) ;
                }
                else if (finalResultLike.Any(f=>f.NoteId==item.Id &&f.IsLiked==false))
                {
                    finalChecktLike.Add(new GetLikeCheckListDto(item.Id, 0));
                }
               
            }
            
            return Ok(new ResultDto<List<GetLikeCheckListDto>>
            {
                IsSuccess = true,
                Data = finalChecktLike

            });





        }
    }
}