﻿using System;
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
            IQueryable<GetNoteResultDto> noteresults = null;

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
            var finalResult =await  noteresults.ToListAsync();
            return Ok(new ResultDto<List<GetNoteResultDto>>
            {
                IsSuccess=true,
                Data=finalResult
                
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
        //update note
        [HttpPost]
        public async Task<IActionResult> UpdateNote(UpdateNoteDto request)
        {
            var user = await _datbaseContext.Users
                    .Where(u => u.Token == request.Token && u.tokenExp > DateTime.Now)
                    .Where(u => u.IsAdmin == false)
                    .Select(u => u)
                    .FirstOrDefaultAsync();



            if (user == null)
            {
                return BadRequest("توکن نامعتبراست ");
            }
            /*List<Note> noteList = user.notes;
            Boolean flag = false;
            

            foreach (var Note in noteList)
            {
                if (Note.Id == request.noteId)
                {
                    flag=true;
                }
            }
            if (flag == false)
            {
                return BadRequest("توکن نامعتبراست ");
            }*/



            IQueryable<User> users = _datbaseContext.Users;
            IQueryable<Note> notes = _datbaseContext.Notes;
            IQueryable<GetNoteResultDto> noteresults = null;



            noteresults = from n in notes
                          join u in users
                          on n.UserId equals u.Id
                          where n.Id == request.noteId
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
            var note = await noteresults.FirstOrDefaultAsync();
            note.Desc = request.Desc;
            note.Title = request.Title;
            await _datbaseContext.SaveChangesAsync();



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
        //admin update
        public async Task<IActionResult> UpdateNoteByAdmin(UpdateNoteDto request)

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
                notes.Where(n => n.Category.Equals(request.Category));
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
            var finalComfored = await noteresults.Where(n => n.Id == request.noteId).Where(n => n.IsConfirmed == true).FirstOrDefaultAsync();
            var finalNotComfored = await noteresults.Where(n => n.Id == request.noteId).Where(n => n.IsConfirmed == false).FirstOrDefaultAsync();

            finalComfored.Desc = request.Desc;
            finalComfored.Title = request.Title;


            return Ok(new ResultDto<List<GetNoteResultDto>>
            {
                IsSuccess = true,


            });
        }
    }
}
