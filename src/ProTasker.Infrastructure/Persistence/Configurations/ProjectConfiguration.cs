using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProTasker.Domain.Entities;

namespace ProTasker.Infrastructure.Persistence.Configurations;

// IEntityTypeConfiguration arayüzü, bir entity'nin veritabanı ayarlarını yapmak için kullanılır.
// Bu ayarları DbContext içinde yapmak yerine buraya ayırarak kodu temiz tutuyoruz (Single Responsibility).
public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        // Veritabanındaki tablo adı "Projects" olsun.
        builder.ToTable("Projects");

        // Primary Key (Birincil Anahtar) olarak Id özelliğini kullan.
        builder.HasKey(p => p.Id);

        // Name alanı:
        // 1. Maksimum 200 karakter olabilir (Veritabanı optimizasyonu için).
        // 2. Zorunludur (IsRequired), yani NULL olamaz.
        builder.Property(p => p.Name)
            .HasMaxLength(200)
            .IsRequired();

        // Description alanı:
        // 1. Maksimum 500 karakter.
        // 2. IsRequired demediğimiz için NULL olabilir (Opsiyonel).
        builder.Property(p => p.Description)
            .HasMaxLength(500);
            
        // İlişki Tanımı (One-to-Many):
        // Bir Projenin (One) çokça Görevi (Many) vardır.
        builder.HasMany(p => p.Items)
            .WithOne(t => t.Project)       // Her görevin bir projesi vardır.
            .HasForeignKey(t => t.ProjectId) // Yabancı anahtar ProjectId'dir.
            .OnDelete(DeleteBehavior.Cascade); // Proje silinirse, ona bağlı tüm görevler de silinsin.
    }
}
