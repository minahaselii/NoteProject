using NoteProject.Dto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteProject.Dto.Note
{
    public class AddCommentDto : TokenDto
    {
        public int NoteId { get; set; }
        public int UserId { get; set; }
        public String UserName { get; set; }
        public string Comments { get; set; }
        public DateTime InsertTime { get; set; }
    }
}
