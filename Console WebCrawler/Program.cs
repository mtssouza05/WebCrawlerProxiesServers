using Console_WebCrawler.Servicos;
using System;
using System.Diagnostics;

Stopwatch sw = Stopwatch.StartNew();


Console.WriteLine("Executando Crawler...");

sw.Start();

await new Crawler().ExecutaCrawler();

sw.Stop();

Console.WriteLine($"Programa executado em: {sw.Elapsed.TotalSeconds} segundos");
Console.WriteLine($"Localização dos arquivos json: {Directory.GetCurrentDirectory()}\\JsonGerados");
Console.WriteLine($"Localização dos arquivos html: {Directory.GetCurrentDirectory()}\\paginasGeradas");
Console.WriteLine($"Localização do Banco: {Directory.GetCurrentDirectory()}\\WebCrawlerDb.db");
Console.WriteLine("Pressione qualquer tecla para encerrar");
Console.ReadLine();



