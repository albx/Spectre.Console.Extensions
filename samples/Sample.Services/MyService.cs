namespace Sample.Services;

public interface IMyService
{
    string SayHello();
}

public class MyService : IMyService
{
    public string SayHello() => "Hello world";
}
