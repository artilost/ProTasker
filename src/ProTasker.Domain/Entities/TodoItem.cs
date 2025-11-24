using ProTasker.Domain.Common;
using ProTasker.Domain.Enums;

namespace ProTasker.Domain.Entities;

// Görev (Task) nesnesi.
public class TodoItem : AuditableEntity
{
    public string Title { get; private set; }
    public string? Note { get; private set; }
    public PriorityLevel Priority { get; private set; }
    public DateTimeOffset? Reminder { get; private set; }
    
    // Görevin yapılıp yapılmadığı.
    // private set olduğu için dışarıdan "task.IsDone = true" denilemez.
    public bool IsDone { get; private set; }

    // Hangi projeye ait olduğunu tutan Foreign Key (Yabancı Anahtar).
    public Guid ProjectId { get; private set; }
    
    // Navigation Property: Kod içinde "task.Project.Name" diyebilmek için.
    public Project Project { get; private set; } = null!;

    // EF Core için boş constructor.
    private TodoItem() { }

    // Yeni görev oluşturucu.
    public TodoItem(string title, PriorityLevel priority, Guid projectId)
    {
        Title = title;
        Priority = priority;
        ProjectId = projectId;
        IsDone = false; // Yeni görev varsayılan olarak yapılmamıştır.
    }

    // Rich Domain Model Örneği:
    // Sadece "IsDone = true" demiyoruz, "Görevi Tamamla" eylemini kodluyoruz.
    public void MarkAsDone()
    {
        // Eğer zaten bitmişse işlem yapma (Idempotency).
        if (IsDone) return;

        IsDone = true;
        
        // İleride buraya "Görevi tamamlayana puan ver" gibi eventler ekleyeceğiz.
        // AddDomainEvent(new TodoItemCompletedEvent(this));
    }

    // Görevi geri alma (tamamlanmadı yapma).
    public void MarkAsUndone()
    {
        IsDone = false;
    }

    // Öncelik değiştirme.
    public void UpdatePriority(PriorityLevel priority)
    {
        Priority = priority;
    }

    // Başlık güncelleme.
    public void UpdateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.");

        Title = title;
    }

    // Hatırlatıcı ayarlama.
    public void SetReminder(DateTimeOffset? reminder)
    {
        Reminder = reminder;
    }
}
