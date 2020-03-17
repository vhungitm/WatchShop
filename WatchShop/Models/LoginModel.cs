using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WatchShop.Models
{
    public class LoginModel
    {
        [Key]
        [Required(ErrorMessage = "Vui lòng nhập tài khoản")]
        public string Username { set; get; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string Password { set; get; }

    }
}