using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoteProject.Dto
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage ="نام الزامیست")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "نام خانوادگی الزامیست")]
        public string LastNmae { get; set; }

        [Required(ErrorMessage = "شماره همراه الزامیست")]
        [RegularExpression(@"^09\d{9}$", ErrorMessage = "شماره تلفن  با رقم 0 شروع و شامل 11 رقم میباشد")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "رمز عبور")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d@$%.!+-_*=();^&]{8,}$", ErrorMessage = "رمز عبور شامل حداقل 8 کاراکتر که خود شامل حداقل یک حرف بزگ ، یک حرف کوچک، ویک عدد میباشد")]
        public string Password { get; set; }
    }
}
