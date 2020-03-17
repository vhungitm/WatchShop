using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace WatchShop.Models
{
    public class RegisterModel
    {
        [Key]
        public long Id { set; get; }

        [Display(Name="Tài khoản")]
        [Required(ErrorMessage = "Vui lòng nhập tài khoản")]
        public string Username { set; get; }

        [Display(Name="Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Độ dài mật khẩu phải từ 6-20 ký tự")]
        public string Password { set; get; }

        [Display(Name="Xác nhận mật khẩu")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "Mật khẩu xác nhận không đúng")]
        public string ConfirmPassword { set; get; }

        [Display(Name="Họ tên")]
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string Name { set; get; }

        [Display(Name="Địa chỉ")]
        public string Address { set; get; }

        [Display(Name="Email")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ Email")]
        public string Email { set; get; }

        [Display(Name="Số điện thoại")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string Phone { set; get; }
    }
}