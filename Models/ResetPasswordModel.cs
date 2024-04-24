using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Azure.Identity;

namespace EmreSarıyer.Models;
public class ResetPasswordModel{
    public string newpassword {get; set;}
    
    [Compare("newpassword",ErrorMessage = "Parolalar eşleşmiyor.")]
    [NotMapped]
    public string newpasswordRepeat {get; set;}
}