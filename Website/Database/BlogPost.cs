using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NodaTime;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Website.Database.ModelBinder;

namespace Website.Database
{
    [Index(nameof(Slug), IsUnique = true)]
    public class BlogPost
    {
        [Key] public Guid Id { get; set; } = Guid.CreateVersion7();

        [Display(Name = "Author")]
        [ForeignKey("Author")]
        public string? AuthorId { get; set; }

        public Author? Author { get; set; }

        [Display(Name = "Slug")] public string Slug { get; set; } = null!;

        [Display(Name = "Title")] public string Title { get; set; } = null!;

        [Display(Name = "Abstract")] public string? Abstract { get; set; }

        [Display(Name = "Content")] public string? Content { get; set; }

        public string? ContentHtml { get; set; }

        public List<BlogPostTag> BlogPostTags { get; } = [];
        public List<Tag> Tags { get; } = [];

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public Instant CreatedAt { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public Instant UpdatedAt { get; set; } = Instant.FromDateTimeUtc(DateTime.UtcNow);

        [ModelBinder(BinderType = typeof(InstantModelBinder))]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        public Instant? PublishedAt { get; set; }
    }
}