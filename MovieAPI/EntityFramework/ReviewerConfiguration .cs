using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieAPI.Models.Entities;
using MovieAPI.Models.Entities.Common;
using MovieAPI.Models.Enum;

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
            builder.Property(x => x.Email)
                .HasConversion(m => m.Address, m => new System.Net.Mail.MailAddress(m));
        }
    }
}
