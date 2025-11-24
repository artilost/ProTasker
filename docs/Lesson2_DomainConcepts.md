# Ders 2: Domain Katmanı ve Mimari Kararlar

Bu doküman, şu ana kadar yazdığımız kodların **neden** yazıldığını ve **sektörde ne işe yaradığını** açıklar.

## 1. Neden Katmanlı Mimari? (Clean Architecture)

Eskiden projeleri tek bir katmanda (örneğin sadece WebApi projesi içinde) yazardık. Veritabanı kodları, iş kuralları ve API kodları iç içe geçerdi. Buna "Spaghetti Code" denir.

**Bizim Yaptığımız (Onion/Clean Architecture):**
Projeyi soğan halkaları gibi düşündük. En içte **Domain** var.

*   **Domain (Çekirdek):** Hiçbir dış kütüphaneye (EF Core, Newtonsoft vb.) bağımlı değildir. Sadece C# dilinin özelliklerini kullanır. Bu sayede veritabanını değiştirsek bile (SQL Server -> MongoDb), Domain kodlarımız **değişmez**.
*   **Bağımlılık Yönü:** Dış katmanlar iç katmanları bilir, ama iç katmanlar dışarıyı bilmez. `Domain` kimseyi tanımaz.

## 2. BaseEntity ve AuditableEntity Nedir?

Kod tekrarını önlemek (DRY - Don't Repeat Yourself) için bu sınıfları oluşturduk.

### BaseEntity
Her tablonun bir `Id`'si olmak zorundadır. Her entity'de tek tek `public Guid Id { get; set; }` yazmak yerine, hepsini `BaseEntity`den türettik.
*   **Guid:** `int` (1, 2, 3) yerine `Guid` (benzersiz uzun kod) kullanıyoruz. Bu, verileri birleştirmeyi kolaylaştırır ve tahmin edilebilirliği (güvenlik açığı) önler.

### AuditableEntity
Kurumsal projelerde "Bu kaydı kim oluşturdu?", "En son kim güncelledi?" soruları çok önemlidir.
*   `CreatedBy`, `CreatedOn`: Kayıt ilk oluştuğunda dolar.
*   `LastModifiedBy`, `LastModifiedOn`: Her güncellemede değişir.
Bunu her entity'e (Project, TodoItem) tek tek yazmak yerine, bu sınıftan miras alarak otomatik hale getirdik.

## 3. Rich Domain Model vs Anemic Domain Model

En önemli konsept budur.

### Anemic Model (Zayıf Model - Kaçındığımız)
Sadece veri tutan, aptal kutular.
```csharp
public class TodoItem {
    public bool IsDone { get; set; } // Herkes değiştirebilir
}
// Kullanımı:
var task = new TodoItem();
task.IsDone = true; // Kontrolsüz değişiklik
```

### Rich Model (Zengin Model - Yaptığımız)
Veriyi ve o veriyi değiştiren kuralları bir arada tutan akıllı nesneler.
```csharp
public class TodoItem {
    // 1. private set: Dışarıdan kimse kafasına göre değiştiremez.
    public bool IsDone { get; private set; } 

    // 2. Constructor: Nesne oluşurken zorunlu alanları isteriz.
    public TodoItem(string title) {
        if (string.IsNullOrEmpty(title)) throw new Exception("Başlık boş olamaz!");
        Title = title;
        IsDone = false; // Varsayılan değer
    }

    // 3. Metodlar: Değişiklik yapmak isteyen bu metodları kullanmak ZORUNDADIR.
    public void MarkAsDone() {
        if (IsDone) return; // Kural: Zaten bitmişse işlem yapma.
        IsDone = true;
    }
}
```

**Bize Ne Kazandırdı?**
*   **Güvenlik:** Veri bütünlüğü bozulmaz. (Örn: Başlıksız görev oluşamaz).
*   **Okunabilirlik:** `task.MarkAsDone()` kodu, `task.IsDone = true` kodundan daha anlaşılırdır.
*   **Bakım:** "Görev tamamlanınca e-posta at" kuralı gelirse, sadece `MarkAsDone` metodunun içine yazarız. Diğer türlü projenin her yerinde `IsDone = true` yapılan yerleri arayıp değiştirmemiz gerekirdi.

## 4. Value Objects (Enum)
`PriorityLevel` (Low, Medium, High) gibi sabit listeleri kod içinde "sihirli sayılar" (0, 1, 2) olarak tutmak yerine `Enum` yaptık. Bu sayede kod okunabilir oldu (`if (priority == PriorityLevel.High)`).
