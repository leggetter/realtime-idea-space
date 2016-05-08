using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace realtime_idea_space.Models
{
    public class CommentModel
    {
        public CommentModel() { Id = Guid.NewGuid(); Created = DateTime.Now; }

        public Guid Id { get; set; }

        public Guid IdeaModelId { get; set; }

        [StringLength(2000), MinLength(20), Required]
        [DataType(DataType.MultilineText)]
        [Display(Name ="Leave a comment")]
        public string Text { get; set; }

        [ScaffoldColumn(false)]
        public DateTime Created { get; set; }

        [Required]
        public string CommentByUserId { get; set; }
    }
}