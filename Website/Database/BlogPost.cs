using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Website.Database.ModelBinder;

namespace Website.Database
{
    [Index(nameof(Slug), IsUnique = true)]
    public class BlogPost
    {
        [Key] public Guid Id { get; init; } = Guid.CreateVersion7();

        [Display(Name = "Author")]
        [ForeignKey("Author")]
        public string? AuthorId { get; init; }

        public Author? Author { get; init; }

        [Display(Name = "Slug")] public string Slug { get; init; } = null!;

        [Display(Name = "Title")] public string Title { get; init; } = null!;

        [Display(Name = "Abstract")] public string? Abstract { get; init; }

        [Display(Name = "Content")] public string? Content { get; init; }

        public string? ContentHtml { get; set; }

        public List<BlogPostTag> BlogPostTags { get; } = new() { };
        public List<Tag> Tags { get; } = new() { };

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public Instant CreatedAt { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public Instant UpdatedAt { get; init; } = Instant.FromDateTimeUtc(DateTime.UtcNow);

        [ModelBinder(BinderType = typeof(InstantModelBinder))]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        public Instant? PublishedAt { get; init; }
    }
}