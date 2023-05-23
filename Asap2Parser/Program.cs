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
            var xdfParser = new XdfParser(a2lFile);
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

    public class XdfParser
    {
        private Asap2File a2l;
        private PROJECT project;
        private MODULE MODULE;

        public XdfParser(Asap2File file)
        {
            this.a2l = file;
            this.project = GetProject(file);

            if (this.project == null)
            {
                throw new Exception("PROJECT not found in Asap2File");
            }
        }

        private static PROJECT GetProject(Asap2File file)
        {
            foreach (var element in file.elements)
            {
                if (element.GetType() == typeof(PROJECT))
                {
                    return (PROJECT)element;
                }
            }
            return null;
        }

        private static MODULE GetModule(PROJECT project)
        {
            foreach (var element in project.modules)
            {
                if (element.Value.GetType() == typeof(MODULE))
                {
                    return element.Value;
                }
            }
            return null;
        }

        public void CreateXdf(string savePath)
        {
            var module = GetModule(project);
            foreach (var element in module.AxisPtsCharacteristicMeasurement)
            {
                var value = element.Value;
                var valueOfType = value.GetType();
                if (valueOfType == typeof(MEASUREMENT))
                {
                    var measurement = (MEASUREMENT)value;
                    Console.WriteLine($"\n\n=== MEASUREMENT" +
                    $"\nName {measurement.Name}" +
                    $"\nAddress 0x{measurement.GetEcuAddress().ToString("X8")}" +
                    $"\nLongIdentifier {measurement.LongIdentifier}" +
                    $"\nDatatype {measurement.Datatype}" +
                    $"\nConversion {measurement.Conversion}" +
                    $"\nResolution {measurement.Resolution}" +
                    $"\nAccuracy {measurement.Accuracy}" +
                    $"\nLowerLimit {measurement.LowerLimit}" +
                    $"\nUpperLimit {measurement.UpperLimit}" +
                    $"\n===");
                }
                else if (valueOfType == typeof(CHARACTERISTIC))
                {
                    var characteristic = (CHARACTERISTIC)value;
                    Console.WriteLine($"\n\n=== CHARACTERISTIC " +
                    $"\nName {characteristic.Name}" +
                    $"\nAddress 0x{characteristic.Address.ToString("X8")}" +
                    $"\nLongIdentifier {characteristic.LongIdentifier}" +
                    $"\ntype {characteristic.type}" +
                    $"\nDeposit {characteristic.Deposit}" +
                    $"\nMaxDiff {characteristic.MaxDiff}" +
                    $"\nConversion {characteristic.Conversion}" +
                    $"\nLowerLimit {characteristic.LowerLimit}" +
                    $"\nUpperLimit {characteristic.UpperLimit}" +
                    $"\n===");
                }
                else if (valueOfType == typeof(Asap2.AXIS_PTS))
                {
                    var axisPts = (AXIS_PTS)value;
                    PrintAxisPts(axisPts);
                }
            }
        }

        private void PrintAxisPts(AXIS_PTS axisPts)
        {
            /*
                public string Name { get; private set; }
                public string LongIdentifier { get; private set; }
                public UInt64 Address { get; private set; }
                public string InputQuantity { get; private set; }
                public string Deposit { get; private set; }
                public decimal MaxDiff { get; private set; }

                public string Conversion { get; private set; }
                public UInt64 MaxAxisPoints { get; private set; }
                public decimal LowerLimit { get; private set; }
                public decimal UpperLimit { get; private set; }

                public List<ANNOTATION> annotation = new List<ANNOTATION>();

                public BYTE_ORDER byte_order;

                public CALIBRATION_ACCESS calibration_access;

                public DEPOSIT deposit;

                public string display_identifier;

                public ECU_ADDRESS_EXTENSION ecu_address_extension;

                public EXTENDED_LIMITS extended_limits;

                public string format;

                public FUNCTION_LIST function_list;

                public GUARD_RAILS guard_rails;

                public List<IF_DATA> if_data = new List<IF_DATA>();

                public MONOTONY monotony;

                public string phys_unit;

                public READ_ONLY read_only;

                public string ref_memory_segment;

                public decimal? step_size;

                public SYMBOL_LINK symbol_link;
             */
            Console.WriteLine($"\n\n=== AXIS_PTS " +
                    $"\nName {axisPts.Name}" +
                    $"\nAddress 0x{axisPts.Address.ToString("X8")}" +
                    $"\nLongIdentifier {axisPts.LongIdentifier}" +
                    $"\nDeposit {axisPts.Deposit}" +
                    $"\nMaxDiff {axisPts.MaxDiff}" +
                    $"\nConversion {axisPts.Conversion}" +
                    $"\nConversion {axisPts.MaxAxisPoints}" + 
                    $"\nLowerLimit {axisPts.LowerLimit}" +
                    $"\nUpperLimit {axisPts.UpperLimit}" +
                    $"\n===");
        }

    }
}
