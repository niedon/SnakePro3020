using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Objeto serpiente, la misma clase gestiona las posiciones dentro del tablero y direcciones
/// </summary>
namespace Snake
{
    class Sierpe
    {
        private List<Punto> serpiente;
        private List<char> serpienteDibujo;
        private Direccion dirSiguiente;
        private Punto nuevaCabeza;
        private Random sacaLenguas;
        
        // De 0 a 100, frecuencia con la que la serpiente saca la lengua
        private int frecuenciaSacaLenguas;
        // Si la serpiente se dibujará con línea doble o simple
        private bool lineaGorda;

        public Sierpe(int ancho, int alto)
        {
            Initialize(ancho, alto, Direccion.Izquierda, 50, true);
        }

        public Sierpe(int ancho, int alto, Direccion dir)
        {
            Initialize(ancho, alto, dir, 50, true);
        }

        public Sierpe(int ancho, int alto, Direccion dir, int frecuenciaSacaLenguas, bool lineaGorda)
        {
            Initialize(ancho, alto, dir, frecuenciaSacaLenguas, lineaGorda);
        }

        internal List<Punto> Serpiente { get => serpiente; set => serpiente = value; }
        public List<char> SerpienteDibujo { get => serpienteDibujo; set => serpienteDibujo = value; }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="ancho">Ancho del tablero por el que se moverá</param>
        /// <param name="alto">Alto del tablero por el que se moverá</param>
        /// <param name="dir">Dirección en la que comenzará la serpiente</param>
        /// <param name="frecuenciaSacaLenguas">Frecuencia de 0 a 100</param>
        /// <param name="lineaGorda">Si se dibujará con línea doble o simple</param>
        private void Initialize(int ancho, int alto, Direccion dir, int frecuenciaSacaLenguas, bool lineaGorda)
        {
            serpiente = new List<Punto>();
            serpienteDibujo = new List<char>();
            sacaLenguas = new Random();

            this.frecuenciaSacaLenguas = frecuenciaSacaLenguas;
            this.lineaGorda = lineaGorda;

            Punto cabeza = new Punto(ancho/2, alto/2);
            Punto cuerpo = new Punto(cabeza);
            switch (dir)
            {
                case Direccion.Arriba:
                    cuerpo.Y++;
                    break;
                case Direccion.Izquierda:
                    cuerpo.X++;
                    break;
                case Direccion.Abajo:
                    cuerpo.Y--;
                    break;
                case Direccion.Derecha:
                    cuerpo.X--;
                    break;
            }

            serpiente.Add(cabeza);
            serpiente.Add(cuerpo);
            dirSiguiente = dir;
        }

        public void crecer()
        {
            siguienteMovimiento(true);
        }

        public void mover()
        {
            siguienteMovimiento(false);
        }

        public void autofagia()
        {
            siguienteMovimiento(true, true);
        }

        private void siguienteMovimiento(bool crece)
        {
            siguienteMovimiento(crece, false);
        }

        /// <summary>
        /// Calcula el siguiente movimiento de la serpiente
        /// </summary>
        /// <param name="crece">Si la serpiente crece</param>
        /// <param name="autofagia">Si la serpiente se partirá si choca consigo misma</param>
        private void siguienteMovimiento(bool crece, bool autofagia)
        {

            serpiente.Insert(0, nuevaCabeza);

            if (autofagia)
            {
                int indicePuntoChocado = serpiente.LastIndexOf(serpiente[0]);
                if (indicePuntoChocado != 0)
                {
                    serpiente.RemoveRange(indicePuntoChocado, (serpiente.Count - indicePuntoChocado));
                }
                else throw new Exception();
            }

            if (!crece) serpiente.RemoveAt(serpiente.Count - 1);

            dibujarSerpiente();
        }

        private void dibujarSerpiente()
        {
            serpienteDibujo.Clear();

            bool sacaLenguaEstaVez = sacaLenguas.Next(100) < frecuenciaSacaLenguas;

            // Cabeza
            if (sacaLenguaEstaVez)
            {
                serpienteDibujo.Add(lineaGorda ? '╬' : '┼');
            }
            else
            {
                switch (serpiente[0].tieneA(serpiente[1]))
                {
                    case Direccion.Arriba:
                        serpienteDibujo.Add(lineaGorda ? '╩' : '┴');
                        break;
                    case Direccion.Derecha:
                        serpienteDibujo.Add(lineaGorda ? '╠' : '├');
                        break;
                    case Direccion.Abajo:
                        serpienteDibujo.Add(lineaGorda ? '╦' : '┬');
                        break;
                    case Direccion.Izquierda:
                        serpienteDibujo.Add(lineaGorda ? '╣' : '┤');
                        break;
                }
            }

            // Cuerpo
            for(int i = 1; i < serpiente.Count-1; i++)
            {
                serpienteDibujo.Add(caracter(serpiente[i - 1], serpiente[i], serpiente[i + 1]));
            }

            // Cola
            int ult = serpiente.Count - 1;
            if(serpiente[ult].tieneA(serpiente[ult-1]) == Direccion.Arriba || serpiente[ult].tieneA(serpiente[ult - 1]) == Direccion.Abajo){
                SerpienteDibujo.Add(lineaGorda ? '║' : '│');
            }
            else
            {
                SerpienteDibujo.Add(lineaGorda ? '═' : '─');
            }
        }

        /// <summary>
        /// Calcula el caracter correspondiente del punto intermedio
        /// </summary>
        /// <param name="anterior">Punto anterior</param>
        /// <param name="intermedio">Punto del que se calculará el caracter</param>
        /// <param name="siguiente">Punto siguiente</param>
        /// <returns>Caracter asociado al punto dado de la serpiente</returns>
        private char caracter(Punto anterior, Punto intermedio, Punto siguiente)
        {
            // Se asocian números primos (y 1) a la serpiente, con lo que siempre
            // se darán resultados únicos en la multiplicación de direcciones
            int multi = (int)intermedio.tieneA(anterior) * (int)intermedio.tieneA(siguiente);

            switch (multi)
            {
                case 2:
                    return lineaGorda ? '╚' : '└';
                case 6:
                    return lineaGorda ? '╔' : '┌';
                case 15:
                    return lineaGorda ? '╗' : '┐';
                case 5:
                    return lineaGorda ? '╝' : '┘';
                case 3:
                    return lineaGorda ? '║' : '│';
                case 10:
                    return lineaGorda ? '═' : '─';
            }
            return '\0';

        }

        /// <summary>
        /// Confirma la dirección que tomará la serpiente en el siguiente movimiento ante un input de dirección
        /// </summary>
        /// <param name="dir">Dirección de input</param>
        /// <returns>Dirección definitiva</returns>
        public Punto confirmarDireccion(Direccion dir)
        {
            // Ignora cambio de dirección si se dan direcciones opuestas
            if (dir == Direccion.Arriba && dirSiguiente == Direccion.Abajo)
            {
            }
            else if (dir == Direccion.Derecha && dirSiguiente == Direccion.Izquierda)
            {
            }
            else if (dir == Direccion.Abajo && dirSiguiente == Direccion.Arriba)
            {
            }
            else if (dir == Direccion.Izquierda && dirSiguiente == Direccion.Derecha)
            {
            }
            else
            {
                dirSiguiente = dir;
            }

            //Calcula la posición que tomará la cabeza en el siguiente movimiento
            nuevaCabeza = new Punto(serpiente[0]);

            switch (dirSiguiente)
            {
                case Direccion.Arriba:
                    nuevaCabeza.Y--;
                    break;
                case Direccion.Izquierda:
                    nuevaCabeza.X--;
                    break;
                case Direccion.Abajo:
                    nuevaCabeza.Y++;
                    break;
                case Direccion.Derecha:
                    nuevaCabeza.X++;
                    break;
            }


            return nuevaCabeza;
        }

        /// <summary>
        /// Prepara una serpiente para aparecer en el tablero de opciones (5x5 y en diagonal)
        /// </summary>
        public void prepararParaOpciones()
        {
            serpiente.Clear();
            serpiente.Add(new Punto(0,0));
            serpiente.Add(new Punto(0,1));
            serpiente.Add(new Punto(1,1));
            serpiente.Add(new Punto(1,2));
            serpiente.Add(new Punto(2,2));
            serpiente.Add(new Punto(2,3));
            serpiente.Add(new Punto(3,3));
            serpiente.Add(new Punto(3,4));
            serpiente.Add(new Punto(4,4));
            dibujarSerpiente();
        }

        /// <summary>
        /// Mueve a la serpiente en zigzag
        /// </summary>
        public void moverParaOpciones()
        {
            //ojo al +=2
            for (int i = 1; i < serpiente.Count; i+=2)
            {
                int temp = serpiente[i].X;
                serpiente[i].X = serpiente[i].Y;
                serpiente[i].Y = temp;
            }
            dibujarSerpiente();
        }

        /// <summary>
        /// Altera los parámetros de la serpiente
        /// </summary>
        /// <param name="frecuencia">De 0 a 100, frecuencia a la que sacará la lengua</param>
        /// <param name="gordi">Si la serpiente se dibujará con línea doble o simple</param>
        public void cambiarParametrosParaOpciones(int frecuencia, bool gordi)
        {
            frecuenciaSacaLenguas = frecuencia;
            lineaGorda = gordi;
        }

    }
}
