using ProTasker.Domain.Common;

namespace ProTasker.Domain.Interfaces;

// Repository Pattern: Veritabanı işlemlerini soyutlayan arabirim.
// "T" bir Entity olmalıdır (BaseEntity'den türemelidir).
// Bu sayede her tablo için ayrı ayrı "GetProjectById", "GetTodoById" yazmak yerine
// tek bir "GetById<Project>" yapısı kuruyoruz.
public interface IRepository<T> where T : BaseEntity
{
    // Id'ye göre kayıt getirir.
    // Task: Asenkron (Async) işlem olduğunu belirtir.
    // T?: Kayıt bulunamazsa null dönebilir.
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    // Tüm kayıtları getirir.
    Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);

    // Yeni kayıt ekler.
    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    // Kaydı günceller.
    // Not: EF Core'da Update işlemi genellikle senkrondur ama biz Task döndürüyoruz.
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

    // Kaydı siler.
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
}
