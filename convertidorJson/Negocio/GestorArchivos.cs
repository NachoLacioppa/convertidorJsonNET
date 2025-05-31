using Dominio.Carpeta;
using Dominio.EstructuraJson;
using Dominio.DatosEntrada;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Negocio
{
    public class GestorArchivos
    {
        private Carpeta Carpeta;
        private List<string> objetosTransformados = new List<string>();

        public GestorArchivos(Carpeta carpeta)
        {
            this.Carpeta = carpeta;
        }

        public void LeerContenidoArchivo()
        {
            if (!Directory.Exists(Carpeta.Ruta))
            {
                Console.Clear();
                Console.WriteLine("ERROR - GestorArchivos - LeerContenidoArchivo - Carpeta no existe");
                return;
            }

            DatosEntrada datosEntrada = EntradaUsuario.Pedir();
            string[] archivos = Directory.GetFiles(Carpeta.Ruta);
            if (archivos.Length > 0)
            {
                foreach (string archivo in archivos)
                {
                    try
                    {
                        string contenido = File.ReadAllText(archivo).Trim();

                        bool esArrayJson = contenido.StartsWith("[") && contenido.EndsWith("]");
                        string contenidoArray = esArrayJson ? contenido : $"[{contenido}]";

                        List<Factura> facturas = JsonSerializer.Deserialize<List<Factura>>(contenidoArray);

                        if (facturas != null && facturas.Count > 0)
                        {
                            foreach (var datos in facturas)
                            {
                                TransformarContenidoArchivo(datos, datosEntrada.Banco, datosEntrada.Cantidad);
                            }
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("WARNING - No se encontraron facturas válidas en el archivo.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Clear();
                        Console.WriteLine($"ERROR - GestorArchivos - LeerContenidoArchivo - {ex.Message}");
                    }
                }
                CrearArchivoSalidaFinal();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("WARNING! - GestorArchivos - LeerContenidoArchivo - No hay archivos en la carpeta IN");
            }
        }

        public void TransformarContenidoArchivo(Factura aux, string bco, int cant)
        {
            string contenidoFormateado = $@"{{banco:'{bco}',cantidad:{cant},factura:{{idFactura:{{cuitEmisor:'{aux.idFactura.cuitEmisor}'}},cuitComprador:'{aux.cuitComprador}',cbuComprador:'{aux.cbuComprador}',cbuEmisor:'{aux.idFactura.cbuEmisor}',fechaVencimientoPago:'{aux.fechaVencimientoPago}',saldoAceptado:{aux.saldoAceptado}}}}}";
            objetosTransformados.Add(contenidoFormateado);
        }

        public void AbrirArchivo(string rutaArchivo)
        {
            try
            {
                if (File.Exists(rutaArchivo))
                {
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = rutaArchivo,
                        UseShellExecute = true
                    };

                    Process.Start(psi);
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine($"WARNING! - GestorArchivos - AbrirArchivo - No se pudo abrir el archivo: no existe la ruta {rutaArchivo}");
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine($"ERROR - GestorArchivos - AbrirArchivo - Error al intentar abrir el archivo: {ex.Message}");
            }
        }

        public void CrearArchivoSalidaFinal()
        {
            if (objetosTransformados.Count == 0)
            {
                return;
            }

            string salida = "[\n" + string.Join(",\n", objetosTransformados) + "\n]";
            string outputPath = AppConfig.OutputFolder;

            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            string fecha = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string archivoSalida = Path.Combine(outputPath, $"resultado_{fecha}.txt");

            File.WriteAllText(archivoSalida, salida);
            //Console.WriteLine($"Archivo de salida generado con {objetosTransformados.Count} objetos: {archivoSalida}");
            MostrarResultadoFinal(archivoSalida, objetosTransformados.Count);

            //Console.WriteLine("\n\n\nPresione cualquier tecla para abrir el archivo. . .");
            //Console.ReadKey();

            Console.ForegroundColor = ConsoleColor.Magenta;

            string message = "Presione cualquier tecla para abrir el archivo...";
            int width = Console.WindowWidth;
            int padding = (width - message.Length) / 2;

            Console.WriteLine("\n\n");
            Console.WriteLine(new string(' ', padding) + message);
            Console.ResetColor();

            Console.ReadKey();

            AbrirArchivo(archivoSalida);

        }

        public void MostrarResultadoFinal(string rutaArchivo, int cantidadObjetos)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;

            // Construimos los textos
            string titulo = "ARCHIVO GENERADO";
            string linea1 = $"[OK] Archivo generado con {cantidadObjetos} objeto{(cantidadObjetos > 1 ? "s" : "")}";
            string linea2 = "[>>] Ruta:";
            string linea3 = $"  {rutaArchivo}";

            // Determinar el ancho del cuadro según la línea más larga
            int anchoTotal = Math.Max(Math.Max(titulo.Length, linea1.Length), linea3.Length) + 6;

            string lineaBorde = new string('═', anchoTotal);

            // Mostrar cuadro
            Console.WriteLine($"╔{lineaBorde}╗");
            Console.WriteLine($"║{titulo.PadLeft((anchoTotal + titulo.Length) / 2).PadRight(anchoTotal)}║");
            Console.WriteLine($"╠{lineaBorde}╣");
            Console.WriteLine($"║  {linea1.PadRight(anchoTotal - 2)}║");
            Console.WriteLine($"║  {linea2.PadRight(anchoTotal - 2)}║");
            Console.WriteLine($"║  {linea3.PadRight(anchoTotal - 2)}║");
            Console.WriteLine($"╚{lineaBorde}╝");

            Console.ResetColor();
            Console.WriteLine();
        }
    }
}


