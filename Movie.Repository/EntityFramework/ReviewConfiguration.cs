using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie.Repository.Entities;
using Movie.Repository.SeedData;

namespace Movie.Repository.EntityFramework
{
    internal class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        /// <summary>
        /// Provides the Entity Framework configuration for 
        /// </summary>
        /// <param name="builder">Type of <see cref="EntityTypeBuilder"/>, containing <see cref="Review"/></param>
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.Property(x => x.Score).IsRequired();

            builder.Property(x => x.MovieId).IsRequired();

            builder.Property(x => x.ReviewerId).IsRequired();

            builder.HasData(ReviewData.Fetch());
        }
    }
}
