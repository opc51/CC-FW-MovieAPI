using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieAPI.Models.Entities;

namespace MovieAPI.EntityFramework
{
    internal class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        /// <summary>
        /// Provides the Entity Framework configuration for 
        /// </summary>
        /// <param name="builder">Type of <see cref="EntityTypeBuilder"/>, containing <see cref="Review"/></param>
        public void Configure(EntityTypeBuilder<Review> builder)
        {
        }
    }
}
