using App.Service;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace App.Services.ExceptionHandler;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        //response artık bu katmanda döndürmemiz lazım.

        #region ServiseKatmanınaHatayıDönme
        var errorAsDto = ServiceResult.Fail(exception.Message ,HttpStatusCode.InternalServerError);
        #endregion

        #region HttpContextResponseNotu
        /*
         * hatanın statuskodu
         * hatanın content type'ı
         * ve hatanın içeriği yazılmalı.
         */
        #endregion


        #region HttpContextResponse
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsJsonAsync(errorAsDto, cancellationToken:cancellationToken);
        #endregion


        return true;
    }
}
