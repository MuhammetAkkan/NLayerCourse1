
using Microsoft.EntityFrameworkCore;

namespace App.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{

    private readonly AppDbContext _context = context;


    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync(); //tek satır olduğunda return kullanmaya gerek yoktur.


}
