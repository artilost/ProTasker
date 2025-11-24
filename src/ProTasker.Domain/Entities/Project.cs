using ProTasker.Domain.Common;

namespace ProTasker.Domain.Entities;

// Project bir Entity'dir. AuditableEntity'den miras alarak Id ve Audit özelliklerine sahip olur.
public class Project : AuditableEntity
{
    // Özelliklerin 'set' metodunu private yaptık (Encapsulation).
    // Dışarıdan kimse "project.Name = 'X'" diyemez.
    // Değiştirmek isteyen, bizim yazdığımız metodları (UpdateDetails) kullanmak zorundadır.
    public string Name { get; private set; }
    public string Description { get; private set; }

    // Bir projenin birden fazla görevi (TodoItem) olabilir.
    // Bu bir "One-to-Many" (Bire-Çok) ilişkisidir.
    // Listeyi private yaptık, dışarıdan doğrudan .Add() yapılamasın diye.
    private readonly List<TodoItem> _items = new();
    
    // Dışarıya sadece okunabilir (IReadOnlyCollection) bir liste veriyoruz.
    public IReadOnlyCollection<TodoItem> Items => _items.AsReadOnly();

    // Entity Framework Core (EF Core), veritabanından veri çekerken bu boş constructor'ı kullanır.
    // Private olması EF Core için sorun değildir.
    private Project() { }

    // Yeni bir proje oluştururken bu constructor kullanılır.
    // Zorunlu alanları (name) burada isteriz. Böylece isimsiz proje oluşamaz.
    public Project(string name, string description)
    {
        Name = name;
        Description = description;
    }

    // Proje bilgilerini güncellemek için bu metot kullanılır.
    // İş kuralları (Validation) buraya eklenebilir.
    public void UpdateDetails(string name, string description)
    {
        // Örnek İş Kuralı: İsim boş olamaz.
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Project name cannot be empty.");

        Name = name;
        Description = description;
    }

    // Projeye yeni görev ekleme metodu.
    public void AddItem(TodoItem item)
    {
        // Burada gerekirse "Maksimum 20 görev olabilir" gibi kurallar eklenebilir.
        _items.Add(item);
    }
}
