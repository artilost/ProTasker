using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProTasker.Infrastructure.Persistence;
using ProTasker.Domain.Interfaces;

namespace ProTasker.Infrastructure;

// Dependency Injection (DI) Konfigürasyonu.
// "Program.cs" dosyasını kirletmemek için, Infrastructure katmanına ait servisleri burada kaydediyoruz.
// "Extension Method" (this IServiceCollection services) kullanarak yapıyoruz.
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // DbContext'i servislere ekliyoruz.
        // UseSqlServer: SQL Server kullanacağımızı belirtiyoruz.
        // ConnectionString: appsettings.json dosyasından "DefaultConnection" adıyla okunur.
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                // MigrationsAssembly: Migration dosyalarının nerede tutulacağını belirtir.
                // Infrastructure projesinde tutulmasını istiyoruz.
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        // Repository ve UnitOfWork servislerini ekliyoruz.
        // Scoped: Her HTTP isteği (Request) için yeni bir tane oluşturulur.
        // Neden Scoped? Çünkü DbContext de Scoped'dır. Bir istek boyunca aynı DbContext'i kullanmak isteriz.
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
