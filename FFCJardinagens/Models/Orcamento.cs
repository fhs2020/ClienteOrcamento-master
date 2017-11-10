using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FFCJardinagens.Models
{
    public class Orcamento
    {
        public int ID { get; set; }
        public int ClienteID { get; set; }
        public String ClienteNome { get; set; }
        public int TotalOrcamentoID { get; set; }
        public TotalOrcamento TotalOrcamento { get; set; }
        public int Quantidade { get; set; }
        public String Descriminação { get; set; }
        public int ProdutoUnidade { get; set; }
        public decimal ProdutoTotal { get; set; }
        public decimal? ValorTotal { get; set; }
        public DateTime? DataOrcamento { get; set; }
    }
}