using App.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace App.Services;

public class FluentValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {

        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values.SelectMany(i => i.Errors).Select(i => i.ErrorMessage).ToList();

            //valitdaiton hataları varsa ServiceResult ile döner
            var resultModel = ServiceResult.Fail(errors);

            context.Result = new BadRequestObjectResult(resultModel);

            return;

        }

        //hata yok ise next ile devam eder
        await next();
    }
}
