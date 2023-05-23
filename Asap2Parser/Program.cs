using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;
using Asap2;
using Microsoft.VisualBasic;

namespace Asap2Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            var a2lPath = "/Users/gabrielkozma/Library/CloudStorage/Dropbox/Car Files/Ecu Tuning/MG1 Ols/A2L/DME861_R1C2900AB/R1C2900AB.a2l";
            var parser = new Parser(a2lPath, new Asap2ErrorReporter());
            var a2lFile = parser.DoParse();
            var xdfParser = new XdfCreator(a2lFile);
            xdfParser.CreateXdf(a2lPath.Replace(".a2l", ".xdf"));
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
