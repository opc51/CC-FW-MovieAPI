using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie.Repository.Entities.Common;
using Movie.Repository.Entities.Enum;
using Movie.Repository.SeedData;
using Entity = Movie.Repository.Entities;

namespace Movie.Repository.EntityFramework
{
    internal class MovieConfiguration : IEntityTypeConfiguration<Entity.Movie>
    {
        /// <summary>
        /// Provides the Entity Framework configuration for 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Entity.Movie> builder)
        {
            builder.Property(x => x.Genre)
                .HasConversion(m => m.Value, m => GenreType.FromValue(m));

            builder.Property(x => x.RunningTime)
                .HasConversion(m => m.Value, m => (RunningTime)m);

            builder.Property(x => x.YearOfRelease)
                .HasConversion(m => m.Value, m => (ReleaseYear)m);

            builder.HasData(MovieData.Fetch());
        }
    }
}
