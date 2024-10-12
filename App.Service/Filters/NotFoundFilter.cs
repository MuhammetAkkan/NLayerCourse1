using App.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace App.Service.Filters;

//attribute ile aciton a yetkilendirme gibi özelleştirmelere izin verir.
public class NotFoundFilter<T, TId>(IGenericRepository<T, TId> genericRepository) : Attribute, IAsyncActionFilter where T : class where TId : struct
{



    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {

        var idValue = context.ActionArguments.TryGetValue("id", out var idAsObject) ? idAsObject : null;
        

        if(idAsObject is not TId id)
        {
            await next();
            return;
        }

        //entity db de var mı?
        var anyEntity = await genericRepository.AnyAsync(id);

        if (anyEntity)
        {
            await next();
            return;
        }

        var entityName = typeof(T).Name; //T nin adını alır.

        //action ın adını alırız
        var actionName = context.ActionDescriptor.DisplayName;


        var result = ServiceResult.Fail($"data bulunmamıştır({entityName}, ({actionName}))");

        context.Result = new NotFoundObjectResult(result);



        //action metot çalıştıktan sonra çalışacak kodlar buraya yazılır.
    }
}