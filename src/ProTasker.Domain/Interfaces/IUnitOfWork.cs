namespace ProTasker.Domain.Interfaces;

// Unit of Work (İş Birimi) Pattern:
// Birden fazla Repository ile yapılan işlemleri tek bir transaction (işlem) altında toplar.
// Senaryo: Hem Proje ekledik hem de içine 3 tane Görev ekledik.
// Eğer "Görev" eklerken hata alırsak, "Proje"nin de veritabanına yazılmamasını (Rollback) isteriz.
// UnitOfWork, "SaveChangesAsync" diyene kadar yapılan her şeyi hafızada tutar, tek seferde veritabanına yazar.
public interface IUnitOfWork
{
    // Değişiklikleri veritabanına kaydeder.
    // Dönüş değeri (int): Kaç kaydın etkilendiğini (eklendi/silindi) döner.
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
