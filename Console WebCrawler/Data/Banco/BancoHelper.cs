using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace Console_WebCrawler.Data.Banco
{
    static class BancoHelper
    {

        private static string CaminhoDb = Path.Combine(Directory.GetCurrentDirectory(), "WebCrawlerDb.db"); 
        public static void CriaBanco()
        {
            SQLiteConnection.CreateFile(CaminhoDb);
            string conString = $"DataSource={CaminhoDb};Version=3;";


            using (var con = new SQLiteConnection(conString))
            {
                con.Open();

                string criaTabela = @"
                    CREATE TABLE IF NOT EXISTS LogExecucao (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    DataInicio DATETIME NOT NULL,
                    DataFim DATETIME NOT NULL,
                    NumPagina INTEGER NOT NULL,
                    QtdLinhas INTEGER NOT NULL,
                    JsonCaminho TEXT NOT NULL
                    );
                ";


                using (var cmd = new SQLiteCommand(criaTabela, con))
                {
                    cmd.ExecuteNonQuery();
                }
            }


          
        }

        public static string RetornaStringConexao() => $"DataSource={CaminhoDb};Version=3;";
        public static string RetornaCaminhoDb() => CaminhoDb;
    }
}
