using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Snake
{
    /// <summary>
    /// Lector y escritor de XML que hace las veces de modelo
    /// </summary>
    class Modelo
    {
        private static readonly string rutaPuntos = @"Persistencia\puntos.xml";
        private static readonly string rutaOpciones = @"Persistencia\opciones.xml";

        /// <summary>
        /// Lee el XML especificado
        /// </summary>
        /// <param name="ruta">Ruta del XML</param>
        /// <returns>Documento</returns>
        private XmlDataDocument getXML(string ruta)
        {
            XmlDataDocument doc = new XmlDataDocument();
            FileStream fs = new FileStream(ruta, FileMode.Open, FileAccess.Read);
            doc.Load(fs);
            fs.Close();
            return doc;
        }

        /// <summary>
        /// Devuelve las puntuaciones guardadas
        /// </summary>
        /// <param name="dificultad">Dificultad de la cual consultar las puntuaciones (0-2) o 3 para ganadores</param>
        /// <returns>Lista de puntuaciones de esa dificultad</returns>
        public List<Puntuacion> getPuntuaciones(int dificultad)
        {
            XmlDataDocument doc = getXML(rutaPuntos);
            XmlNodeList listXmlModo;

            // Guarda el índice que buscamos de la lista de nodos
            int accedido=-1;

            listXmlModo = doc.GetElementsByTagName("modo");
            for (int i = 0; i < listXmlModo.Count; i++)
            {
                if (listXmlModo[i].Attributes.GetNamedItem("dif").InnerText.Equals($"{dificultad}"))
                {
                    accedido = i;
                    break;
                }
            }
            
            // Si el índice que buscamos no está, lanza excepción
            if (accedido == -1) throw new XmlException();

            List<Puntuacion> retorna = new List<Puntuacion>();
            try
            {
                // Parsea todos los resultados
                for (int i = 0; i < listXmlModo[accedido].ChildNodes.Count; i++)
                {
                    Puntuacion p = new Puntuacion();
                    p.Puntos = int.Parse(listXmlModo[accedido].ChildNodes[i].ChildNodes[0].InnerText);
                    p.Nombre = listXmlModo[accedido].ChildNodes[i].ChildNodes[1].InnerText;
                    retorna.Add(p);
                }

                // Se devuelven los resultados (truncados hasta 5 y ordenados)
                retorna.Sort((p, q) => q.Puntos.CompareTo(p.Puntos));
                if (retorna.Count > 5) return retorna.GetRange(0, 5);
                return retorna;

            }
            catch (FormatException)
            {
                throw new XmlException();
            }

        }

        /// <summary>
        /// Comprueba si una puntuación dada entraría en los puestos más altos
        /// </summary>
        /// <param name="dificultad">Dificultad en la que se ha jugado</param>
        /// <param name="puntos">Puntos conseguidos</param>
        /// <returns>El puesto que ocuparía, o -1 si no ocupa ninguno</returns>
        public int comprobarPuntuacion(int dificultad, int puntos)
        {
            List<Puntuacion> score = getPuntuaciones(dificultad);

            if (score.Count == 0) return 1;

            if (puntos <= score[score.Count - 1].Puntos)
            {
                return -1;
            }
            else
            {
                for (int i = 0; i < score.Count; i++)
                {
                    if (puntos >= score[i].Puntos) return i + 1;
                }
                return -1;
            }
        }

        /// <summary>
        /// Guarda una puntuación entre las más altas
        /// </summary>
        /// <param name="dificultad">Dificultad en la que se ha jugado (3 para ganadores)</param>
        /// <param name="puntos">Puntos conseguidos</param>
        /// <param name="nombre">Nombre del jugador</param>
        public void guardarPuntuacion(int dificultad, int puntos, string nombre)
        {
            // Se recupera la lista de puntuaciones
            List<Puntuacion> lista = getPuntuaciones(dificultad);

            // Se añade la nueva puntuación y se ordenan todas
            Puntuacion nuevo = new Puntuacion();
            nuevo.Puntos = puntos;
            nuevo.Nombre = nombre;
            lista.Add(nuevo);
            lista.Sort((p, q) => q.Puntos.CompareTo(p.Puntos));

            XmlDataDocument doc = getXML(rutaPuntos);

            // Se busca el tag de modo que coincida con esta dificultad
            XmlNodeList modo = doc.GetElementsByTagName("modo");
            XmlNode esteModo=null;
            foreach(XmlNode n in modo)
            {
                if (n.Attributes.GetNamedItem("dif").InnerText.Equals($"{dificultad}"))
                {
                    esteModo = n;
                    break;
                }
            }

            // Si no existe, se lanza error
            if (esteModo == null) throw new XmlException();

            // Se borran todas las etiquetas hijas
            while (esteModo.FirstChild != null) esteModo.RemoveChild(esteModo.FirstChild);

            // Se limitan las puntuaciones guardadas a 5
            int numDatos = lista.Count<5 ? lista.Count : 5;

            // Se vuelven a rellenar con los nuevos datos
            for(int i=0; i < numDatos; i++)
            {
                XmlNode n = esteModo.OwnerDocument.CreateElement("puntuacion");
                esteModo.AppendChild(n);

                XmlNode nodoPuntos = n.OwnerDocument.CreateElement("puntos");
                nodoPuntos.InnerText = lista[i].Puntos.ToString();
                XmlNode nodoNombre = n.OwnerDocument.CreateElement("nombre");
                nodoNombre.InnerText = lista[i].Nombre;

                n.AppendChild(nodoPuntos);
                n.AppendChild(nodoNombre);
            }

            doc.Save(rutaPuntos);

        }

        /// <summary>
        /// Devuelve las opciones guardadas
        /// </summary>
        /// <returns>Bean de opciones</returns>
        public BeanOpciones getOpciones()
        {
            XmlDataDocument xml = getXML(rutaOpciones);
            BeanOpciones retorna = new BeanOpciones();
            retorna.EsGorda = xml.GetElementsByTagName("esGorda")[0].InnerText == "1";
            retorna.SacaLengua = xml.GetElementsByTagName("sacaLengua")[0].InnerText == "1";
            retorna.FrecuenciaSacaLengua = int.Parse(xml.GetElementsByTagName("frecuenciaSacaLengua")[0].InnerText);
            retorna.IndiceManzana = int.Parse(xml.GetElementsByTagName("indiceManzana")[0].InnerText);

            return retorna;
        }

        /// <summary>
        /// Guarda las opciones
        /// </summary>
        /// <param name="b">Bean de opciones</param>
        public void guardaOpciones(BeanOpciones b)
        {
            XmlDataDocument xml = getXML(rutaOpciones);
            xml.GetElementsByTagName("esGorda")[0].InnerText = b.EsGorda ? "1" : "0";
            xml.GetElementsByTagName("sacaLengua")[0].InnerText = b.SacaLengua ? "1" : "0";
            xml.GetElementsByTagName("frecuenciaSacaLengua")[0].InnerText = b.FrecuenciaSacaLengua.ToString();
            xml.GetElementsByTagName("indiceManzana")[0].InnerText = b.IndiceManzana.ToString();

            xml.Save(rutaOpciones);
        }
    }
}
