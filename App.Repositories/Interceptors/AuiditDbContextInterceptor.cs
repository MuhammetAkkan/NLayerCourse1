using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace App.Repositories.Interceptors;

public class AuiditDbContextInterceptor : SaveChangesInterceptor
{

    //dictionary çalışma prensibi => key value pair
    private static readonly Dictionary<EntityState, Action<DbContext, IAuditEntity>> Behaviors = new()
    {
        {EntityState.Added, AddBehavior},
        {EntityState.Modified, UpdateBehavior}

    };


    private static void AddBehavior(DbContext context, IAuditEntity auditEntity)
    {
        auditEntity.Created = DateTime.Now;
        context.Entry(auditEntity).Property(x => x.Updated).IsModified = false; 

    }

    private static void UpdateBehavior(DbContext context, IAuditEntity auditEntity)
    {
        auditEntity.Updated = DateTime.Now;
        context.Entry(auditEntity).Property(x => x.Created).IsModified = false;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = new CancellationToken())
    {
        //base entity e ihtiyacımız var.

        foreach (var entry in eventData.Context!.ChangeTracker.Entries().ToList())
        {
            
            if(entry.Entity is not IAuditEntity auditEntity) continue;


            if (entry.State is not EntityState.Added || entry.State is not EntityState.Modified) continue;
            
            Behaviors[entry.State].Invoke(eventData.Context, auditEntity);
        }
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
