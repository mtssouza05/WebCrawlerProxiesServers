# Proxies Crawler
Este projeto é um Web Crawler que coleta dados de proxies listados na grid no endereço:
https://proxyservers.pro/proxy/list/order/updated/order_dir/desc.

Os dados capturados são:

* IP Address
* Port
* Country
* Protocol
  
Além disso, o programa salva os seguintes arquivos na pasta de saída:

* Arquivos JSON contendo os dados coletados de cada página.
* Arquivos HTML de cada página processada.
* Logs das operações persistidos em um banco SQLite.
## Informações Técnicas
* Banco de Dados:
    O banco utilizado é o SQLite, escolhido para facilitar e agilizar o desenvolvimento.
    Após a primeira execução, o arquivo do banco (WebCrawlerDb.db) será gerado automaticamente na pasta bin/debug/net8.0. Neste banco, são armazenados os logs de cada execução do programa.

* Tecnologias Utilizadas:

    * Plataforma: .NET 8
    * Pacotes:
        * HtmlAgilityPack
        * System.Data.SQLite
## Gerenciamento de Threads
O programa é multithreaded e utiliza a classe SemaphoreSlim para controlar o número de threads em execução simultânea.

## Saída dos Arquivos
Os arquivos gerados durante a execução do programa serão armazenados nas seguintes pastas, localizadas dentro do diretório bin:

* JsonGerados: Contém os arquivos .json com os dados extraídos.
* paginasGeradas: Contém os arquivos .html de cada página processada.
## Execução e Resultado
Após a execução do programa, o console exibirá:

* O tempo total de execução.
* Os diretórios onde os arquivos de saída foram gerados.
## Como Executar
Certifique-se de ter o .NET 8 instalado na máquina.
Compile e execute o projeto.
Os arquivos gerados estarão disponíveis na pasta bin/debug/net8.0.
