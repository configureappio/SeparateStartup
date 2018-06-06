using ConfigureAppIo.Demos.SayHello.Common;

namespace ConfigureAppIo.Demos.SayHello.EnglishInformal
{
    public class EnglishInformalHello : IEnglishInformalHello
    {
        public string GetHello() => "Hi!";
    }

    public interface IEnglishInformalHello : ISayHello
    {
    }
}
