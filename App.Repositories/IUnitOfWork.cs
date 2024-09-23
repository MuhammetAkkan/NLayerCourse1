namespace App.Repositories;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(); //değişiklikleri kaydetmek için, int ise kaç satır etkilendiğini dönecek.
}

