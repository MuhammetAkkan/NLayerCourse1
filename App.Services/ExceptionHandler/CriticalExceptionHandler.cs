using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Services.ExceptionHandler;

public class CriticalExceptionHandler() : IExceptionHandler
{
    #region Notlar
    /*
     * geriye bir dto genellikle dönülmüyor, başka bir servis çalıştırılabilir, örnek vermek gerekirse bir mail servisi çalıştırılabilir.
     * TryHandleAsync metodu async olduğu için ValueTask döndürüyor. Eğer ki true dönersek hatayı ele aldığımızı belirtiyoruz, ama false ise başka bir handler çalıştırılabilir.
     */
    #endregion
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        //business logic

        if(exception is CriticalException)
        {
            Console.WriteLine("Hata hata hata!!!!"); //bu
        }

        return ValueTask.FromResult(false); //bir sonraki handler çalışsın => GlobalExceptionHandler
    }
}
