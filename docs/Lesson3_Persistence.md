# Ders 3: Veri Erişimi ve EF Core (Persistence)

Bu ders, uygulamamızın veritabanı ile nasıl konuştuğunu ve "Code-First" yaklaşımını anlatır.

## 1. Entity Framework Core (EF Core) Nedir?
EF Core, bir **ORM (Object-Relational Mapper)** aracıdır.
*   **Eskiden:** SQL sorguları yazardık (`SELECT * FROM Users WHERE ...`).
*   **Şimdi:** C# kodları yazıyoruz (`context.Users.Where(...)`). EF Core bunu bizim için SQL'e çeviriyor.

## 2. Neden `IEntityTypeConfiguration` Kullanıyoruz?
`DbContext` sınıfı, veritabanı ayarlarının yapıldığı yerdir. Ancak yüzlerce tablonuz olduğunda, tüm ayarları `OnModelCreating` metoduna yazmak bu dosyayı okunamaz hale getirir (God Class).
Bu yüzden her tablonun ayarını (kolon boyutu, zorunluluk, ilişki) kendi dosyasına (`ProjectConfiguration`, `TodoItemConfiguration`) ayırdık.
*   **Single Responsibility Principle (SRP):** Her sınıfın tek bir sorumluluğu olmalı.

## 3. Code-First Migration Nedir?
Klasik yöntemde önce SQL'de tabloları oluşturur, sonra C# sınıflarını yazardık.
**Code-First** yaklaşımında ise:
1.  Önce C# sınıflarını (Entity) yazarız.
2.  `dotnet ef migrations add InitialCreate` komutunu çalıştırırız.
3.  EF Core, kodumuza bakıp "Sen `Project` sınıfı yazmışsın, ben buna uygun `CREATE TABLE Projects...` SQL kodunu hazırladım" der.
4.  `dotnet ef database update` dediğimizde bu SQL veritabanında çalıştırılır.

**Avantajı:**
*   Veritabanı şemasını versiyonlayabiliriz (Git ile).
*   Ekipteki herkes aynı veritabanı yapısına sahip olur.
*   SQL bilmeden veritabanı tasarlayabiliriz.

## 4. Interceptors (Otomatik Audit)
`ApplicationDbContext` içinde `SaveChangesAsync` metodunu ezdik (override).
Bu sayede;
*   Siz her `context.SaveChanges()` dediğinizde,
*   Sistem araya girip "Bu kayıt yeni mi? O zaman `CreatedBy` alanını doldur. Güncelleniyor mu? `LastModified` alanını doldur" der.
*   Böylece her işlemde manuel olarak tarih atamak zorunda kalmayız.
