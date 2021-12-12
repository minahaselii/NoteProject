﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteProject.Dto.Note
{
    public class GetNoteListResultDto
    {
        public int Id { get; set; }
        public string Desc { get; set; }
        public string Title { get; set; }
        public DateTime InsertTime { get; set; }
        public string Category { get; set; }
        public bool IsConfirmed { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
