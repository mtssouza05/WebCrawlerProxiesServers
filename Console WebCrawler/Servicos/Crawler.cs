using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Data.SQLite;
using Console_WebCrawler.DTO;
using Console_WebCrawler.Data;
using System.Data.Entity.Migrations.Model;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Console_WebCrawler.Servicos
{
    public class Crawler
    {
        public const string urlBase = "https://proxyservers.pro/proxy/list/order/updated/order_dir/desc";


        public async Task ExecutaCrawler()
        {
            int paginaNum = 0, linhaCont = 0;
            DateTime dataInicio = DateTime.Now;
            List<Dictionary<string, string>> dadosExtraidos = new List<Dictionary<string, string>>();
            SemaphoreSlim semaforo = new SemaphoreSlim(3);
            List<Task> tasks = new List<Task>();

            bool flagContinuaCrawl = true;
            object lockObj = new object();

            while (flagContinuaCrawl)
            {
                semaforo.Wait();
                paginaNum++;

                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        string url = $"{urlBase}/page/{paginaNum}";
                        string conteudoHtml = await BaixarPaginaAsync(url);

                        if (string.IsNullOrWhiteSpace(conteudoHtml))
                        {
                            lock (lockObj)
                            {
                                flagContinuaCrawl = false;
                            }
                            return;
                        }
                        
                        var dadosPagina = ExtrairDadosHtml(conteudoHtml);

                        if (dadosPagina == null || dadosPagina.Count == 0)
                        {
                            

                            lock (lockObj)
                            {
                                flagContinuaCrawl = false;
                            }
                            return;
                        }
                        else
                        {
                            SalvaPaginaHtml(conteudoHtml, paginaNum);
                        }

                        lock (dadosExtraidos)
                        {
                            dadosExtraidos.Clear();
                            dadosExtraidos.AddRange(dadosPagina);
                            linhaCont = dadosPagina.Count;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro no crawler: {ex.Message}");
                    }
                    finally
                    {
                        semaforo.Release();
                    }
                }));

                await Task.WhenAll(tasks);



                if (flagContinuaCrawl)
                {
                    string arquivoJson = SalvaJson(dadosExtraidos);

                    SalvaLog(new LogExecucaoItem()
                    {
                        DataInicio = dataInicio,
                        DataFim = DateTime.Now,
                        NumPagina = paginaNum,
                        QtdLinhas = linhaCont,
                        JsonCaminho = arquivoJson
                    });
                }


            }
        }

        public async Task<string> BaixarPaginaAsync(string url)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var resposta = await httpClient.GetAsync(url);

                    if (!resposta.IsSuccessStatusCode)
                        return null;

                    return await resposta.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao fazer download da pagina: {ex.Message}");
                return null;
            }
        }
        public void SalvaPaginaHtml(string conteudo, int numPagina)
        {
            string diretorio = Path.Combine(Directory.GetCurrentDirectory(), "paginasGeradas");

            if (!Directory.Exists(diretorio))
                Directory.CreateDirectory(diretorio);

            string arquivo = Path.Combine(diretorio, $"pagina_{numPagina}_{DateTime.Now:yyyyMMddHHmmss}.html");
            File.WriteAllText(arquivo, conteudo);
        }
        public List<Dictionary<string, string>> ExtrairDadosHtml(string conteudo)
        {
            List<Dictionary<string, string>> dados = new List<Dictionary<string, string>>();

            HtmlDocument docHtml = new HtmlDocument();
            docHtml.LoadHtml(conteudo);
            var tabela = docHtml.DocumentNode.SelectSingleNode("//div//table[contains(@class, 'table-hover')]");

            if (tabela == null)
                return dados;

            var linhas = tabela.SelectNodes(".//tbody//tr");

            if (linhas == null)
                return dados;

            foreach (var linha in linhas)
            {
                var colunas = linha.SelectNodes("td");

                var portTd = colunas[2].SelectSingleNode(".//span[@class='port']");
                string portDados = portTd.GetAttributeValue("data-port", string.Empty);

                if (colunas == null)
                    continue;

                Dictionary<string, string> item = new Dictionary<string, string>
                {
                    ["IP Address"] = colunas[1].InnerText.Trim(),
                    ["Port"] = portDados,
                    ["Country"] = colunas[3].InnerText.Trim(),
                    ["Protocol"] = colunas[6].InnerText.Trim()
                };

                dados.Add(item);
            }

            return dados;
        }
        public string SalvaJson(List<Dictionary<string, string>> jsonData)
        {
            string diretorio = Path.Combine(Directory.GetCurrentDirectory(), "JsonGerados");

            if (!Directory.Exists(diretorio))
                Directory.CreateDirectory(diretorio);

            string arquivo = Path.Combine(diretorio, $"json_proxie_{DateTime.Now:yyyyMMddHHmmss}.json");
            File.WriteAllText(arquivo, JsonSerializer.Serialize(jsonData));

            return arquivo;
        }
        public void SalvaLog(LogExecucaoItem logItem)
        {
            Conexao conexao = new Conexao();
            conexao.SalvaExecucao(logItem);
        }
    }
}
