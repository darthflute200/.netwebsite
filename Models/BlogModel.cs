using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmreSarıyer.Models;
public class BlogModel{
    [Key]
    public int Blogid {get; set;}
    public string BlogTitle {get; set;}
    public string BlogSubtitle{get; set;}
    public string Picture {get; set;}
    public string Text {get; set;}
    public string Youtubelink{get; set;}
}