using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace EmreSarıyer.Models;

public class BlogdetailModel{
    public BlogModel Blog { get; set; }
    public UserModel User { get; set; }
    public List<CommentModel> Comments { get; set; } = new List<CommentModel>();
    public List<UserModel> commentUsers {get; set;} = new List<UserModel>();
}