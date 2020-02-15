using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snake
{
    class AdaptadorGrafico
    {

        private static readonly string[] letrasIntro = {
            "╔═══╗                 ╔═══╗        ╔═══╗╔═══╗╔═══╗╔═══╗",
            "║╔══╝        ╔╗ ╔╗    ║╔═╗║        ╚══╗║║╔═╗║╚══╗║║╔═╗║",
            "║║   ╔══╗╔══╗║║╔╝║╔══╗║║ ║║╔══╗╔══╗   ║║║║╔╝║   ║║║║╔╝║",
            "║╚══╗║╔╗║╚═╗║║║║╔╝║╔╗║║╚═╝║║╔═╝║╔╗║╔══╝║║╚╝ ║ ╔═╝║║╚╝ ║",
            "╚══╗║║║║║╔═╝║║╚╝║ ║╚╝║║╔══╝║║  ║║║║╚══╗║║ ╔╗║╔╝ ╔╝║ ╔╗║",
            "   ║║║║║║║╔╗║║╔╗╚╗║╔═╝║║   ║║  ║║║║   ║║║╔╝║║║ ╔╝ ║╔╝║║",
            "╔══╝║║║║║║╚╝║║║╚╗║║╚═╗║║   ║║  ║╚╝║╔══╝║║╚═╝║║ ╚═╗║╚═╝║",
            "╚═══╝╚╝╚╝╚══╝╚╝ ╚╝╚══╝╚╝   ╚╝  ╚══╝╚═══╝╚═══╝╚═══╝╚═══╝"
        };

        private static readonly  string[] letrasGameOver = {
            "╔════╗               ╔════╗              ",
            "║╔═══╝               ║╔══╗║              ",
            "║║    ╔══╗╔════╗╔══╗ ║║  ║║╔╗  ╔╗╔══╗╔══╗",
            "║║╔══╗╚═╗║║╔╗╔╗║║╔╗║ ║║  ║║║╚╗╔╝║║╔╗║║╔═╝",
            "║║╚═╗║╔═╝║║║║║║║║╚╝║ ║║  ║║╚╗║║╔╝║╚╝║║║  ",
            "║║  ║║║╔╗║║║║║║║║╔═╝ ║║  ║║ ║╚╝║ ║╔═╝║║  ",
            "║╚══╝║║╚╝║║║║║║║║╚═╗ ║╚══╝║ ╚╗╔╝ ║╚═╗║║  ",
            "╚════╝╚══╝╚╝╚╝╚╝╚══╝ ╚════╝  ╚╝  ╚══╝╚╝  "
        };

        private static readonly string[] letrasVictoria = {
            "╔══╗   ╔══╗          ╔═╗                        ",
            "╚╗ ║   ║ ╔╝╔═╗     ╔═╝ ╚═╗             ╔═╗      ",
            " ║ ║   ║ ║ ╚═╝╔═══╗╚═╗ ╔═╝╔═════╗╔════╗╚═╝╔════╗",
            " ║ ╚╗ ╔╝ ║ ╔═╗║ ╔═╝  ║ ║  ║ ╔═╗ ║║ ╔══╝╔═╗╚═══╗║",
            " ╚╗ ║ ║ ╔╝ ║ ║║ ║    ║ ║  ║ ║ ║ ║║ ║   ║ ║╔═══╝║",
            "  ╚╗╚═╝╔╝  ║ ║║ ║    ║ ║  ║ ║ ║ ║║ ║   ║ ║║ ╔╗ ║",
            "   ╚╗ ╔╝   ║ ║║ ╚═╗  ║ ║  ║ ╚═╝ ║║ ║   ║ ║║ ╚╝ ║",
            "    ╚═╝    ╚═╝╚═══╝  ╚═╝  ╚═════╝╚═╝   ╚═╝╚════╝"
        };

        private static readonly string[] comentarioManzana = {
            "No es una manzana pero se le parece",      // Ó
            "Lo importante es ser feliz",               // ☺
            "De algo así hablaba una leyenda china",    // ☼
            "Por si añoras Los Sims",                   // §
            "A esta serpiente le gusta el Copyleft",    // ©
            "Con más música de la que esperabas",       // ♫
            "El cero por antonomasia en informática",   // Ø
            "Esto se parece a una casa",                // ⌂
            "Para acabar con tanta parrafada",          // ¶
            "Quien fue a cedilla perdió la silla" };    // Ç
        

        /// <summary>
        /// Muestra la animación de inicio de juego
        /// </summary>
        public void mostrarIntro()
        {
            // Se preparan las cadenas a mostrar
            int longitudCadena = letrasIntro[0].Length;
            string cualquierTecla = padCenter("Pulse cualquier tecla para comenzar",longitudCadena);

            // Sonido de entrada
            Thread t = new Thread(() =>
            {
                int dura = 200;
                int[] frecuencias = { 139, 277, 554, 1109, 2217, 4435 };
                for (int i = 0; i < 6; i += 2)
                {
                    Console.Beep(frecuencias[i], dura);
                    Console.Beep(frecuencias[i + 1], dura);
                    Console.Beep(frecuencias[i], dura);
                    Console.Beep(frecuencias[i + 1], i == 4 ? dura * 4 : dura);
                }

            });
            t.Start();

            // Animación
            for (int i = 0; i < longitudCadena; i++)
            {
                Console.Clear();
                foreach (string s in letrasIntro)
                {
                    Console.WriteLine(new String(' ', longitudCadena - (i + 1)) + s.Substring(0, i + 1));
                }

                // Evita la espera de la presentación pulsando enter
                if (Console.KeyAvailable)
                {
                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                    {
                        t.Abort();
                        return;
                    }
                }
                Thread.Sleep(60);
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(cualquierTecla);
            Console.ReadKey();
        }

        /// <summary>
        /// Muestra el menú de inicio
        /// </summary>
        /// <param name="pos">Posición en la que aparecerá el puntero</param>
        public void menuInicio(int pos)
        {
            
            string[] menu = new string[5];
            menu[0]= padCenter($" {(pos == 0 ? '>' : ' ')} COMENZAR PARTIDA", letrasIntro[0].Length);
            menu[1]= padCenter($" {(pos == 1 ? '>' : ' ')} INSTRUCCIONES   ", letrasIntro[0].Length);
            menu[2]= padCenter($" {(pos == 2 ? '>' : ' ')} OPCIONES        ", letrasIntro[0].Length);
            menu[3]= padCenter($" {(pos == 3 ? '>' : ' ')} PUNTUACIONES    ", letrasIntro[0].Length);
            menu[4]= padCenter($" {(pos == 4 ? '>' : ' ')} SALIR           ", letrasIntro[0].Length);

            Console.Clear();
            foreach (string s in letrasIntro) Console.WriteLine(s);
            foreach (string s in menu)
            { 
                Console.WriteLine();
                Console.WriteLine(s);
            }

        }

        /// <summary>
        /// Muestra la elección de ancho o alto del tablero de juego
        /// </summary>
        /// <param name="parametro">Ancho o alto</param>
        /// <param name="min">Dimensión mínima</param>
        /// <param name="max">Dimensión máxima</param>
        /// <param name="mensaje">Mensaje informativo que se mostrará</param>
        public void elegirParametro(string parametro, int min, int max, string mensaje)
        {
            string formato = $"Elige {parametro} del tablero (entre {min} y {max})".PadRight(53);
            Console.Clear();
            foreach (string s in letrasIntro) Console.WriteLine(s);
            Console.WriteLine();
            Console.WriteLine(mensaje);
            Console.WriteLine();
            Console.WriteLine("╔═════════════════════════════════════════════════════╗");
            Console.WriteLine($"║{formato}║");
            Console.WriteLine("╚═════════════════════════════════════════════════════╝");
            Console.WriteLine();
        }

        /// <summary>
        /// Muestra la elección de dificultad de la partida
        /// </summary>
        /// <param name="ancho">Ancho definitivo del tablero</param>
        /// <param name="alto">Alto definitivo del tablero</param>
        /// <param name="pos">Posición del marcador</param>
        public void elegirDificultad(int ancho, int alto, int pos)
        {
            string formato = "Elige la dificultad";
            formato = formato.PadRight((53 - formato.Length) / 2 + formato.Length).PadLeft(53);

            Console.Clear();
            foreach (string s in letrasIntro) Console.WriteLine(s);
            Console.WriteLine();
            Console.WriteLine($"Tablero de {ancho}x{alto}");
            Console.WriteLine();
            Console.WriteLine("╔═════════════════════════════════════════════════════╗");
            Console.WriteLine($"║{formato}║");
            Console.WriteLine("╚═════════════════════════════════════════════════════╝");
            Console.WriteLine();
            switch (pos)
            {
                case 0:
                    Console.WriteLine("[FÁCIL] NORMAL  DIFÍCIL ");
                    break;
                case 1:
                    Console.WriteLine(" FÁCIL [NORMAL] DIFÍCIL ");
                    break;
                case 2:
                    Console.WriteLine(" FÁCIL  NORMAL [DIFÍCIL]");
                    break;
            }
        }

        /// <summary>
        /// Muestra la pantalla de instrucciones
        /// </summary>
        public void pantallaInstrucciones()
        {
            Console.Clear();
            foreach (string s in letrasIntro) Console.WriteLine(s);
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine(padCenter("   ←↑→↓ - Movimiento       ", letrasIntro[0].Length));
            Console.WriteLine(padCenter("  Enter - Seleccionar      ", letrasIntro[0].Length));
            Console.WriteLine(padCenter("      P - Pausar en partida", letrasIntro[0].Length));

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine(padCenter("SnakePro3020, por Basilio Cadaval (github.com/niedon)", letrasIntro[0].Length));
        }

        /// <summary>
        /// Muestra la pantalla de opciones
        /// </summary>
        /// <param name="pos">Posición del marcador</param>
        /// <param name="serpiente">Representación gráfica de la serpiente</param>
        /// <param name="opciones">Información de opciones que se mostrarán</param>
        public void pantallaOpciones(int pos, char[,] serpiente, BeanOpciones opciones)
        {
            // Líneas del recuadro de la serpiente
            string[] serpienteDibujo = new string[11];
            serpienteDibujo[0] = new string(' ',14);
            serpienteDibujo[1] = " ╔" + new String('═', 11) + '╗';
            serpienteDibujo[2] = " ║" + new String(' ', 11) + '║';
            serpienteDibujo[8] = " ║" + new String(' ', 11) + '║';
            serpienteDibujo[9] = " ╚" + new String('═', 11) + '╝';
            serpienteDibujo[10] = serpienteDibujo[0];
            for (int i = 0; i < 5; i++)
            {
                char[] temp = new char[5];
                Buffer.BlockCopy(serpiente, i * 5 * 2, temp, 0, 5 * 2);
                serpienteDibujo[i + 3] = " ║   " + new String(temp) + "   ║";
            }

            // Líneas de las opciones
            string[] opcionesDibujo = new string[11];
            opcionesDibujo[0] = "  " + (pos == 0 ? '>' : ' ') + " GROSOR      ";
            opcionesDibujo[0] += opciones.EsGorda ? " CULEBRILLA  [ANACONDA]" :
                                                    "[CULEBRILLA]  ANACONDA ";

            opcionesDibujo[2] = "  " + (pos == 1 ? '>' : ' ') + " LENGUA      ";
            opcionesDibujo[2] += opciones.SacaLengua ? "[BURLONA]  TÍMIDA " :
                                                        " BURLONA  [TÍMIDA]";

            opcionesDibujo[4] = "  " + (pos == 2 ? '>' : ' ') + " FRECUENCIA  ";
            opcionesDibujo[4] += opciones.SacaLengua ? $"<{opciones.FrecuenciaSacaLengua}%>" : "[LAS SERPIENTES TÍMIDAS NO SACAN LA LENGUA]";

            opcionesDibujo[6] = "  " + (pos == 3 ? '>' : ' ') + " MANZANA     ";
            opcionesDibujo[6] += $"< {opciones.Manzana} > {comentarioManzana[opciones.IndiceManzana]}";

            opcionesDibujo[8] = "  " + (pos == 4 ? '>' : ' ') + " GUARDAR     ";
            opcionesDibujo[10] = "  " + (pos == 5 ? '>' : ' ') + " SALIR       ";

            
            // Borrar consola y dibujar
            Console.Clear();

            foreach (string s in letrasIntro) Console.WriteLine(s);
            Console.WriteLine();
            for (int i = 0; i < serpienteDibujo.Length; i++)
            {
                Console.WriteLine(serpienteDibujo[i] + opcionesDibujo[i]);
            }

        }

        /// <summary>
        /// Muestra la pantalla de puntuaciones más altas
        /// </summary>
        /// <param name="pos">Posición del marcador</param>
        /// <param name="nombres">Nombres y puntuaciones que se mostrarán, o null si no hay</param>
        public void pantallaPuntuaciones(int pos, string[,] nombres)
        {
            // Se crean dos bloques "cuadrados" de array
            string[] arrModos = new string[5];
            arrModos[0] = $" {(pos == 0 ? '>' : ' ')} FÁCIL      ";
            arrModos[1] = $" {(pos == 1 ? '>' : ' ')} NORMAL     ";
            arrModos[2] = $" {(pos == 2 ? '>' : ' ')} DIFÍCIL    ";
            arrModos[3] = $" {(pos == 3 ? '>' : ' ')} GANADORES  ";
            arrModos[4] = new string(' ', 14);

            string[] arrNombres = new string[5];
            if (nombres == null)
            {
                for (int i = 0; i < arrNombres.Length; i++)
                {
                    arrNombres[i] = padCenter("[ SIN DATOS ]",15);
                }
            }
            else
            {
                for (int i = 0; i < arrNombres.Length; i++)
                {
                    arrNombres[i] = $"{nombres[i, 0]}  {nombres[i, 1]}";
                }
            }
            

            Console.Clear();

            foreach (string s in letrasIntro) Console.WriteLine(s);
            Console.WriteLine();
            for (int i = 0; i < arrModos.Length; i++)
            {
                Console.WriteLine(arrModos[i] + arrNombres[i]);
                Console.WriteLine();
            }

        }

        /// <summary>
        /// Método principal, dibuja un frame de la partida
        /// </summary>
        /// <param name="puntos">Puntos actuales del jugador</param>
        /// <param name="tablero">Representación gráfica de la serpiente</param>
        public void dibujarFrame(int puntos, char[,] tablero)
        {
            // Se asignan estos valores porque se accederá mucho a ellos
            int anchoTablero = tablero.GetLength(1);
            int altoTablero = tablero.GetLength(0);

            // Procesamiento de primeras líneas (puntos y borde superior)
            string puntosString = puntos.ToString();
            string simboloPuntos = "Puntos:";
            if (simboloPuntos.Length + puntosString.Length > anchoTablero+2) simboloPuntos = "P:";
            string intermedio = new string(' ', anchoTablero + 2 - (simboloPuntos.Length + puntosString.Length));

            // Todas las líneas "estáticas"
            string primeraLinea = simboloPuntos + intermedio + puntosString;
            string segundaLinea = '╔' + new String('═', anchoTablero) + '╗';
            string[] lineasIntermedias = new string[altoTablero];
            string ultimaLinea = '╚' + new String('═', anchoTablero) + '╝';

            // Se pasa el tablero char[,] a string[] para evitar procesamiento
            // en la creación del frame
            for (int i = 0; i < altoTablero; i++)
            {
                char[] temp = new char[anchoTablero];
                //Se usa este método para ahorrar recursos, ya que
                //estas líneas se transitan mucho
                Buffer.BlockCopy(tablero, i * anchoTablero * 2, temp, 0, anchoTablero * 2);
                lineasIntermedias[i] = '║' + new String(temp) + '║';
            }

            // Tras hacer todas las instancias y cálculos se
            // refresca el frame, así se evita flickering
            Console.Clear();

            Console.WriteLine(primeraLinea);
            Console.WriteLine(segundaLinea);
            foreach (string s in lineasIntermedias) Console.WriteLine(s);
            Console.WriteLine(ultimaLinea);
        }

        /// <summary>
        /// Muestra la pantalla de pausa
        /// </summary>
        /// <param name="puntos">Puntos actuales del jugador</param>
        /// <param name="tablero">Representación gráfica de la serpiente</param>
        public void dibujarPausa(int puntos, char[,] tablero)
        {
            int anchoTablero = tablero.GetLength(1);
            int altoTablero = tablero.GetLength(0);

            //Primeras líneas
            string puntosString = puntos.ToString();
            string simboloPuntos = "Puntos:";
            string intermedio = new string(' ', anchoTablero + 2 - (simboloPuntos.Length + puntosString.Length));

            string primeraLinea = simboloPuntos + intermedio + puntosString;
            string segundaLinea = '╔' + new String('═', anchoTablero) + '╗';
            string lineaIntermedia = '║' + new String(' ',anchoTablero) + '║';
            string ultimaLinea = '╚' + new String('═', anchoTablero) + '╝';

            int indiceLetrero = altoTablero / 2;

            string simboloPausa = "[PAUSA]";
            int anchoSinTexto = anchoTablero - simboloPausa.Length;
            string lineaPausa = '║' + new String(' ', anchoSinTexto / 2) + simboloPausa + new string(' ',(anchoSinTexto % 2==0) ? anchoSinTexto/2 : (anchoSinTexto/2)+1) + '║';


            // Tras hacer todas las instancias y cálculos se
            // refresca el frame, así se evita flickering
            Console.Clear();

            Console.WriteLine(primeraLinea);
            Console.WriteLine(segundaLinea);
            for(int i=0; i<altoTablero; i++)
            {
                if (i == indiceLetrero) Console.WriteLine(lineaPausa);
                else Console.WriteLine(lineaIntermedia);
            }
            Console.WriteLine(ultimaLinea);
        }

        /// <summary>
        /// Muestra la pantalla de partida perdida
        /// </summary>
        /// <param name="esVictoria">Si el final de partida es por victoria o derrota</param>
        /// <param name="puntos">Puntos que ha conseguido el jugador</param>
        /// <param name="puestoRecord">La posición en la que ha quedado (o -1 si no hay récord)</param>
        /// <param name="nombre">Nombre actual que ha elegido el jugador</param>
        public void dibujarPartidaFinalizada(bool esVictoria, int puntos, int puestoRecord, string nombre)
        {
            int anchoPantalla = esVictoria ? letrasVictoria[0].Length : letrasGameOver[0].Length;

            string puntuacion = padCenter($"PUNTUACIÓN: {puntos}", anchoPantalla);
            string record = padCenter($"PUESTO CONSEGUIDO: {puestoRecord}", anchoPantalla);
            string insertaNom = padCenter("INTRODUZCA SU NOMBRE", anchoPantalla);
            string cuadroArriba = padCenter("╔════════════════╗", anchoPantalla);
            string cuadroMedio = padCenter($"║{nombre.PadRight(16)}║", anchoPantalla);
            string cuadroAbajo = padCenter("╚════════════════╝", anchoPantalla);

            Console.Clear();
            if (esVictoria) foreach (string s in letrasVictoria) Console.WriteLine(s);
            else foreach (string s in letrasGameOver) Console.WriteLine(s);

            Console.WriteLine();
            Console.WriteLine(puntuacion);

            // Si se ha conseguido récord, aparece el input para el nombre
            if (puestoRecord > 0)
            {
                Console.WriteLine();
                Console.WriteLine(record);
                Console.WriteLine();
                Console.WriteLine(insertaNom);
                Console.WriteLine();
                Console.WriteLine(cuadroArriba);
                Console.WriteLine(cuadroMedio);
                Console.WriteLine(cuadroAbajo);
            }
        }

        /// <summary>
        /// Muestra la animación de derrota
        /// </summary>
        public void animacionDerrota()
        {
            // Tono musical final
            Thread t = new Thread(() =>
            {
                int dura = 250;
                int[] frecuencias = { 139, 277, 554, 1109, 2217, 4435 };
                Console.Beep(frecuencias[4], dura);
                Console.Beep(frecuencias[3], dura);
                Console.Beep(frecuencias[2], dura);
                Console.Beep(frecuencias[3], dura);
                Console.Beep(frecuencias[2], dura);
                Console.Beep(frecuencias[1], dura);
                Console.Beep(frecuencias[2], dura * 4);

            });
            t.Start();

            int ancho = letrasGameOver[0].Length;

            // Animación game over
            for (int i = 0; i < ancho; i++)
            {
                Console.Clear();

                for (int j = 0; j < letrasGameOver.Length; j++)
                {
                    if (j % 2 == 0)
                    {
                        Console.WriteLine(new String(' ', ancho - (i + 1)) + letrasGameOver[j].Substring(0, i + 1));
                    }
                    else
                    {
                        Console.WriteLine(letrasGameOver[j].Substring(ancho - (i + 1)) + new String(' ', ancho - i));
                    }
                }

                // Evita la espera de la presentación pulsando enter
                if (Console.KeyAvailable)
                {
                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                    {
                        t.Abort();
                        break;
                    }
                }
                Thread.Sleep(60);
            }
        }

        /// <summary>
        /// Muestra la animación de victoria (solo unos pocos elegidos podrán verla siquiera)
        /// </summary>
        public void animacionVictoria()
        {
            // Tono musical
            Thread t = new Thread(() =>
            {
                int tiempoNegra = 500;

                Console.Beep(523 * 2, tiempoNegra / 3);
                Console.Beep(523 * 2, tiempoNegra / 3);
                Console.Beep(523 * 2, tiempoNegra / 3);
                Console.Beep(523 * 2, tiempoNegra);
                Console.Beep(415 * 2, tiempoNegra);
                Console.Beep(466 * 2, tiempoNegra);
                Console.Beep(523 * 2, (int)(tiempoNegra * 0.6));
                Console.Beep(466 * 2, (int)(tiempoNegra * 0.3));
                Console.Beep(523 * 2, tiempoNegra * 3);
            });
            t.Start();

            int longitudCadena = letrasVictoria[0].Length;

            // Animación
            for (int i = 0; i < longitudCadena; i++)
            {
                Console.Clear();
                foreach (string s in letrasVictoria)
                {
                    Console.WriteLine(new String(' ', longitudCadena - (i + 1)) + s.Substring(0, i + 1));
                }

                // Evita la espera de la presentación pulsando enter
                if (Console.KeyAvailable)
                {
                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                    {
                        t.Abort();
                        return;
                    }
                }
                Thread.Sleep(60);
            }

            Console.WriteLine();
            Console.WriteLine(padCenter("¡¡¡HAS GANADO!!!",longitudCadena));
            Console.ReadKey(true);
        }

        /// <summary>
        /// Centra un string rellenando el resto con espacios en blanco
        /// </summary>
        /// <param name="origen">String original</param>
        /// <param name="longitudNuevoString">Longitud que tendrá el nuevo string</param>
        /// <returns></returns>
        private string padCenter(string origen, int longitudNuevoString)
        {
            return origen.PadRight((longitudNuevoString - origen.Length) / 2 + origen.Length).PadLeft(longitudNuevoString);
        }

    }
}
