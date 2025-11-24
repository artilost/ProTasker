using ProTasker.Domain.Interfaces;

namespace ProTasker.Infrastructure.Persistence;

// Unit of Work implementasyonu.
// Aslında EF Core'un "DbContext"i zaten kendi içinde bir Unit of Work'tür.
// Biz burada onu kendi arayüzümüzle (IUnitOfWork) sarmalıyoruz.
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Biriken tüm işlemleri (Ekleme, Silme, Güncelleme) tek seferde veritabanına yollar.
        // Transaction (İşlem Bütünlüğü) sağlar.
        // Yani 10 işlemden 1'i hata verirse, hiçbiri yapılmaz (Rollback).
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
