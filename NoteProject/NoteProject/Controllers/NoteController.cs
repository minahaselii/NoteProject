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
            var user =await _datbaseContext.Users
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
                User=user
            };
            await _datbaseContext.Notes.AddAsync(note);
            try
            {
                await _datbaseContext.SaveChangesAsync();
                return Ok(new ResultDto<int>
                {
                    IsSuccess=true,
                    Data= note.Id,
                    Message="عملیات با موفقیت انجام شد"
                });
            }
            catch
            {
                return  BadRequest(new ResultDto<int>
                {
                    IsSuccess = false,
                    Message = "خطا"
                });
            }

        }
        [HttpPost]
        public async Task<IActionResult> GetNoteList(GetNoteListDto request)
        {
            if (!request.IsClientSide)
            {
                var adminuser = await _datbaseContext.Users
                    .Where(u => u.Token == request.Token && u.tokenExp > DateTime.Now)
                    .Where(u=>u.IsAdmin==true)
                    .Select(u => u)
                    .FirstOrDefaultAsync();

                if (adminuser == null)
                {
                    return BadRequest("توکن نامعتبراست ");
                }
            }

            IQueryable<User> users=_datbaseContext.Users;
            IQueryable<Note> notes=_datbaseContext.Notes;
            IQueryable<GetNoteListResultDto> noteresults = null;

            if (request.IsClientSide)
            {
               notes= notes.Where(n => n.IsConfirmed);
            }
            if(!string.IsNullOrWhiteSpace(request.Category))
            {
                //notes.Where(n => n.Category.Contains(request.Category));
                //notes.Where(n => n.Category.StartsWith(request.Category));
                notes.Where(n => n.Category.Equals(request.Category));
            }
            noteresults = from n in notes
                          join u in users
                          on n.UserId equals u.Id
                          select new GetNoteListResultDto
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
            var finalResult =await  noteresults.ToListAsync();
            return Ok(new ResultDto<List<GetNoteListResultDto>>
            {
                IsSuccess=true,
                Data=finalResult
                
            });

        }
    }
}
