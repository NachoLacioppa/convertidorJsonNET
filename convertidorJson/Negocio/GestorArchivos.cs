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

        //public void LeerContenidoArchivo()
        //{
        //    if (!Directory.Exists(Carpeta.Ruta))
        //    {
        //        Console.WriteLine("ERROR - GestorArchivos - LeerContenidoArchivo - Carpeta no existe");
        //        return;
        //    }

        //    string[] archivos = Directory.GetFiles(Carpeta.Ruta);
        //    if (archivos.Length > 0)
        //    {
        //        foreach (string archivo in archivos)
        //        {
        //            Console.WriteLine($"{archivo}");
        //            try
        //            {
        //                string contenido = File.ReadAllText(archivo);
        //                Factura datos = JsonSerializer.Deserialize<Factura>(contenido);

        //                TransformarContenidoArchivo(datos);

        //            }
        //            catch(Exception ex)
        //            {
        //                Console.WriteLine($"ERROR - GestorArchivos - LeerContenidoArchivo - {ex.Message}");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("WARNING! - GestorArchivos - LeerContenidoArchivo - No hay archivos en la carpeta IN");
        //    }
        //}
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
                    Console.WriteLine($"{archivo}");
                    try
                    {
                        string contenido = File.ReadAllText(archivo).Trim();

                        // Verificamos si hay más de un JSON (por coma sin corchetes)
                        bool esArrayJson = contenido.StartsWith("[") && contenido.EndsWith("]");
                        string contenidoArray = esArrayJson ? contenido : $"[{contenido}]";

                        // Intentamos deserializar como lista
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
            Console.WriteLine($"Saldo aceptado: {aux.saldoAceptado}");
            Console.WriteLine($"Fecha de vencimiento: {aux.fechaVencimientoPago}");
            Console.WriteLine($"CUIT Emisor: {aux.idFactura.cuitEmisor}");
            Console.WriteLine($"CBU Emisor: {aux.idFactura.cbuEmisor}");
            Console.WriteLine($"CUIT Comprador: {aux.cuitComprador}");
            Console.WriteLine($"CBU Comprador: {aux.cbuComprador}");
        }
    }
}