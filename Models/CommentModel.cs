using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Azure.Identity;

namespace EmreSarıyer.Models;
public class CommentModel{
    [Key]
    public int Commentid {get; set;}
    [Required]
    public int Blogid {get; set;}
    [Required]
    public int Userid{get; set;}
    [Required]
    public string Comment {get; set;}
    [Required]
    public DateTime date {get; set;}
    public string UserName {get; set;}
    public string UserSurname {get; set;}

}