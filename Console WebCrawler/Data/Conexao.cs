using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_WebCrawler.Data.Banco;
using Console_WebCrawler.DTO;

namespace Console_WebCrawler.Data
{
    class Conexao
    {
        public Conexao()
        {
            if (!Path.Exists(BancoHelper.RetornaCaminhoDb())){
                BancoHelper.CriaBanco();
            }

        }


        public void SalvaExecucao(LogExecucaoItem logExecucao)
        {
            try
            {
                using (var con = new SQLiteConnection(BancoHelper.RetornaStringConexao()))
                {
                    con.Open();

                    string insereLog = "INSERT INTO LogExecucao (DataInicio, DataFim, NumPagina, QtdLinhas, JsonCaminho) " +
                        "VALUES (@dataInicio, @dataFim, @numPagina, @qtdLinhas, @jsonCaminho);";

                    using (var cmd = new SQLiteCommand(insereLog, con))
                    {
                        cmd.Parameters.AddWithValue("@dataInicio", logExecucao.DataInicio);
                        cmd.Parameters.AddWithValue("@dataFim", logExecucao.DataFim);
                        cmd.Parameters.AddWithValue("@numPagina", logExecucao.NumPagina);
                        cmd.Parameters.AddWithValue("@qtdLinhas", logExecucao.QtdLinhas);
                        cmd.Parameters.AddWithValue("@jsonCaminho", logExecucao.JsonCaminho);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Erro ao salvar log: {ex.Message}");
            }
           
        }

    }
}
