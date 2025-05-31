using System;
using System.Collections.Generic;
using System.Linq;
using Dominio.DatosEntrada;
using System;

namespace Negocio
{
    public static class EntradaUsuario
    {
        public static DatosEntrada Pedir()
        {
            Console.Write("Ingresá el codigo del banco: ");
            string banco = Console.ReadLine();

            Console.Write("Ingresá la cantidad de facturas por registro: ");
            int cantidad;
            while (!int.TryParse(Console.ReadLine(), out cantidad))
            {
                Console.Write("Valor inválido. Vuelva a ingresar la cantidad de facturas por registro: ");
            }

            return new DatosEntrada
            {
                Banco = banco,
                Cantidad = cantidad
            };
        }
    }
}
