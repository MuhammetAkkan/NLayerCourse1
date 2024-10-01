using System.Net;
using System.Text.Json.Serialization;

namespace App.Service;

public class ServiceResult<T>
{
    public T? Data { get; set; } //Başarılı olduğunda T tipinde bir data döndürülecek.

    public List<T>? Datas { get; set; } //Başarılı olduğunda T tipinde bir data serisi döndürülecek
    public List<string>? ErrorMessage { get; set; } //Başarısız olma durumunda hataları tutacak.

    [JsonIgnore]
    public bool IsSuccess => ErrorMessage == null || ErrorMessage.Count == 0; //Başarılı olma durumunu kontrol eder.

    [JsonIgnore]
    public bool IsFail => !IsSuccess; //Başarısız olma durumunu kontrol eder.

    [JsonIgnore]
    public HttpStatusCode Status { get; set; }


    [JsonIgnore]
    public string? UrlAsCreated { get; set; }

    //statik factory methodlar
    public static ServiceResult<T> Success(T data, HttpStatusCode statusCode = HttpStatusCode.OK)
    { //default bir http status code belirledik.
        return new ServiceResult<T>
        {
            Data = data, //nesne döndürülecek yani data döndürülecek.
            Status = statusCode,
        };
    }


    public static ServiceResult<T> SuccessAsCreated(T data, string url)
    { //default bir http status code belirledik.
        return new ServiceResult<T>
        {
            Data = data, //nesne döndürülecek yani data döndürülecek.
            Status = HttpStatusCode.Created,
            UrlAsCreated = url
        };
    }


    public static ServiceResult<T> Success(List<T> datas, HttpStatusCode statusCode = HttpStatusCode.OK)
    { //default bir http status code belirledik.
        return new ServiceResult<T>
        {
            Datas = datas,
            Status = statusCode,
        };
    }


    public static ServiceResult<T> Fail(List<string> errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new ServiceResult<T>
        {
            ErrorMessage = errorMessage,
            Status = statusCode
        };
    }

    public static ServiceResult<T> Fail(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new ServiceResult<T>
        {
            ErrorMessage = new List<string> { errorMessage },
            Status = statusCode
        };
    }


}

public class ServiceResult
{
    
    public List<string>? ErrorMessage { get; set; } //Başarısız olma durumunda hataları tutacak.

    [JsonIgnore]
    public bool IsSuccess => ErrorMessage == null || ErrorMessage.Count == 0; //Başarılı olma durumunu kontrol eder.

    [JsonIgnore]
    public bool IsFail => !IsSuccess; //Başarısız olma durumunu kontrol eder.

    [JsonIgnore]
    public HttpStatusCode Status { get; set; }


    [JsonIgnore]
    public string? UrlAsCreated { get; set; }

    //statik factory methodlar
    public static ServiceResult Success(HttpStatusCode statusCode = HttpStatusCode.OK)
    { //default bir http status code belirledik.
        return new ServiceResult
        {
            Status = statusCode,
        };
    }


    public static ServiceResult Fail(List<string> errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new ServiceResult
        {
            ErrorMessage = errorMessage,
            Status = statusCode
        };
    }

    public static ServiceResult Fail(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new ServiceResult
        {
            ErrorMessage = new List<string> { errorMessage },
            Status = statusCode
        };
    }


}