using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ProTasker.Domain.Common;
using ProTasker.Domain.Entities;

namespace ProTasker.Infrastructure.Persistence;

// DbContext: Uygulama ile Veritabanı arasındaki köprüdür.
// Veritabanı tablolarını C# nesneleri (DbSet) olarak temsil eder.
public class ApplicationDbContext : DbContext
{
    // Constructor: Ayarları (hangi veritabanı, connection string vb.) dışarıdan alır.
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Veritabanındaki "Projects" ve "TodoItems" tablolarına erişim sağlar.
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    // Model oluşturulurken çalışır. Tablo ayarlarını burada yükleriz.
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // "Bu assembly'deki (Infrastructure projesindeki) tüm IEntityTypeConfiguration sınıflarını bul ve uygula" der.
        // Yani ProjectConfiguration ve TodoItemConfiguration otomatik olarak devreye girer.
        // Tek tek "builder.ApplyConfiguration(new ProjectConfiguration())" yazmaktan kurtarır.
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    // SaveChanges: Veritabanına "Kaydet" komutu gönderildiğinde çalışır.
    // Biz bu metodu eziyoruz (override) ki araya girip kendi işlerimizi yapalım.
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // ChangeTracker: EF Core'un izlediği tüm nesneleri (eklenen, güncellenen) tutar.
        // Sadece AuditableEntity'den türeyenleri (Project, TodoItem) seçiyoruz.
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    // Yeni kayıt ekleniyorsa:
                    entry.Entity.CreatedBy = "System"; // İleride buraya gerçek kullanıcı adını koyacağız.
                    entry.Entity.Created = DateTimeOffset.UtcNow; // Şu anki zaman (UTC).
                    break;

                case EntityState.Modified:
                    // Kayıt güncelleniyorsa:
                    entry.Entity.LastModifiedBy = "System";
                    entry.Entity.LastModified = DateTimeOffset.UtcNow;
                    break;
            }
        }

        // İşlemler bittikten sonra orijinal SaveChanges metodunu çağırıp veritabanına yazıyoruz.
        return await base.SaveChangesAsync(cancellationToken);
    }
}
