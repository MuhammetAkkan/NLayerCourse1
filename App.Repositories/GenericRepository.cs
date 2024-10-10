using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace App.Repositories
{
    public class GenericRepository<T>(AppDbContext context) : IGenericRepository<T> where T : class
    {
        //ctor oluşturmadık da public class GenericRepository<T> (AppDbContext context) burada contexti alıyoruz.

        private readonly DbSet<T> _dbSet = context.Set<T>(); //dbseti aldık ve T ye eşitledik.


        public async ValueTask CreateAsync(T entity) => await _dbSet.AddAsync(entity);


        public void Delete(T entity) => _dbSet.Remove(entity);


        //AsNoTracking() => verileri takip etmeyecek, sadece verileri getirecek.
        public IQueryable<T> GetAll() => _dbSet.AsQueryable().AsNoTracking(); //AsQueryable() => dbseti sorgulamak için kullanılır.


        public ValueTask<T?> GetByIdAsync(int id) => _dbSet.FindAsync(id);


        public void Update(T entity) => _dbSet.Update(entity);


        //zaten filtrelenmiş bir veri dönecek, veri sadece döneceğinden AsNoTracking() kullanıldı.
        public IQueryable<T> Where(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate).AsNoTracking();

    }
}
