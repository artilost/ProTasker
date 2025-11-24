using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProTasker.Domain.Entities;

namespace ProTasker.Infrastructure.Persistence.Configurations;

public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        // Tablo adı "TodoItems"
        builder.ToTable("TodoItems");

        // Primary Key
        builder.HasKey(t => t.Id);

        // Başlık zorunlu ve max 200 karakter.
        builder.Property(t => t.Title)
            .HasMaxLength(200)
            .IsRequired();

        // Enum Dönüşümü (Conversion):
        // PriorityLevel enum'ı (Low=1, Medium=2) normalde veritabanında sayı (int) olarak tutulur.
        // Ancak veritabanına bakan biri "1"in ne olduğunu anlamayabilir.
        // HasConversion<string>() diyerek veritabanına "Low", "Medium" şeklinde metin olarak yazılmasını sağlıyoruz.
        // Dezavantajı: Biraz daha fazla yer kaplar. Avantajı: Okunabilirlik.
        builder.Property(t => t.Priority)
            .HasConversion<string>();
    }
}
