using NoteProject.Dto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteProject.Dto.Note
{
    public class GetNoteDto: TokenDto
    {
        public int Id { get; set; }
        public bool IsClientSide { get; set; }

    }
}
