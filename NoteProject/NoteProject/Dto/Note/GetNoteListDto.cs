using NoteProject.Dto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteProject.Dto.Note
{
    public class GetNoteListDto:TokenDto
    {
        public bool IsClientSide { get; set; }
        public string Category { get; set; }
    }
}
