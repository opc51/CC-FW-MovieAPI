using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie.Domain.Enum;
using Movie.Repository.SeedData;

namespace Movie.Repository.EntityFramework
{
    internal class MovieConfiguration : IEntityTypeConfiguration<Domain.Movie>
    {
        /// <summary>
        /// Provides the Entity Framework configuration for 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Domain.Movie> builder)
        {
            builder.Property(x => x.Genre)
                .HasConversion(m => m.Value, m => GenreType.FromValue(m));

            builder.Property(x => x.RunningTime)
                .HasConversion(m => m.Value, m => (Domain.RunningTime)m);

            builder.Property(x => x.YearOfRelease)
                .HasConversion(m => m.Value, m => (Domain.ReleaseYear)m);

            builder.HasData(MovieData.Fetch());
        }
    }
}
