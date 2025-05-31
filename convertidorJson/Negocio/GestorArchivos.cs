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
                            Console.WriteLine("WARNING - No se encontraron facturas válidas en el archivo.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ERROR - GestorArchivos - LeerContenidoArchivo - {ex.Message}");
                    }
                }
                CrearArchivoSalidaFinal();
            }
            else
            {
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
                    Console.WriteLine($"WARNING! - GestorArchivos - AbrirArchivo - No se pudo abrir el archivo: no existe la ruta {rutaArchivo}");
                }
            }
            catch (Exception ex)
            {
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
            Console.WriteLine($"Archivo de salida generado con {objetosTransformados.Count} objetos: {archivoSalida}");

            AbrirArchivo(archivoSalida);
        }
    }
}


