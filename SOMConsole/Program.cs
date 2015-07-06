using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace SOMConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var Instancias = CsvInterface.ReadCsvFile(@"C:\Users\Fer\Dropbox\problema1.csv");
            var Pesos = new List<Tuple<Double, Double>>();

            Pesos = CsvInterface.ReadCsvFile(@"C:\Users\Fer\Dropbox\PesosProblema1.csv");

            Console.ReadKey();

        }


        private static int FuncionActivacion(List<Tuple<Double,Double>> VectoresDePeso, Tuple<Double,Double> Instancia)
        {
            List<Double> Distancias = CalcularDistancias(VectoresDePeso, Instancia);
            var Result = Distancias.IndexOf(Distancias.Min());
            return Result;
        }

        private static List<Double> CalcularDistancias(List<Tuple<Double,Double>> VectoresDePeso, Tuple<Double,Double> Instancia)
        {
            List<Double> Distancias = new List<Double>();
            
            foreach (var Vector in VectoresDePeso)
            {
                var Sumatoria = Math.Pow(Vector.Item1 - Instancia.Item1, 2) + Math.Pow(Vector.Item2 - Instancia.Item2, 2);
                var DistanciaEuclidea = Math.Sqrt(Sumatoria);
                Console.WriteLine("Distancia Euclidea " + VectoresDePeso.IndexOf(Vector) + ":" + DistanciaEuclidea);
                Distancias.Add(DistanciaEuclidea);
            }

            return Distancias;
            
        }

        private static List<Tuple<Double, Double>> ActualizarPesos(
            List<Tuple<Double, Double>> VectoresDePeso, 
            Tuple<Double,Double> Instancia, 
            int t,
            int IndiceGanador)
        {              
            List<Tuple<Double, Double>> NuevosPesos = new List<Tuple<Double, Double>>();
            foreach (var Vector in VectoresDePeso)
            {
                var Indice = VectoresDePeso.IndexOf(Vector);//Para obtener el indice del bucle (n° de iteración)  
                var x1 = Vector.Item1 + 1.00 / t * FuncionEscalon(Indice, IndiceGanador) * (Instancia.Item1 - Vector.Item1);//Actualiza el peso X1
                var x2 = Vector.Item2 + 1.00 / t * FuncionEscalon(Indice, IndiceGanador) * (Instancia.Item2 - Vector.Item2);//Actualiza el peso X2
                NuevosPesos.Add(Tuple.Create(x1, x2));
            }
            return NuevosPesos;
        }


        private static double FuncionEscalon(int IndiceVector, int IndiceGanador)
        {
            return IndiceVector == IndiceGanador ? 1 : 0;
            /*Es lo mismo que hacer
            if (IndiceVector == IndiceGanador) 
            {
                return 1;
            }
            else
            {
                return 0;
            }
            */
        }
        private static List<List<Tuple<Double,Double>>> SOMAlgorithm(List<Tuple<Double,Double>> Pesos, List<Tuple<Double,Double>> Patrones)
        {
            List<List<Tuple<Double,Double>>> HistorialActualizacion = new List<List<Tuple<Double,Double>>>();
            
            foreach (var Patron in Patrones)
	        {
		        var IxGanador = FuncionActivacion(Pesos,Patron);
                Pesos = ActualizarPesos(Pesos,Patron,Patrones.IndexOf(Patron),IxGanador);
                HistorialActualizacion.Add(Pesos);//Se guardan las configuraciones de pesos parciales. La ultima será la final.
	        }
            return HistorialActualizacion;
        }

        private static List<List<Tuple<Double,Double>>> SOMAlgorithm(List<Tuple<Double,Double>> Patrones, int CantSalidas)
        {
            
            List<Tuple<Double,Double>> Pesos = new List<Tuple<Double,Double>>();
            for (var i =0; i<CantSalidas;i++)
            {
                Random rnd = new Random();
                var pesoX1 = rnd.Next() * (Double.MaxValue - Double.MinValue) + Double.MinValue;
                var pesoX2 = rnd.Next() * (Double.MaxValue - Double.MinValue) + Double.MinValue;

                Pesos.Add(Tuple.Create(pesoX1,pesoX2));
            }

            List<List<Tuple<Double,Double>>> HistorialActualizacion = new List<List<Tuple<Double,Double>>>();
            
            foreach (var Patron in Patrones)
	        {
		        var IxGanador = FuncionActivacion(Pesos,Patron);
                Pesos = ActualizarPesos(Pesos,Patron,Patrones.IndexOf(Patron),IxGanador);
                HistorialActualizacion.Add(Pesos);//Se guardan las configuraciones de pesos parciales. La ultima será la final.
	        }
            return HistorialActualizacion;
        }


        /*private static Double CalcularVarianza(List<Double> Distancias)
        {
            var media = Distancias.Sum() / Distancias.Count();
            var sum = 0.00;
            foreach (var dist in Distancias)
            {
                sum += Math.Pow(dist - media, 2);
            }
            return sum / (Distancias.Count() - 1.00);
        }

        private static double FuncionGaussiana(List<Double> Distancias, List<Tuple<Double,Double>> VectoresDePeso)
        {
            var Varianza = CalcularVarianza(Distancias);
            var Distancia = Math.Pow(Patron.item1 - VectoresDePeso.FindIndex(Indice), 2);
            return Math.Exp(-Math.Pow(distancia, 2) / (2 * Math.Pow(Varianza, 2)));*/
        }
    }
}
