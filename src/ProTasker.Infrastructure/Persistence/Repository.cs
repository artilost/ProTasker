using Microsoft.EntityFrameworkCore;
using ProTasker.Domain.Common;
using ProTasker.Domain.Interfaces;

namespace ProTasker.Infrastructure.Persistence;

// IRepository arayüzünün somut (concrete) hali.
// Veritabanı işlemleri için Entity Framework Core'un "DbContext" sınıfını kullanır.
public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        // DbContext içinden ilgili tabloyu (DbSet) seçiyoruz.
        // T=Project ise Projects tablosunu, T=TodoItem ise TodoItems tablosunu getirir.
        _dbSet = context.Set<T>();
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        // Veritabanına "Eklenecek" olarak işaretler. Henüz SQL çalışmaz.
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        // Veritabanına "Silinecek" olarak işaretler.
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        // Tüm kayıtları liste olarak çeker.
        // ToListAsync() dediğimiz anda SQL sorgusu (SELECT * FROM ...) çalışır.
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // Primary Key'e göre arama yapar.
        return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
    }

    public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        // Veritabanına "Güncellenecek" olarak işaretler.
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }
}
