using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteProject.Entity
{
    public class Note : EntityId<int>
    {
        public static List<string>  CategoryList =new List<string>(){
            "کارمندی",
            "فریلنسری",
            "آزاد",
        };

        public string Desc { get; set; }
        public string Title { get; set; }
        public DateTime InsertTime { get; set; }
        public string Category { get; set; }
        public bool IsConfirmed { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }

    }
}
