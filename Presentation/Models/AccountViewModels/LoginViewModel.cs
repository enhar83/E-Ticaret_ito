using System.ComponentModel.DataAnnotations;

namespace Presentation.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı kısmı boş geçilemez")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Şifre zorunludur")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
