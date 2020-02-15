using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    /// <summary>
    /// Bean de puntuación individual
    /// </summary>
    class Puntuacion
    {
        private int puntos;
        private string nombre;

        public int Puntos { get => puntos; set => puntos = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        
    }
}
