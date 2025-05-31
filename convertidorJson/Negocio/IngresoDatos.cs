using Dominio.DatosEntrada;
using System;

namespace Negocio
{
    public static class EntradaUsuario
    {
        public static DatosEntrada Pedir()
        {
            Console.Clear();
            DibujarVentana();

            int posX = 6;
            int posYBanco = 5;
            int posYCantidad = 7;

            // Mostrar etiquetas iniciales
            Console.SetCursorPosition(posX, posYBanco);
            Console.Write("-> Código del banco: ");
            Console.SetCursorPosition(posX, posYCantidad);
            Console.Write("   Cantidad de facturas por registro: ");

            // Input Banco
            Console.SetCursorPosition(posX + 21, posYBanco); // alineado justo después del texto
            Console.ForegroundColor = ConsoleColor.Yellow;
            string banco = Console.ReadLine();
            Console.ResetColor();

            // Borrar flecha anterior y marcar nuevo input
            Console.SetCursorPosition(posX, posYBanco);
            Console.Write("   Código del banco: ");

            Console.SetCursorPosition(posX, posYCantidad);
            Console.Write("-> Cantidad de facturas por registro: ");

            // Posición base del input (ajustar si cambiaste layout)
            int inputX = posX + 38;

            // Input cantidad
            Console.SetCursorPosition(inputX, posYCantidad);
            Console.ForegroundColor = ConsoleColor.Yellow;
            int cantidad;
            string input;
            while (true)
            {
                Console.SetCursorPosition(inputX, posYCantidad);
                Console.ForegroundColor = ConsoleColor.Yellow;

                // Leer y validar
                input = Console.ReadLine();

                if (int.TryParse(input, out cantidad))
                    break;

                // Mostrar mensaje de error
                Console.SetCursorPosition(posX, posYCantidad + 1);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("   Valor inválido. Ingresá un número válido.");

                // Borrar input anterior (sobreescribir con espacios)
                Console.SetCursorPosition(inputX, posYCantidad);
                Console.Write(new string(' ', input.Length));
                Console.SetCursorPosition(inputX, posYCantidad);
            }
            Console.ResetColor();


            return new DatosEntrada
            {
                Banco = banco,
                Cantidad = cantidad
            };
        }

        private static void DibujarVentana()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;

            string titulo = "INGRESO CODIGO DE BANCO Y CANTIDAD REGISTROS";
            int ancho = 70;
            int alto = 10;

            string bordeHorizontal = new string('═', ancho);

            Console.WriteLine($"╔{bordeHorizontal}╗");
            Console.WriteLine($"║{titulo.PadLeft((ancho + titulo.Length) / 2).PadRight(ancho)}║");
            Console.WriteLine($"╠{bordeHorizontal}╣");

            // Espacios vacíos interiores
            for (int i = 0; i < alto; i++)
                Console.WriteLine($"║{"".PadRight(ancho)}║");

            Console.WriteLine($"╚{bordeHorizontal}╝");

            Console.ResetColor();
        }
    }
}
