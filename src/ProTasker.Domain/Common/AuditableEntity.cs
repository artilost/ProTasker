namespace ProTasker.Domain.Common;

// Bu sınıf, "Kim, Ne Zaman yaptı?" sorularına cevap veren alanları içerir.
// BaseEntity'den miras alır, yani Id özelliği buna da gelir.
// Abstract'tır, tek başına kullanılmaz.
public abstract class AuditableEntity : BaseEntity
{
    // Kaydın oluşturulma zamanı.
    // DateTimeOffset kullanıyoruz çünkü saat dilimi (TimeZone) bilgisini de tutar.
    // Global uygulamalarda DateTime yerine DateTimeOffset tercih edilir.
    public DateTimeOffset Created { get; set; }

    // Kaydı oluşturan kullanıcının Id'si veya adı.
    // Nullable (string?) çünkü sistem otomatik oluşturmuş olabilir (kullanıcı yok).
    public string? CreatedBy { get; set; }

    // Kaydın son güncellenme zamanı.
    public DateTimeOffset LastModified { get; set; }

    // Kaydı son güncelleyen kişi.
    public string? LastModifiedBy { get; set; }
}
