using BreakTimeApp.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BreakTimeApp.Models.EntityConfigurations
{
    public class TimeStoreItemDbConfiguration : IEntityTypeConfiguration<TimeStoreItemDb>
    {

        [LogAspect]
        public void Configure(EntityTypeBuilder<TimeStoreItemDb> builder)
        {
            builder.HasKey(t => t.ID);
            builder.Property(t => t.ID).IsRequired();
        }
    }
}
