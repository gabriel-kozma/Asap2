using System;
using Asap2;
namespace Asap2Parser
{
    public class XdfCreator
    {
        private Asap2File a2l;
        private PROJECT project;
        private MODULE MODULE;

        public XdfCreator(Asap2File file)
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
                    Console.WriteLine(measurement);
                }
                else if (valueOfType == typeof(CHARACTERISTIC))
                {
                    var characteristic = (CHARACTERISTIC)value;
                    Console.WriteLine(characteristic);
                }
                else if (valueOfType == typeof(Asap2.AXIS_PTS))
                {
                    var axisPts = (AXIS_PTS)value;
                    Console.WriteLine(axisPts);
                }
            }
        }
    }
}

