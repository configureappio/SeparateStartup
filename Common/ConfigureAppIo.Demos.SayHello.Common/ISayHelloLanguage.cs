namespace ConfigureAppIo.Demos.SayHello.Common
{
    public interface ISayHelloLanguage : ISayHello
    {
        string Language { get; }
    }
}