using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOMConsole
{
    class Som
    {
        List<List<Tuple<Double, Double>>> HistorialActualizacion { get; set; }
        List<List<Double>> HistorialDistancias { get; set; }
        /// <summary>
        /// Devuelve una Lista de Listas de Tuplas(Doule,Doule) que contiene el historial de configuraciones de los pesos.
        /// El algoritmo tiene como condicion de Fin que se estabilicen los pesos, es decir que termina cuando la anteultima y ultima configuracion son iguales.
        /// </summary>
        /// <param name="Pesos">Lista de tuplas(double,double) que representa la configuracion de pesos iniciales.</param>
        /// <param name="Patrones">Lista de tuplas(double,double) que representa los patrones de entrada.</param>
        public List<List<Tuple<Double, Double>>> SOMAlgorithm(List<Tuple<Double, Double>> Pesos, List<Tuple<Double, Double>> Patrones)
        {
            this.HistorialActualizacion = new List<List<Tuple<Double, Double>>>();
            this.HistorialDistancias = new List<List<Double>>();
            var UltimaConfig = new List<Tuple<Double, Double>>();
            var AnteUltimaConfig = new List<Tuple<Double, Double>>();
            int i = 0;
            
                //foreach (var Patron in Patrones)
                do
                {
                    var Patron = Patrones[i % 5];

                    var IxGanador = FuncionActivacion(Pesos, Patron);
                    Pesos = ActualizarPesos(Pesos, Patron, Patrones.IndexOf(Patron)+1, IxGanador);
                    
                    
                    HistorialActualizacion.Add(Pesos);//Se guardan las configuraciones de pesos parciales. La ultima será la final.
                    
                    //Print pesos
                    Console.WriteLine();
                    Console.WriteLine("Nuevos Pesos:");
                    Pesos.ForEach(delegate(Tuple<Double, Double> peso) { Console.WriteLine(peso.Item1 + "; " + peso.Item2); });
                    Console.WriteLine();

                    i++;
                    UltimaConfig = HistorialActualizacion.Last();

                    if (i>=2)
                    {
                        AnteUltimaConfig = HistorialActualizacion.ElementAt(HistorialActualizacion.IndexOf(UltimaConfig)-1) ;
                    }

                } while (!UltimaConfig.SequenceEqual(AnteUltimaConfig));
            return HistorialActualizacion;
        }
        /// <summary>
        /// Devuelve una Lista de Listas de Tuplas(Doule,Doule) que contiene el historial de configuraciones de los pesos.
        /// El algoritmo tiene como condicion de Fin que se estabilicen los pesos, es decir que termina cuando la anteultima y ultima configuracion son iguales.
        /// La eleccion de los pesos Iniciales se realiza de forma aleatoria.
        /// </summary>
        /// <param name="Patrones">Lista de tuplas(double,double) que representa los patrones de entrada.</param>
        /// <param name="CantSalidas">Representa la cantidad de neuronas de salidas.</param>
        public List<List<Tuple<Double, Double>>> SOMAlgorithm(List<Tuple<Double, Double>> Patrones, int CantSalidas)
        {

            List<Tuple<Double, Double>> Pesos = new List<Tuple<Double, Double>>();
            Random rnd = new Random();
            for (int j = 0; j < CantSalidas; j++)
            {
                
                int signoX1 = rnd.NextDouble() < 0.5 ? -1 : 1;
                var pesoX1 = rnd.NextDouble()*signoX1;
                int signoX2 = rnd.NextDouble() < 0.5 ? -1 : 1;
                var pesoX2 = rnd.NextDouble()*signoX2;

                Pesos.Add(Tuple.Create(pesoX1, pesoX2));
            }

            this.HistorialActualizacion = new List<List<Tuple<Double, Double>>>();
            this.HistorialDistancias = new List<List<Double>>();
            var UltimaConfig = new List<Tuple<Double, Double>>();
            var AnteUltimaConfig = new List<Tuple<Double, Double>>();
            int i = 0;
            
            //foreach (var Patron in Patrones)
            do
            {
                var Patron = Patrones[i % 5];
                var IxGanador = FuncionActivacion(Pesos, Patron);
                Pesos = ActualizarPesos(Pesos, Patron, Patrones.IndexOf(Patron)+1, IxGanador);
                HistorialActualizacion.Add(Pesos);//Se guardan las configuraciones de pesos parciales. La ultima será la final.

                //Print pesos
                Console.WriteLine();
                Console.WriteLine("Nuevos Pesos:");
                Pesos.ForEach(delegate(Tuple<Double, Double> peso) { Console.WriteLine(peso.Item1 + "; " + peso.Item2); });
                Console.WriteLine();

                i++;
                UltimaConfig = HistorialActualizacion.Last();

                if (i >= 2)
                {
                    AnteUltimaConfig = HistorialActualizacion.ElementAt(HistorialActualizacion.IndexOf(UltimaConfig) - 1);
                }

            } while (!UltimaConfig.SequenceEqual(AnteUltimaConfig));
            return HistorialActualizacion;
        }

        private int FuncionActivacion(List<Tuple<Double, Double>> VectoresDePeso, Tuple<Double, Double> Instancia)
        {
            List<Double> Distancias = CalcularDistancias(VectoresDePeso, Instancia);
            this.HistorialDistancias.Add(Distancias);
            var Result = Distancias.IndexOf(Distancias.Min());
            return Result;
        }

        private static List<Double> CalcularDistancias(List<Tuple<Double, Double>> VectoresDePeso, Tuple<Double, Double> Instancia)
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
        private static List<Tuple<Double, Double>> ActualizarPesos(List<Tuple<Double, Double>> VectoresDePeso,Tuple<Double, Double> Instancia,int t,int IndiceGanador)
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
        
    }
}
