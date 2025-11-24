# Ders 4: Repository ve Unit of Work Pattern

Bu ders, veritabanı işlemlerini neden soyutladığımızı ve transaction yönetimini anlatır.

## 1. Repository Pattern Nedir?
Veritabanı erişim kodlarını (EF Core, SQL vb.) iş mantığından (Application katmanı) saklamak için kullanılan bir tasarım desenidir.

### Neden Kullanıyoruz?
*   **Soyutlama:** Yarın öbür gün EF Core yerine Dapper veya MongoDB kullanmak istersek, sadece `Repository` sınıfının içini değiştiririz. Business katmanındaki kodlar (`_repository.GetByIdAsync(id)`) değişmez.
*   **Test Edilebilirlik:** Unit Test yazarken veritabanına gitmek istemeyiz. Repository arayüzünü (`IRepository`) taklit eden (Mock) sahte sınıflar kullanarak testi hızlandırırız.
*   **Kod Tekrarını Önleme:** Her tablo için `Add`, `Delete`, `GetById` yazmak yerine `Repository<T>` ile tek bir generic sınıf yazdık.

## 2. Unit of Work Nedir?
Birden fazla repository ile yapılan işlemleri **tek bir transaction (işlem)** altında toplar.

### Senaryo
Bir e-ticaret sitesinde "Sipariş Ver" butonuna bastınız:
1.  Sipariş tablosuna kayıt atıldı.
2.  Stok tablosundan ürün düşüldü.
3.  Kullanıcıya puan eklendi.

Eğer 3. adımda hata olursa (Puan eklenemezse), ilk 2 adımın da iptal edilmesi (Rollback) gerekir. Yoksa stok düşer ama sipariş oluşmaz.

**Unit of Work Çözümü:**
```csharp
// 1. İşlemleri hafızada yap (Veritabanına gitmez)
await _projectRepository.AddAsync(project);
await _todoRepository.AddAsync(todo);

// 2. Tek seferde kaydet (Transaction)
await _unitOfWork.SaveChangesAsync(); 
```
Eğer `SaveChangesAsync` sırasında hata olursa, hiçbiri kaydedilmez. Veri bütünlüğü korunur.

## 3. Dependency Injection (DI) Ayarı
`Program.cs` veya `DependencyInjection.cs` içinde bu servisleri tanıttık:
```csharp
services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
```
*   **Scoped:** Her HTTP isteği (Request) için yeni bir repository oluşturulur. İstek bitince temizlenir. Veritabanı bağlantıları için en uygun yaşam döngüsüdür.
