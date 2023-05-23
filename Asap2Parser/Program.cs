using Asap2;

namespace Asap2Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            var parser = new Parser("/Users/gabrielkozma/Library/CloudStorage/Dropbox/Car Files/Ecu Tuning/MG1 Ols/A2L/DME861_R1C2900AB/R1C2900AB.a2l", new Asap2ErrorReporter());
            var a2lFile = parser.DoParse();
        }
    }

    class Asap2ErrorReporter : IErrorReporter
    {
        public void reportError(string message)
        {
            Console.WriteLine(message);
        }

        public void reportInformation(string message)
        {
            Console.WriteLine(message);
        }

        public void reportWarning(string message)
        {
            Console.WriteLine(message);
        }
    }
}


