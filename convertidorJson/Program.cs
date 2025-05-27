using System;
using Dominio.Carpeta;
using Negocio;

class Program
{
    static void Main()
    {
        Console.WriteLine("Inicio\n");

        Carpeta carpeta = new Carpeta(AppConfig.InputFolder);
        GestorArchivos gestor = new GestorArchivos(carpeta);

        gestor.LeerContenidoArchivo();

        Console.WriteLine("Fin\n");
    }
}
