using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace SOMForms
{
    class CsvInterface
    {
        public static List<Tuple<Double, Double>> ReadCsvFile(String filePath) 
        {
            //Para indicar que los decimales estan separados or punto y no coma
            var CI = new CultureInfo("en-AU");

            var reader = new StreamReader(File.OpenRead(filePath));
            List<Tuple<Double, Double>> Instancias = new List<Tuple<Double, Double>>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(';');
                Tuple<Double, Double> Instancia = Tuple.Create(Convert.ToDouble(values[0],CI), Convert.ToDouble(values[1],CI));
                Instancias.Add(Instancia);
            }

            return Instancias;
        }
    }
}
