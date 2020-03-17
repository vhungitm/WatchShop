using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WatchShop.Areas.Admin.Models
{
    public class LoginModel
    {
        //Tài khoản
        [Required(ErrorMessage="Vui lòng nhập tên tài khoản!")]
        public string Username { set; get; }

        //Mật khẩu
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!")]
        public string Password { set; get; }
        public bool RememberMe { set; get; }
    }
}