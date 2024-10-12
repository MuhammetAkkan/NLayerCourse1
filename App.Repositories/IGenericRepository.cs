using System.Linq.Expressions;
using System.Security.Cryptography;

namespace App.Repositories;

public interface IGenericRepository<T, in TId> where T : class where TId : struct
{
    //Aşağıdaki yapı geneldir, daha fazla çoğaltılması genellikle mimari için uygun değildir.

    /*
     *Task kullanmamamızın sebebi data katmanı sayılan bu katmanda ToList vb yapmakacak olmamızdır.
     */
    IQueryable<T> GetAll(); //db den gelen veriye bir filtreleme yapacaksak IQueryable kullanılır.

    IQueryable<T> Where(Expression<Func<T, bool>> predicate); //Func<T, bool> predicate => T tipinde bir parametre alacak ve bool dönecek bir metot oluşturduk.
    /*
     * var evenNumbers = numbers.Where(n => n % 2 == 0).ToList(); yapıp foreachte eventNumbers ın içine girersek verileri elde ederiz. => bu şekilde kullanılır.
     */
    ValueTask<T?> GetByIdAsync(int id); //Task yerine valueTask kullanıyoruz çünkü tek bir veri dönecek.
    ValueTask CreateAsync(T entity);
    void Update(T entity); //void kullanıyoruz çünkü bir şey döndürmeyecek.
    void Delete(T entity);   //void kullanıyoruz çünkü bir şey döndürmeyecek.

    Task<bool> AnyAsync(TId id);
}

