using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieAPI.Models.Entities;
using MovieAPI.Models.Entities.Common;
using MovieAPI.Models.Enum;

namespace MovieAPI.EntityFramework
{
    internal class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        /// <summary>
        /// Provides the Entity Framework configuration for 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.Property( x => x.Genre)
                .HasConversion(m => m.Value, m => GenreType.FromValue(m));

            builder.Property(x => x.RunningTime)
                .HasConversion(m => m.Value, m => (RunningTime) m);

            builder.Property(x => x.YearOfRelease)
                .HasConversion(m => m.Value, m => (ReleaseYear) m);

            // is this still needed?
            builder.Property(x => x.GetAverageScore);
        }
    }
}
