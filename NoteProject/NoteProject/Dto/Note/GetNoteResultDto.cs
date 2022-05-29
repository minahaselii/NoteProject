using NoteProject.Entity;

using System;
using System.Collections.Generic;

namespace NoteProject.Entity
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
        public List<Tag> TagList { get; set; }
    }
}
