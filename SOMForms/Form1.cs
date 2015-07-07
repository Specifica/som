using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SOMForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string PathPatrones;
        string PathPesos;
        Som som = new Som();
        List<List<Tuple<Double, Double>>> HistorialActualizacion;
        List<List<Double>> HistorialDistancias;
        List<Tuple<Double, Double>> Pesos;
        List<Tuple<Double, Double>> Patrones;

        private string AbrirArchivo()
        {
            OpenFileDialog OFD = new OpenFileDialog();
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                return OFD.FileName;
            }
            else{
                return "";
            }
        }

        private void btnImportPatterns_Click(object sender, EventArgs e)
        {
            PathPatrones = this.AbrirArchivo();
            try
            {
                Patrones = CsvInterface.ReadCsvFile(PathPatrones);
                Console.WriteLine("Archivo {0} importado como Patrones de entrada", PathPatrones);
                txtInforme.AppendText(Environment.NewLine + "Archivo " + PathPatrones + " importado como Patrones de entrada");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }

        private void btnImportWeights_Click(object sender, EventArgs e)
        {

            PathPesos = this.AbrirArchivo();
            try
            {
                Pesos = CsvInterface.ReadCsvFile(PathPesos);
                Console.WriteLine("Archivo {0} importado como Pesos Iniciales de las neuronas", PathPesos);
                txtInforme.AppendText(Environment.NewLine + "Archivo " + PathPesos + " importado como Pesos Iniciales de las neuronas");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            try
            {
                if (PathPesos == null || PathPesos == "")
                {
                    txtInforme.ResetText();
                    //Calcular con sin pesos iniciales
                    Console.WriteLine("true");
                    HistorialActualizacion = som.SOMAlgorithm(Patrones,Convert.ToInt16(counterNeuronas.Value));
                    HistorialDistancias = som.HistorialDistancias;
                }
                else
                {
                    //Calcular con los pesos iniciales
                    Console.WriteLine("False");
                    HistorialActualizacion = som.SOMAlgorithm(Pesos, Patrones);
                    HistorialDistancias = som.HistorialDistancias;
                }
        
                //Impresion del informe
                txtInforme.ResetText();
                txtInforme.AppendText(Environment.NewLine + "Pesos Parciales:");
                var iteracion = 0;
                foreach (var ConfigPesos in HistorialActualizacion)
                {
                    iteracion = HistorialActualizacion.IndexOf(ConfigPesos);

                    if (true)
                    {
                        var Distancias = HistorialDistancias[iteracion];
                        txtInforme.AppendText(Environment.NewLine + "--------------------------------------------");
                        txtInforme.AppendText(Environment.NewLine + "ITERACION: " + iteracion);
                        txtInforme.AppendText(Environment.NewLine);
                        txtInforme.AppendText(Environment.NewLine + "Distancias:" );
                        txtInforme.AppendText(Environment.NewLine);
                        Distancias.ForEach(delegate(Double dist)
                        {
                            txtInforme.AppendText(Environment.NewLine + dist);
                        });
                    }
                    txtInforme.AppendText(Environment.NewLine);
                    txtInforme.AppendText(Environment.NewLine + "Pesos:");
                    txtInforme.AppendText(Environment.NewLine);
                    for (int i = 0; i < ConfigPesos.Count(); i++)
                    {
                        var peso = ConfigPesos[i];
                        txtInforme.AppendText(Environment.NewLine + "W" + i + " = [" + peso.Item1 + "; " + peso.Item2 + "]");
                    }
                    txtInforme.AppendText(Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtInforme.AppendText(Environment.NewLine + "Pesos borrados.");
            PathPesos = "";
            Pesos = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtInforme.ResetText();
        }
    }
}
