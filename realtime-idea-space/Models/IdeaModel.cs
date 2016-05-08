using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace realtime_idea_space.Models
{
    public class IdeaModel
    {
        public IdeaModel() { Id = Guid.NewGuid(); Created = DateTime.Now; }

        public Guid Id { get; set; }

        [Required]
        public string CreatedByUserId { get; set; }

        [StringLength(50), Required]
        public string Title { get; set; }

        [ScaffoldColumn(false)]
        public DateTime Created { get; set; }

        [StringLength(2000), MinLength(20), Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public virtual ICollection<CommentModel> Comments { get; set; }
    }
}