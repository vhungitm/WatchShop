namespace Model
{
    using System.ComponentModel.DataAnnotations;

    public class Login
    {
        [Key]
        [Required(ErrorMessage = "Vui lòng nhập tài khoản")]
        public string Username { set; get; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string Password { set; get; }

        public bool Rememeber { set; get; }

        public string GroupID { set; get; }

    }
}
