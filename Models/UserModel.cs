using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmreSarıyer.Models;
public class UserModel{
    [Key]
    public int id {get; set;}
    [Required(ErrorMessage ="İsim alanı boş bırakılamaz")]
    public string Name {get; set;}
    [Required(ErrorMessage ="Soyadı alanı boş bırakılamaz")]
    public string Surname {get; set;}
    [Required(ErrorMessage ="E-mail alanı boş bırakılamaz")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    public string email {get; set;}
    [Required(ErrorMessage ="Parola alanı boş bırakılamaz")]
    public string Password {get; set;}
    
    [Compare("Password",ErrorMessage = "Parolalar eşleşmiyor.")]
    [NotMapped]
    public string PasswordRepeat {get; set;}
    public string UserRole {get; set;}
        public UserModel()
        {
            UserRole = "1";
        }
    public string ResetPasswordToken {get; set;}
    public DateTime? ResetPasswordExpiration{get; set;}
}