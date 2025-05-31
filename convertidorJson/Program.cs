using System;
using Dominio.Carpeta;
using Negocio;

class Program
{
    static void Main()
    {
        Console.Title = "Convertidor de FCEM";

        DibujarTitulo();

        Carpeta carpeta = new Carpeta(AppConfig.InputFolder);
        GestorArchivos gestor = new GestorArchivos(carpeta);

        gestor.LeerContenidoArchivo();

        //Console.WriteLine("\n\n\nPresione cualquier tecla para cerrar esta ventana. . .");
        //Console.ReadKey();

    }

    static void DibujarTitulo()
    {
        string titulo = " CONVERTIDOR DE FCEM ";
        int anchoConsola = Console.WindowWidth;
        string borde = new string('═', titulo.Length + 4);
        string lineaVacia = $"║{"".PadLeft(titulo.Length + 2)}║";
        string lineaTitulo = $"║ {titulo} ║";

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(borde.PadLeft((anchoConsola + borde.Length) / 2));
        Console.WriteLine(lineaVacia.PadLeft((anchoConsola + lineaVacia.Length) / 2));
        Console.WriteLine(lineaTitulo.PadLeft((anchoConsola + lineaTitulo.Length) / 2));
        Console.WriteLine(lineaVacia.PadLeft((anchoConsola + lineaVacia.Length) / 2));
        Console.WriteLine(borde.PadLeft((anchoConsola + borde.Length) / 2));
        Console.ResetColor();
        Console.WriteLine();
    }
}
