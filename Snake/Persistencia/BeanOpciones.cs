using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class BeanOpciones
    {
        private bool esGorda, sacaLengua;
        private int frecuenciaSacaLengua, indiceManzana;
        private char manzana;

        public bool EsGorda { get => esGorda; set => esGorda = value; }
        public bool SacaLengua { get => sacaLengua; set => sacaLengua = value; }
        public int FrecuenciaSacaLengua { get => frecuenciaSacaLengua; set => frecuenciaSacaLengua = value; }
        public char Manzana { get => manzana; set => manzana = value; }
        public int IndiceManzana { get => indiceManzana; set => indiceManzana = value; }
    }
}
