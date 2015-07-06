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
            var Instancias = CsvInterface.ReadCsvFile(@"C:\Users\Fer500\Dropbox\problema1.csv");

            foreach (var inst in Instancias)
            {
                Console.WriteLine(inst.Item1 + "; " + inst.Item2);
            }
            //W1 = [0.5, -0.3]; W2 = [-0.5, 0.8]; W3 = [-0.9, -0.7]; W4 = [-0.2, -0.8]; W5 = [-0.1, -0.1];

            var VectoresDePeso = new List<Tuple<Double, Double>>();

            VectoresDePeso = CsvInterface.ReadCsvFile(@"C:\Users\Fer500\Dropbox\PesosProblema1.csv");

            List<int> NeuronasActivadas = new List<int>();

            foreach (var Instancia in Instancias)
            {
                Console.WriteLine("Distancias contra instancia: " + Instancias.IndexOf(Instancia));
                var IndiceGanador = FuncionActivacion(VectoresDePeso, Instancia);
                NeuronasActivadas.Add(IndiceGanador);

                Console.WriteLine();

                Console.WriteLine("Nuevos Pesos:");
                VectoresDePeso = ActualizarPesos(VectoresDePeso, Instancia, Instancias.IndexOf(Instancia) + 1, IndiceGanador);
                foreach (var item in VectoresDePeso)
                {
                    Console.WriteLine(item.Item1 + ";" + item.Item2);
                }
            }
            Console.WriteLine("Neuronas activadas");
            foreach (var neurona in NeuronasActivadas)
            {
                Console.WriteLine(neurona);
            }


            Console.WriteLine("Nuevos Pesos:");

            Console.ReadKey();

        }


        private static int FuncionActivacion(List<Tuple<Double,Double>> VectoresDePeso, Tuple<Double,Double> Instancia)
        {
            List<Double> Distancias = new List<Double>();
            
            foreach (var Vector in VectoresDePeso)
            {
                var Sumatoria = Math.Pow(Vector.Item1 - Instancia.Item1, 2) + Math.Pow(Vector.Item2 - Instancia.Item2, 2);
                var DistanciaEuclidea = Math.Sqrt(Sumatoria);
                Console.WriteLine("Distancia Euclidea " + VectoresDePeso.IndexOf(Vector) + ":" + DistanciaEuclidea);
                Distancias.Add(DistanciaEuclidea);
            }
            
            var Result = Distancias.IndexOf(Distancias.Min());
            return Result;
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

        /*
        private static double FuncionGaussiana(Double distancia, List<Double> Columna, int Indice)
        {
            //Calculo de varianza
            var sum = 0.00;
            foreach (var x in Columna)
            {
                sum += Math.Pow(x -  ,2);
            }
            var Varianza = sum / Columna.Count();

            return Math.Exp(-Math.Pow(distancia, 2) / (2 * Math.Pow(Varianza, 2)));
        }*/
    }
}
