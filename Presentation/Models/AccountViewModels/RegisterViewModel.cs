using System.ComponentModel.DataAnnotations;

namespace Presentation.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "İsim kısmı boş geçilemez")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyisim kısmı boş geçilemez")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı kısmı boş geçilemez")]
        public string UserName { get; set; }

        [Required, EmailAddress(ErrorMessage = "Geçerli bir email giriniz")]
        public string Email { get; set; }

        [Required, Phone(ErrorMessage = "Geçerli bir telefon giriniz")]
        public string PhoneNumber { get; set; }

        [Required, DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalı")]
        public string Password { get; set; }

        [Required, DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor")]
        public string ConfirmPassword { get; set; }
    }
}
