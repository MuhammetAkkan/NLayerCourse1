namespace App.Services.ExceptionHandler;

public class CriticalException(string message) : Exception(message); //bir string message döneceğiz ve bu mesajı base sınıfına gönderiyoruz.

