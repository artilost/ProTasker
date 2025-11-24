# ProTasker 🚀

**ProTasker**, kurumsal standartlarda geliştirilen, ölçeklenebilir bir Görev ve Proje Yönetim Sistemidir.
Bu proje, **.NET Senior/Lead Developer** yetkinliklerini kazanmak amacıyla, Clean Architecture ve Domain-Driven Design (DDD) prensipleriyle sıfırdan geliştirilmektedir.

## 📚 Proje Amacı
Bu repo, sadece çalışan bir kod üretmeyi değil, **"Neden?"** ve **"Nasıl?"** sorularına cevap vermeyi hedefler. Her satır kodun arkasında bir mühendislik kararı vardır.

## 🏗 Mimari ve Teknolojiler
Proje **Clean Architecture (Onion Architecture)** yapısına uygun olarak katmanlara ayrılmıştır:

*   **Core (Domain):** Saf C# nesneleri, İş Kuralları (Rich Domain Model). Hiçbir dış bağımlılığı yoktur.
*   **Core (Application):** Use-Case'ler, CQRS, Validation. (Geliştiriliyor)
*   **Infrastructure:** Veritabanı (EF Core), E-posta, Dosya işlemleri.
*   **Presentation (WebApi):** RESTful API uçları.

**Kullanılan Teknolojiler:**
*   .NET 9
*   Entity Framework Core (Code-First)
*   SQL Server
*   Dependency Injection
*   Repository & Unit of Work Patterns
*   Domain Events

## 📂 Klasör Yapısı
```
src/
├── ProTasker.Domain/       # Entity'ler, Value Object'ler, Interface'ler
├── ProTasker.Application/  # (Yakında) Command/Query, DTO'lar
├── ProTasker.Infrastructure/# EF Core, Repository Implementasyonları
└── ProTasker.WebApi/       # Controller'lar, API Konfigürasyonu
```

## 🚀 Kurulum ve Çalıştırma

1.  Repoyu klonlayın.
2.  Veritabanını oluşturun (LocalDB kullanılır):
    ```bash
    dotnet ef database update -p src/ProTasker.Infrastructure -s src/ProTasker.WebApi
    ```
3.  Projeyi çalıştırın:
    ```bash
    dotnet run --project src/ProTasker.WebApi
    ```

## 📖 Ders Notları
Proje geliştirilirken hazırlanan eğitim dokümanları `docs/` klasöründedir:
*   [Ders 2: Domain Kavramları (Rich Model, BaseEntity)](docs/Lesson2_DomainConcepts.md)
*   [Ders 3: Veri Erişimi ve EF Core (Migrations, Configs)](docs/Lesson3_Persistence.md)
*   [Ders 4: Repository & Unit of Work](docs/Lesson4_RepositoryPattern.md)

---
*Geliştirme süreci devam etmektedir...*