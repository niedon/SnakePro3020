using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snake
{
    /// <summary>
    /// Clase principal del programa. Controlador. Gestiona todo lo demás.
    /// </summary>
    class GameManager
    {
        // Vista y modelo, respectivamente
        private readonly AdaptadorGrafico ag;
        private readonly Modelo modelo;

        // Datos por defecto del tablero
        private static readonly int anchoMin = 10;//No menos de 7
        private static readonly int anchoDef = 30;
        private static readonly int anchoMax = 50;
        private static readonly int altoMin = 10;
        private static readonly int altoDef = 15;
        private static readonly int altoMax = 20;

        // Opciones/personalización
        private BeanOpciones beanOpciones;
        private static readonly char[] arrayManzana = { 'Ó', '☺', '☼', '§', '©', '♫', 'Ø', '⌂', '¶', 'Ç' };

        // Datos de esta partida
        private int anchoTablero;
        private int altoTablero;
        private int dificultad;
        private int tiempoBase;
        private int tiempoDinamico;

        private string mensaje;

        private bool partidaGanada;

        private bool muertePorAutofagiaEnModoFacil;

        private char[,] tablero;
        private int puntos;
        private Random generaManzanas;
        private bool hayManzana;
        private Punto nuevaManzana;
        
        private Sierpe sierpe;
        private Direccion direccionElegida;

        public GameManager()
        {
            ag = new AdaptadorGrafico();
            modelo = new Modelo();
            beanOpciones = modelo.getOpciones();

        }

        /// <summary>
        /// Inicia la partida. Es necesario ejecutarlo para comenzar, no vale solo con instanciar.
        /// </summary>
        public void iniciar()
        {
            ag.mostrarIntro();
            
            while (true)
            {
                
                switch (menuInicio())
                {
                    case 0:
                        iniciarParametros();
                        comenzarPartida();
                        break;
                    case 1:
                        instrucciones();
                        break;
                    case 2:
                        opciones();
                        break;
                    case 3:
                        puntuaciones();
                        break;
                    case 4:
                        return;
                }
            }
        }

        /// <summary>
        /// Controla la pantalla del menú de inicio
        /// </summary>
        /// <returns>Posición del marcador en el menú</returns>
        private int menuInicio()
        {
            // Posición del puntero
            int pos = 0;

            while (true)
            {
                ag.menuInicio(pos);

                ConsoleKey k = Console.ReadKey(true).Key;
                if (k == ConsoleKey.DownArrow && pos < 4) pos++;
                else if (k == ConsoleKey.UpArrow && pos > 0) pos--;
                else if (k == ConsoleKey.Enter) return pos;
            }
        }

        /// <summary>
        /// Controla la pantalla de instrucciones
        /// </summary>
        private void instrucciones()
        {
            ag.pantallaInstrucciones();
            while (true) if (Console.ReadKey(true).Key == ConsoleKey.Enter) break;
        }

        /// <summary>
        /// Controla la pantalla de opciones
        /// </summary>
        private void opciones()
        {
            // Marcador de opción y minitablero para serpiente de muestra
            int marcador = 0;
            char[,] tableroOpciones = new char[5, 5];

            // Se cargan las opciones en el bean para modificar sobre la marcha
            BeanOpciones opcionesTemp = new BeanOpciones();
            opcionesTemp.EsGorda = beanOpciones.EsGorda;
            opcionesTemp.SacaLengua = beanOpciones.SacaLengua;
            opcionesTemp.FrecuenciaSacaLengua = beanOpciones.FrecuenciaSacaLengua;
            opcionesTemp.Manzana = arrayManzana[beanOpciones.IndiceManzana];
            opcionesTemp.IndiceManzana = beanOpciones.IndiceManzana;

            // Se instancia la serpiente de muestra
            Sierpe serpienteDeLasOpciones = new Sierpe(5, 5, Direccion.Izquierda, (opcionesTemp.SacaLengua ? opcionesTemp.FrecuenciaSacaLengua : 0), opcionesTemp.EsGorda);
            serpienteDeLasOpciones.prepararParaOpciones();

            while (true)
            {
                // Para que la interfaz responda mejor, se actualiza la lectura de
                // tecla cada 30ms pero se mueve la serpiente cada 300
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(30);
                    if (Console.KeyAvailable)
                    {
                        ConsoleKey k = Console.ReadKey(true).Key;

                        switch (k)
                        {
                            case ConsoleKey.UpArrow:
                                if (marcador > 0) marcador--;
                                break;
                            case ConsoleKey.DownArrow:
                                if (marcador < 5) marcador++;
                                break;
                            case ConsoleKey.RightArrow:
                                if (marcador == 0 && !opcionesTemp.EsGorda) opcionesTemp.EsGorda = true;
                                else if (marcador == 1 && opcionesTemp.SacaLengua) opcionesTemp.SacaLengua = false;
                                else if (marcador == 2 && opcionesTemp.FrecuenciaSacaLengua < 100) opcionesTemp.FrecuenciaSacaLengua += 5;
                                else if (marcador == 3 && opcionesTemp.IndiceManzana < arrayManzana.Length - 1)
                                {
                                    opcionesTemp.IndiceManzana++;
                                    opcionesTemp.Manzana = arrayManzana[opcionesTemp.IndiceManzana];
                                }
                                break;
                            case ConsoleKey.LeftArrow:
                                if (marcador == 0 && opcionesTemp.EsGorda) opcionesTemp.EsGorda = false;
                                else if (marcador == 1 && !opcionesTemp.SacaLengua) opcionesTemp.SacaLengua = true;
                                else if (marcador == 2 && opcionesTemp.FrecuenciaSacaLengua > 0) opcionesTemp.FrecuenciaSacaLengua -= 5;
                                else if (marcador == 3 && opcionesTemp.IndiceManzana > 0)
                                {
                                    opcionesTemp.IndiceManzana--;
                                    opcionesTemp.Manzana = arrayManzana[opcionesTemp.IndiceManzana];
                                }
                                break;
                            case ConsoleKey.Enter:
                                if (marcador == 4)
                                {
                                    // Se guardan las opciones en el modelo
                                    modelo.guardaOpciones(opcionesTemp);
                                    beanOpciones = modelo.getOpciones();
                                    return;
                                }
                                else if (marcador == 5) return;
                                break;
                            case ConsoleKey.Escape:
                                return;
                        }

                        // Se actualizan los parámetros de la serpiente
                        serpienteDeLasOpciones.cambiarParametrosParaOpciones(opcionesTemp.SacaLengua ? opcionesTemp.FrecuenciaSacaLengua : 0, opcionesTemp.EsGorda);
                        

                        // Se limpia el array y se rellena
                        Array.Clear(tableroOpciones, 0, tableroOpciones.Length);
                        rellenarArrayConSerpiente(tableroOpciones, serpienteDeLasOpciones);

                        // Se dibuja en pantalla y se espera
                        ag.pantallaOpciones(marcador, tableroOpciones, opcionesTemp);

                    }
                }

                // Código copiado vilmente para evitar modularización innecesaria
                // (la única diferencia es el segundo comando)

                // Se actualizan los parámetros de la serpiente y se mueve
                serpienteDeLasOpciones.cambiarParametrosParaOpciones(opcionesTemp.SacaLengua ? opcionesTemp.FrecuenciaSacaLengua : 0, opcionesTemp.EsGorda);
                serpienteDeLasOpciones.moverParaOpciones();

                // Se limpia el array y se rellena
                Array.Clear(tableroOpciones, 0, tableroOpciones.Length);
                rellenarArrayConSerpiente(tableroOpciones, serpienteDeLasOpciones);

                // Se dibuja en pantalla y se espera
                ag.pantallaOpciones(marcador, tableroOpciones, opcionesTemp);
            }

        }

        /// <summary>
        /// Controla la pantalla de puntuaciones
        /// </summary>
        private void puntuaciones()
        {
            int pos = 0;
            bool recarga = true;
            List<Puntuacion> record = null;
            string[,] array = new string[5, 2];

            while (true)
            {
                // Se recarga la pantalla solo cuando haya movimiento de marcador
                if (recarga)
                {
                    record  = modelo.getPuntuaciones(pos);
                    for (int i = 0; i < 5; i++)
                    {
                        if (i >= record.Count)
                        {
                            array[i, 0] = "";
                            array[i, 1] = "[ SIN DATOS ]";
                        }
                        else
                        {
                            // Raro será el caso, pero si los puntos conseguidos tienen más
                            // de 6 cifras, se muestra un string de 6 caracteres para evitar
                            // el descuadre de la pantalla
                            string puntos = record[i].Puntos.ToString();
                            array[i, 0] = puntos.Length > 6 ? "MUCHOS" : puntos.PadLeft(6, ' ');
                            string nombre = record[i].Nombre;
                            array[i, 1] = nombre.Length > 16 ? record[i].Nombre.Substring(0, 16) : nombre;
                        }
                        
                    }
                    recarga = false;
                }
                
                ag.pantallaPuntuaciones(pos, array);

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        if (pos > 0)
                        {
                            pos--;
                            recarga = true;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (pos < 3)
                        {
                            pos++;
                            recarga = true;
                        }
                        break;
                    case ConsoleKey.Enter:
                    case ConsoleKey.Escape:
                        return;
                }
            }
            
        }

        /// <summary>
        /// Controla la pantalla de inicio de parámetros
        /// </summary>
        private void iniciarParametros()
        {
            // String para parseo de números y mensaje de información
            string temp;
            mensaje = "";
            
            // Ancho
            while (true)
            {
                ag.elegirParametro("ancho",anchoMin,anchoMax,mensaje);

                temp = Console.ReadLine();

                if (validadorNum(temp, anchoMin, anchoMax, true))
                {
                    break;
                }
            }

            // Alto
            while (true)
            {
                ag.elegirParametro("alto", altoMin, altoMax, mensaje);

                temp = Console.ReadLine();

                if (validadorNum(temp, altoMin, altoMax, false))
                {
                    break;
                }
            }

            // Dificultad
            int pos = 1;
            while (true)
            {
                ag.elegirDificultad(anchoTablero, altoTablero, pos);

                ConsoleKey tecla = Console.ReadKey().Key;

                if(tecla == ConsoleKey.LeftArrow && pos > 0)
                {
                    pos--;
                }
                else if(tecla == ConsoleKey.RightArrow && pos < 2)
                {
                    pos++;
                }
                else if(tecla == ConsoleKey.Enter)
                {
                    dificultad = pos;
                    if (dificultad == 0)
                    {
                        tiempoBase = 400;
                        tiempoDinamico = 200;
                    }
                    else if(dificultad==1)
                    {
                        tiempoBase = 200;
                        tiempoDinamico = 100;
                    }
                    else
                    {
                        tiempoBase = 100;
                        tiempoDinamico = 60;
                    }

                    break;
                }

            }

        }

        /// <summary>
        /// Comienza la partida y controla su flujo
        /// </summary>
        private void comenzarPartida()
        {
            // Se crean las instancias necesarias para la partida
            tablero = new char[altoTablero, anchoTablero];
            puntos = 0;
            hayManzana = false;
            generaManzanas = new Random();
            direccionElegida = Direccion.Izquierda;

            sierpe = new Sierpe(anchoTablero, altoTablero, direccionElegida, (beanOpciones.SacaLengua ? beanOpciones.FrecuenciaSacaLengua : 0), beanOpciones.EsGorda);
            partidaGanada = false;
            

            while (true)
            {
                // Se limpia el array anterior
                Array.Clear(tablero,0,tablero.Length);

                // Se pone una nueva manzana si no hay
                if (!hayManzana)
                {
                    // Antes de generarla se comprueba si hay espacio
                    // (si no lo hay, es que la partida ha finalizado)
                    if(sierpe.Serpiente.Count >= anchoTablero*altoTablero)
                    {
                        partidaGanada = true;
                        partidaFinalizada();
                        return;
                    }
                    while (true)
                    {
                        // Genera un punto aleatorio en el tablero de juego
                        int xManzana = generaManzanas.Next(anchoTablero);
                        int yManzana = generaManzanas.Next(altoTablero);

                        // Compara el punto generado con el cuerpo de la serpiente
                        // (así no se generará una manzana sobre la serpiente)
                        nuevaManzana = new Punto(xManzana,yManzana);
                        if (!sierpe.Serpiente.Contains(nuevaManzana))
                        {
                            hayManzana = true;
                            break;
                        }
                    }
                }

                // Lee la última tecla pulsada
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.UpArrow:
                            direccionElegida = Direccion.Arriba;
                            break;
                        case ConsoleKey.RightArrow:
                            direccionElegida = Direccion.Derecha;
                            break;
                        case ConsoleKey.DownArrow:
                            direccionElegida = Direccion.Abajo;
                            break;
                        case ConsoleKey.LeftArrow:
                            direccionElegida = Direccion.Izquierda;
                            break;
                        case ConsoleKey.P:
                            ag.dibujarPausa(puntos, tablero);
                            while (true) if (Console.ReadKey(true).Key == ConsoleKey.P) break;
                            break;
                    }
                }

                // Confirma la posición de la cabeza en el siguiente movimiento
                // (en la serpiente se ignorará si se le pasa una dirección
                // opuesta a la que tiene actualmente)
                Punto nuevaCabeza = sierpe.confirmarDireccion(direccionElegida);

                // Si la nueva posición de la cabeza  es una manzana
                if(nuevaCabeza.Equals(nuevaManzana))
                {
                    sierpe.crecer();
                    hayManzana = false;
                    puntos += 50;
                    // Si la manzana es la nota musical, sonará un tono
                    if(beanOpciones.IndiceManzana == 5)
                    {
                        new Thread(() => Console.Beep(554, 300)).Start();
                    }

                }
                // Si la serpiente ha chocado
                else if (serpienteChoca(nuevaCabeza))
                {
                    // Se activa penalización si está en modo fácil,
                    // si no, game over
                    if (muertePorAutofagiaEnModoFacil)
                    {
                        sierpe.autofagia();
                    }
                    else
                    {
                        partidaFinalizada();
                        return;
                    }
                }
                // Si no hay ningún evento, se mueve normalmente
                else
                {
                    sierpe.mover();
                }

                // Se asignan la manzana y la serpiente al array
                // y se deja lo demás vacío
                tablero[nuevaManzana.Y, nuevaManzana.X] = arrayManzana[beanOpciones.IndiceManzana];
                rellenarArrayConSerpiente(tablero, sierpe);

                ag.dibujarFrame(puntos, tablero);

                if (tiempoDinamico > 0) tiempoDinamico--;
                Thread.Sleep(tiempoBase+tiempoDinamico);

            }
        }

        /// <summary>
        /// Rellena un char[,] con la serpiente de parámetro
        /// </summary>
        /// <param name="array"></param>
        /// <param name="serpiente"></param>
        private void rellenarArrayConSerpiente(char[,] array, Sierpe serpiente)
        {
            for (int i = 0; i < serpiente.Serpiente.Count; i++)
            {
                array[serpiente.Serpiente[i].Y, serpiente.Serpiente[i].X] = serpiente.SerpienteDibujo[i];
            }
        }

        /// <summary>
        /// Comprueba si la serpiente de la partida choca con un punto
        /// </summary>
        /// <param name="p">Punto que se quiere comprobar</param>
        /// <returns>Si la serpiente choca o no</returns>
        private bool serpienteChoca(Punto p)
        {
            muertePorAutofagiaEnModoFacil = false;

            // Si choca consigo misma (y no es la cola)
            if (sierpe.Serpiente.Contains(p) && !sierpe.Serpiente[sierpe.Serpiente.Count-1].Equals(p))
            {
                muertePorAutofagiaEnModoFacil = dificultad==0;
                return true;
            }
            // Si se sale del tablero
            if (p.X < 0 || p.X >= anchoTablero || p.Y < 0 || p.Y >= altoTablero)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Controla la pantalla de partida finalizada
        /// </summary>
        private void partidaFinalizada()
        {
            // Puntero
            string nombre = "_";

            // Posición conseguida
            int nuevoRecord;

            // Se comprueba si se ha conseguido algún récord
            if (partidaGanada)
            {
                nuevoRecord = modelo.comprobarPuntuacion(3, puntos);
                ag.animacionVictoria();
            }
            else
            {
                nuevoRecord = modelo.comprobarPuntuacion(dificultad, puntos);
                ag.animacionDerrota();
            }
            
            // Empieza el bucle de opciones
            while (true)
            {
                // Se vuelve a dibujar (sin animación)
                ag.dibujarPartidaFinalizada(partidaGanada, puntos, nuevoRecord, nombre);

                // Se registran eventos de teclado
                ConsoleKeyInfo info = Console.ReadKey(true);
                ConsoleKey pulsada = info.Key;

                // Si no hay récord, ante cualquier evento se vuelve a la pantalla de inicio
                if (nuevoRecord == -1)
                {
                    break;
                }
                // Si se pulsa Enter/Esc, se registra la puntuación (aunque no haya nombre)
                else if(pulsada==ConsoleKey.Enter || pulsada == ConsoleKey.Escape)
                {
                    // Evita nombres vacíos
                    if (nombre.Equals("_")) nombre = "??????";

                    if (partidaGanada)
                    {
                        modelo.guardarPuntuacion(3, puntos, nombre.Substring(0, nombre.Length - 1));
                    }
                    else
                    {
                        modelo.guardarPuntuacion(dificultad, puntos, nombre.Substring(0, nombre.Length - 1));
                    }
                    
                    break;
                }
                // Evento borrar
                else if (pulsada == ConsoleKey.Backspace)
                {
                    if (nombre.Length>1)
                    {
                        if (nombre.Length < 16 || nombre[15] == '_')
                        {
                            nombre = nombre.Substring(0, nombre.Length - 2);
                        }
                        else
                        {
                            nombre = nombre.Substring(0, nombre.Length - 1);
                        }
                        nombre += '_';
                    }
                }
                // Evento pulsar letra
                else
                {
                    if (char.IsLetterOrDigit(info.KeyChar))
                    {
                        nombre = nombre.Substring(0, nombre.Length - 1);
                        nombre += char.ToUpper(info.KeyChar);
                        
                        if (nombre.Length < 16) nombre += '_';
                    }
                }
            }
            
        }

        /// <summary>
        /// Valida un input de texto para comprobar si es una dimensión válida
        /// </summary>
        /// <param name="aConvertir">Input de texto</param>
        /// <param name="min">Dimensión mínima</param>
        /// <param name="max">Dimensión máxima</param>
        /// <param name="esAncho">Si se está comprobando el ancho (o el alto)</param>
        /// <returns>Si el input es válido</returns>
        private bool validadorNum(string aConvertir, int min, int max, bool esAncho)
        {
            // Si no hay input, se elige el número por defecto
            if (string.IsNullOrEmpty(aConvertir))
            {
                if (esAncho) anchoTablero = anchoDef;
                else altoTablero = altoDef;
                mensaje = $"Eligiendo {(esAncho ? "ancho" : "alto")} por defecto ({(esAncho ? anchoDef : altoDef)})";
                return true;
            }

            int intTemp;
            double doubleTemp;

            // Comienza una serie de parseos para averiguar de que se trata el input
            if(int.TryParse(aConvertir,out intTemp))
            {
                if (intTemp < min)
                {
                    mensaje = $"[No se aceptan números menores que {min}]";
                }
                else if (intTemp > max)
                {
                    mensaje = $"[No se aceptan números mayores que {max}]";
                }
                else
                {
                    if (esAncho) anchoTablero = intTemp;
                    else altoTablero = intTemp;
                    mensaje = $"[El {(esAncho ? "ancho" : "alto")} se ha establecido en {intTemp}]";
                    return true;
                }
            }
            else if(double.TryParse(aConvertir,out doubleTemp))
            {
                if((aConvertir.Contains("E") || aConvertir.Contains("e")))
                {
                    mensaje = "[Aún no hemos implementado notación científica]";
                }
                else if(doubleTemp%1 == 0)
                {
                    mensaje = "[Ese número está fuera del rango permitido]";
                }
                else
                {
                    mensaje = "[Aún no contemplamos tableros con casillas decimales]";
                }
            }
            else
            {
                mensaje = "[Debe elegir un número]";
            }
            return false;
        }
    }
}
