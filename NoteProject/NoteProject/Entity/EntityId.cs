using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoteProject.Entity
{
    public class EntityId<T>
    {
        [Key]
        public T Id { get; set; }
    }
}
