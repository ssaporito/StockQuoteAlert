# StockQuoteAlert

Esta é uma aplicação que monitora preços de ativos selecionados e gera alertas de acordo com os valores configurados. Há duas versões: uma que busca simular uma infraestrutura real, baseada em Docker e RabbitMQ, e uma monolítica por linha de comando, para testes e ambientes simplificados.

## Requisitos

- .NET 6.0 ou superior
- Docker (para a versão com microsserviços)

## Configuração
Ambas as versões utilizam appsettings.json para configuração. Na versão monolítica, há apenas um appsettings a ser definido, com a seguinte estrutura:

    {
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft.Hosting.Lifetime": "Information"
        }
      },
      "MailInfo": {
        "SenderName": "Stock Quote Alert", // Nome que aparece nos alertas enviados por e-mail
        "SenderEmail": "noreply@stockquotealert.com", // Remetente dos alertas
        "RecipientName": "ThatPerson", // Nome do destinatário dos alertas
        "RecipientEmail": "recipient@somewhere.com" // E-mail de destino dos alertas
      },
      "StockApiSettings": {
        "Endpoint": "https://www.alphavantage.co/query", // Endpoint da API utilizada
        "ApiKey": "demo" // Chave de API do Alpha Vantage
      },
      "SmtpSettings": {
        "Server": "smtp.example.com", // Servidor SMTP escolhido
        "Port": 587, // Porta do servidor
        "Username": "username", // Usuário do servidor
        "Password": "password" // Senha do servidor
      }
    }

Como as chaves de API da Alpha Vantage possuem limitações de uso, a chave "demo" é oferecida para alguns valores específicos (exemplos em https://www.alphavantage.co/documentation/#latestprice) e pode ser usada em testes limitados. Para utilização em ambientes reais, uma assinatura paga superior seria recomendada, dado que o monitoramento é executado a cada 5 segundos por padrão.

## Execução por Linha de Comando

1. Abra um terminal na pasta que contém o arquivo `StockAlertAppSimple.exe`.

2. Execute o comando `.\StockAlertAppSimple.exe [STOCK] [SELL] [BUY]`, substituindo `[STOCK]`, `[SELL]` e `[BUY]` pelos valores desejados. Por exemplo: `.\StockAlertAppSimple.exe IBM 143.35 143.2`.

## Execução pelo Visual Studio

A solução aberta no Visual Studio permite selecionar a forma de execução com simplicidade, além de permitir configurar variáveis de ambiente ou argumentos para linha de comando. 

Para execução da versão de microsserviços, é necessário selecionar docker-compose como o projeto de início (necessário Docker instalado). Neste caso, é possível passar os argumentos através do arquivo docker-compose.yml, como a seguir:

    stockalertservice:
        ... outras configurações
        environment:
          - STOCK=NOME_DO_ATIVO
          - SELL=VALOR_DE_VENDA
          - BUY=VALOR_DE_COMPRA
        ... outras configurações
      ...

Para a versão monolítica, basta escolher o projeto StockAlertAppSimple como ponto de início e configurar suas propriedades de debug com os argumentos em sequência:

    NOME_DO_ATIVO VALOR_DE_VENDA VALOR_DE_COMPRA

## Observações
- A API utilizada para a cotação é a do Alpha Vantage, que utiliza um sufixo para ativos fora dos EUA (ex.: a ação PETR4 é indexada como PETR4.SA), e este deve ser incluído no primeiro argumento do programa (ex.: .\StockAlertAppSimple.exe **PETR4.SA** 22.67 22.59).
- No modo Docker Compose, o aplicativo será iniciado junto com um servidor RabbitMQ, que é necessário para o seu funcionamento.

- STOCK se refere ao nome da ação a ser monitorada.
- SELL se refere ao preço de venda desejado para a ação.
- BUY se refere ao preço de compra desejado para a ação.