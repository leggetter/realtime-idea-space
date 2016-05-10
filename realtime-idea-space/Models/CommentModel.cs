using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace realtime_idea_space.Models
{
    public class CommentModel
    {
        public IdeaModel _idea;

        public CommentModel() { Id = Guid.NewGuid(); Created = DateTime.Now; }

        public CommentModel(Guid ideaModelId, string text, string commentByUserId):
            this()
        {
            IdeaModelId = ideaModelId;
            Text = text;
            CommentByUserId = commentByUserId;
        }

        public Guid Id { get; set; }

        public Guid IdeaModelId { get; set; }

        [JsonIgnore]
        public IdeaModel ParentIdeaModel
        {
            get
            {
                if (_idea == null)
                {
                    var db = new ApplicationDbContext();
                    _idea = db.IdeaModels.First(idea => idea.Id == this.IdeaModelId);
                }
                return _idea;
            }
        }

        [StringLength(2000), MinLength(2), Required]
        [DataType(DataType.MultilineText)]
        [Display(Name ="Leave a comment")]
        public string Text { get; set; }

        [ScaffoldColumn(false)]
        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }

        [Required]
        public string CommentByUserId { get; set; }
    }
}