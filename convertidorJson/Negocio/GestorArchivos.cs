using Dominio.Carpeta;
using Dominio.EstructuraJson;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Negocio
{
    public class GestorArchivos
    {
        private Carpeta Carpeta;
        public GestorArchivos(Carpeta carpeta)
        {
            this.Carpeta = carpeta;
        }

        private List<string> objetosTransformados = new List<string>();

        public void LeerContenidoArchivo()
        {
            if (!Directory.Exists(Carpeta.Ruta))
            {
                Console.WriteLine("ERROR - GestorArchivos - LeerContenidoArchivo - Carpeta no existe");
                return;
            }

            string[] archivos = Directory.GetFiles(Carpeta.Ruta);
            if (archivos.Length > 0)
            {
                foreach (string archivo in archivos)
                {
                    //Console.WriteLine($"{archivo}");
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
                                TransformarContenidoArchivo(datos);
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
            }
            else
            {
                Console.WriteLine("WARNING! - GestorArchivos - LeerContenidoArchivo - No hay archivos en la carpeta IN");
            }
        }

        public void TransformarContenidoArchivo(Factura aux)
        {
            string contenidoFormateado = $@"
                {{
                    banco: ""BancoEjemplo"",
                    cantidad: 1,
                    factura: {{
                        idFactura: {{ cuitEmisor: ""{aux.idFactura.cuitEmisor}"" }},
                        cuitComprador: ""{aux.cuitComprador}"",
                        cbuComprador: ""{aux.cbuComprador}"",
                        cbuEmisor: ""{aux.idFactura.cbuEmisor}"",
                        fechaVencimientoPago: ""{aux.fechaVencimientoPago}"",
                        saldoAceptado: {aux.saldoAceptado}
                    }}
                }}";

            CrearArchivoSalida(contenidoFormateado);

        }

        public void CrearArchivoSalida(string aux)
        {
            objetosTransformados.Add(aux);
            Console.WriteLine(string.Join(", ", objetosTransformados));
        }
    }
}