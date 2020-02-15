using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    /// <summary>
    /// Clase personalizada de punto en el tablero
    /// </summary>
    class Punto :IEquatable<Punto>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Punto(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Punto(Punto segundoPunto)
        {
            X = segundoPunto.X;
            Y = segundoPunto.Y;
        }

        public bool Equals(Punto otro)
        {
            return otro != null && otro.X == X && otro.Y == Y;
        }

        /// <summary>
        /// Comprueba si este punto tiene a otro directamente contiguo
        /// </summary>
        /// <param name="p">Punto a comprobar respecto a este</param>
        /// <returns>Dirección a la que está el otro punto</returns>
        public Direccion tieneA(Punto p)
        {
            if (p == null) throw new InvalidOperationException();
            if (X == p.X && Y == p.Y - 1) return Direccion.Abajo;
            if (X == p.X && Y == p.Y + 1) return Direccion.Arriba;
            if (X == p.X - 1 && Y == p.Y) return Direccion.Derecha;
            if (X == p.X + 1 && Y == p.Y) return Direccion.Izquierda;
            throw new InvalidOperationException();
        }
    }
}
