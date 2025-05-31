using System;
using Dominio.Carpeta;
using Negocio;

class Program
{
    static void Main()
    {
        Console.WriteLine($"Inicio - {DateTime.Now.ToString("yyyyMMdd_HHmmss")}\n");

        Carpeta carpeta = new Carpeta(AppConfig.InputFolder);
        GestorArchivos gestor = new GestorArchivos(carpeta);

        gestor.LeerContenidoArchivo();

        Console.WriteLine("\nFin");
    }
}
