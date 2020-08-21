# Game Server

Este projeto é uma API que Salva o resultado de jogos e recupera um leadboard para cada jogo.

## Tecnologias

- Aspnet core 3.1
- EntityFramework Core

> Download do [dotnet 3.1][1]
### Instalação

> Ambiente [windows][2]
>
> Ambiente [Linux][3]
>
> Ambiente [MacOS][4]

O Projeto está compatível com SQLServer, porém, ele está preparado para ampliar a compatibilidade com outros bancos que estão listado [aqui][7].

---

## Para Executar

 Clone o projeto

```bash
git clone https://github.com/tgspn/GameServer.git
cd GameServer
```

 O projeto está implementado em Aspnet core, é possível executá-lo diretamente no console, assim, como no linux e macos.

### No Visual studio

Abra o projeto com o Visual Studio e faça as alterações no arquivo de [configuração][0] de acordo com as instruções.

Execute o projeto com F5 ou aperte para iniciar a depuração.

### Console
Para executar o projeto utilizando apenas o console,

Entre na pasta do Projeto

```bash
cd GameServer
```

e execute com o seguinte comando

```dotnetcli
dotnet run
```


> **Atenção**
>
> Quando deixar de utilizar o banco de dados padrão, é necessário realizar a [migração][6] antes de executar o projeto.

---
## Configuração

É possivel fazer algumas customizações, para isso é utilizado o arquivo [appsettings.json][5]

### ConnectionString

É necessário alterar a `connectionString` para apontar o banco corretamente. 
> O nome da conexão é `ApplicationDbContext` e o valor é a connection string do banco.

### Alterar o banco utilizado

É possivel alterar o banco utilizado apenas trocando o valor da chave `"databaseType"`
> Atualmente o único banco compatível com o sistema é o SQLServer

> O banco padrão é o `InMemory`, quando não há nenhum outro configurado o banco padrão é utilizado.

|Padrão| Banco|      Valor     |           Observação|
|---|---------------------|:--------------:|:----:|
|   | SQLServer           |   `SQLServer`  |                                 |
|*  | Em Memoria          |   `InMemory`   | Tem a mesma função de um Banco de dados porém é direcionado apenas para teste. |

---
## Migração

O sistema utiliza ferramenta de mapeamento de objeto relacional (ORM) Entity Framework Core, com ela fica muito mais simples o mapeamento dos registro do banco de dado dentro da programação, pois as entidades são mapeadas como objetos.

No Entity Framework é possível utilizar de duas formas, *Code First* e *Database First*. Na primeira opção o mapeamento é feito no código e é gerado a base a partir do código, já na segunda opção é montada toda a base e depois é feito o mapeamento no sistema.

Neste projeto fora utilizado a implementação *Code First*, desta forma é necessário executar o comando de migração, para que a base seja gerada.

No visual studio
```azurepowershell
Update-Database
```
No Console

```dotnetcli
dotnet ef database update
```

[0]: #configuração
[1]: https://dotnet.microsoft.com/download
[2]: https://docs.microsoft.com/pt-br/dotnet/core/install/windows?tabs=netcore31
[3]: https://docs.microsoft.com/pt-br/dotnet/core/install/linux
[4]: https://docs.microsoft.com/pt-br/dotnet/core/install/macos
[5]: ./GameServer/appsettings.json
[6]: #migração
[7]: https://docs.microsoft.com/pt-br/ef/core/providers/?tabs=dotnet-core-cli