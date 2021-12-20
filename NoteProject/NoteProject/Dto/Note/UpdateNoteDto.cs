using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NoteProject.Dto.Common;


namespace NoteProject.Dto.Note
{
    public class UpdateNoteDto:TokenDto
    {
        public bool IsClientSide { get; set; }
        public int noteId { get; set; }
        public string Desc { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
    }
}
