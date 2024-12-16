using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_WebCrawler.DTO
{
    public class LogExecucaoItem
    {
        public int Id { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int NumPagina { get; set; }
        public int QtdLinhas { get; set; }
        public string JsonCaminho { get; set; } = string.Empty;
    }
}
