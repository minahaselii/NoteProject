using NoteProject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteProject.Dto.Note
{
    public class GetNoteResultDto
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
        public Boolean IsLiked { get; set; }
        public int LikeNum { get; set; }
        public List<User> LikeUserList { get; set; }
    }
}
