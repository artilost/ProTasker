using System.ComponentModel.DataAnnotations.Schema;

namespace ProTasker.Domain.Common;

// Abstract: Bu sınıftan doğrudan nesne üretilemez (new BaseEntity() diyemeyiz).
// Sadece diğer sınıflar (Project, TodoItem) buna miras verebilir.
public abstract class BaseEntity
{
    // Her veritabanı kaydının benzersiz bir kimliği (Primary Key) olmalıdır.
    // Guid (Globally Unique Identifier) kullanıyoruz çünkü:
    // 1. Tahmin edilemez (Güvenlik). Ardışık sayılar (1, 2, 3) tahmin edilebilir.
    // 2. Dağıtık sistemlerde çakışma riski yoktur.
    public Guid Id { get; set; } = Guid.NewGuid();

    // Domain Event'leri (Olayları) tutmak için bir liste.
    // Örnek: "Görev Tamamlandı", "Proje Oluşturuldu" gibi olaylar burada birikir.
    // [NotMapped]: Bu listenin veritabanında bir kolon olarak OLUŞMAMASINI sağlar.
    // Çünkü bu olaylar veritabanında saklanmaz, sadece o anki işlem bitince fırlatılır.
    private readonly List<BaseEvent> _domainEvents = new();

    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    // Bir olay gerçekleştiğinde bu metoda göndeririz.
    public void AddDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    // Olay işlendikten sonra listeden silmek için.
    public void RemoveDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    // Tüm olayları temizlemek için.
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
