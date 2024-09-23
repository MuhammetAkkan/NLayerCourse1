using System.Net;

namespace App.Services;

public class ServiceResult<T>
{
    public T? Data { get; set; } //Başarılı olduğunda T tipinde bir data döndürülecek
    public List<string>? ErrorMessage { get; set; } //Başarısız olma durumunda hataları tutacak.

    public bool IsSuccess => ErrorMessage == null || ErrorMessage.Count == 0; //Başarılı olma durumunu kontrol eder.

    public bool IsFail => !IsSuccess; //Başarısız olma durumunu kontrol eder.

    public HttpStatusCode StatusCode { get; set; }



    //statik factory methodlar
    public static ServiceResult<T> Success(T data, HttpStatusCode statusCode = HttpStatusCode.OK)
    { //default bir http status code belirledik.
        return new ServiceResult<T>
        {
            Data = data,
            StatusCode = statusCode,
        };
    }


    public static ServiceResult<T> Fail(List<string> errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new ServiceResult<T>
        {
            ErrorMessage = errorMessage,
            StatusCode = statusCode
        };
    }

    public static ServiceResult<T> Fail(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new ServiceResult<T>
        {
            ErrorMessage = [errorMessage],
            StatusCode = statusCode
        };
    }

}
