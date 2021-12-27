using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NoteProject.Dto.Common;


namespace NoteProject.Dto.Note
{
    public class AdminSetNoteStatusDto : TokenDto
    {
        public bool IsClientSide { get; set; }
        public int noteId { get; set; }
    }
}
