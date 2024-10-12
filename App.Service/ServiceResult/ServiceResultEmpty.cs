using System.Net;

public class ServiceResultEmpty
{
    /*
    public List<string>? ErrorMessage { get; set; } //Başarısız olma durumunda hataları tutacak.

    public bool IsSuccess => ErrorMessage == null || ErrorMessage.Count == 0; //Başarılı olma durumunu kontrol eder.

    public bool IsFail => !IsSuccess; //Başarısız olma durumunu kontrol eder.

    public HttpStatusCode Status { get; set; }




    //statik factory methodlar
    public static ServiceResultEmpty Success(HttpStatusCode statusCode = HttpStatusCode.OK)
    { //default bir http status code belirledik.
        return new ServiceResultEmpty
        {
            Status = statusCode,
        };
    }


    public static ServiceResultEmpty Fail(List<string> errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new ServiceResultEmpty
        {
            ErrorMessage = errorMessage,
            Status = statusCode
        };
    }

    public static ServiceResultEmpty Fail(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new ServiceResultEmpty
        {
            ErrorMessage = [errorMessage],
            Status = statusCode
        };
    }
    */


}
