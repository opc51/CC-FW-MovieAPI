using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieAPI.Models.Domain;

namespace MovieAPI.EntityFramework
{
    internal class ReviewerConfiguration : IEntityTypeConfiguration<Reviewer>
    {
        /// <summary>
        /// Provides the Entity Framework configuration for 
        /// </summary>
        /// <param name="builder">An <see cref="EntityTypeBuilder"/> of type <see cref="Reviewer"/></param>
        public void Configure(EntityTypeBuilder<Reviewer> builder)
        {
            builder.Property(x => x.Name).IsRequired();

            builder.Property(x => x.Email).IsRequired();

            //builder.Property(x => x.Id)
            //    .ValueGeneratedOnAdd()
            //    .HasValueGenerator<ReviewerIdValueGenerator>();
        }
    }
}
