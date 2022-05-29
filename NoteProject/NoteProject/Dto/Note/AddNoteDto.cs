using NoteProject.Dto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteProject.Entity
{
    public class AddNoteDto: TokenDto
    {
        public string Desc { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }

    
       


    }
}
