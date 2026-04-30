# Sistema Integrado de Produtos e Relatórios (Ecossistema Distribuído)

**Instituição:** Senai Afonso Greco  
**Cidade:** Nova Lima - MG  
**Disciplina:** Desenvolvimento de Sistemas  
**Professor:** Frederico Martins Aguiar  
**Aluno:** Otavio Henrique Barbosa Soares  

---

##  Sobre o Projeto
Este projeto é uma solução completa de software baseada em uma **Arquitetura Distribuída**, desenvolvida como requisito avaliativo máximo da disciplina de Desenvolvimento de Sistemas. O ecossistema é composto por quatro projetos que interagem entre si, focando na gestão de estoque, consumo de serviços web (API) e geração de relatórios gerenciais. O grande destaque desta solução é a **persistência de dados descentralizada**, garantindo que cada ponta do sistema possua seu próprio banco de dados isolado.

##  Tecnologias e Pacotes Utilizados
* **Linguagem:** C# (.NET 8)
* **IDE:** Visual Studio 2022
* **Padrão Arquitetural:** API RESTful + Aplicação Desktop (WPF) + Worker/Console
* **Pacotes NuGet Instalados (Gerenciador de Pacotes):**
  * `Microsoft.EntityFrameworkCore` (ORM base do projeto)
  * `Microsoft.EntityFrameworkCore.Sqlite` (Provedor do banco de dados SQLite)
  * `Microsoft.EntityFrameworkCore.Tools` (Para execução dos comandos *Add-Migration* e *Update-Database*)
  * `Microsoft.EntityFrameworkCore.Design` (Para suporte à criação do banco na API)
  * `System.Net.Http.Json` (Para serialização/desserialização ágil de DTOs nas requisições HTTP)

---

## Estrutura Completa da Solução
A solução foi dividida estrategicamente em 4 subprojetos para garantir a separação de responsabilidades (Separation of Concerns) e alta coesão:

### 1. `Shared` (Biblioteca de Classes)
* **Objetivo:** Armazenar as entidades de domínio compartilhadas.
* **Conteúdo:** Contém a classe base `Produto.cs`, garantindo que regras de negócio fundamentais não precisem ser reescritas em outros projetos.

### 2. `SistemaIntegracaoProdutos.API` (Servidor Web / Backend)
* **Objetivo:** Atuar como o servidor central de processamento.
* **Conteúdo:** Possui a `ProdutosController` que expõe os endpoints HTTP (GET, POST, DELETE). Recebe os dados, valida e salva em seu banco próprio.

### 3. `SistemaInterface` (Aplicativo Desktop WPF)
* **Objetivo:** Ser o painel de controle interativo do usuário.
* **Conteúdo:** Faz o CRUD consumindo a API. Possui as telas `MainWindow` (gerenciamento) e `HistoricoWindow` (visualização de relatórios). Utiliza DTOs para tráfego de rede seguro.

### 4. `SistemaSimulator` (Console Application / Robô)
* **Objetivo:** Simular um ambiente de produção gerando carga de dados.
* **Conteúdo:** Possui serviços dedicados (`LogService`) que geram produtos aleatórios (ex: Mouse, Teclado), fazem o POST para a API de forma automatizada e registram um histórico local dessa operação.

---

##  A Tríplice Persistência (Bancos de Dados SQLite)
O projeto atende à exigência avançada de persistência local em múltiplos pontos. O Entity Framework Core foi configurado com a abordagem *Code-First* para gerar três bancos **SQLite** (`.db`) independentes:

1.  **`produtos.db` (Na API):** O banco de dados mestre. Armazena o cadastro oficial de todos os produtos do ecossistema.
2.  **`wpf_local.db` (No WPF):** O banco local do usuário. Armazena um cache dos produtos manipulados e a tabela `HistoricoRelatorios`, garantindo que os relatórios gerenciais não se percam ao fechar o programa.
3.  **`simulator_log.db` (No Console):** O banco de auditoria. Salva um log de cada item que o simulador gerou e enviou com sucesso para a API.

*(Nota: Em tempo de execução, o SQLite gera arquivos auxiliares `.db-shm` e `.db-wal` para controle de transações, provando que os bancos estão ativos e operantes).*

---

## Boas Práticas e Padrões (Design Patterns)
* **Uso de DTOs (Data Transfer Objects):** Implementamos `ProdutoDTO` e `RelatorioDTO` nas camadas clientes. Isso garante que o tráfego de rede contenha apenas os dados necessários, desacoplando a interface gráfica das entidades complexas de banco de dados.
* **Comunicação Assíncrona:** Toda a comunicação HTTP e chamadas de banco de dados utilizam `async` e `await` (ex: `GetFromJsonAsync`, `SaveChangesAsync`), impedindo o congelamento da interface (UI Thread).
* **Injeção de Dependência:** Uso de instâncias de `DbContext` em blocos `using` para garantir o fechamento das conexões e liberação de memória após o uso.

---

## Guia de Execução (Passo a Passo para Teste)

Para ver o ecossistema completo funcionando e testar todas as persistências, siga este fluxo:

### Passo 1: Inicialização Múltipla
1. Abra a solução no Visual Studio 2022.
2. Clique com o botão direito na **Solução 'SistemaIntegracaoProdutos'** > **Configurar Projetos de Inicialização...**.
3. Marque **"Vários projetos de inicialização"** e defina a "Ação" como **Iniciar** para a API, o WPF (SistemaInterface) e o Console (SistemaSimulator).
4. Pressione **F5**.

### Passo 2: Testando o Ecossistema
1. **Verificando a API:** O navegador abrirá a tela do *Swagger*. A API já está pronta recebendo requisições.
2. **Verificando o Simulador:** Uma janela preta (Console) abrirá gerando produtos aleatórios. Você verá a mensagem de que os itens foram gerados, enviados para a API e **salvos no banco local do Console (`simulator_log.db`)**.
3. **Verificando o WPF (Desktop):**
   * A janela gráfica abrirá. Clique em **Atualizar Lista** para ver os itens que o Console acabou de injetar na API.
   * Cadastre um item manualmente pela tela. O sistema avisará que o item foi salvo na API e no **banco local do WPF (`wpf_local.db`)**.
   * Clique em **Excluir Selecionado** para testar o método DELETE.

### Passo 3: O Sistema de Relatórios (A Cereja do Bolo)
1. Na tela do WPF, clique no botão **Gerar Relatório**.
2. O sistema fará o cálculo total de itens em estoque e o valor financeiro acumulado.
3. Clique no botão **Ver Histórico**. Uma nova janela (`HistoricoWindow`) será aberta consultando a tabela `HistoricoRelatorios` no banco local, provando a **persistência definitiva do relatório**.

---

##  Considerações Finais
Este projeto vai muito além de um simples CRUD. A implementação de uma arquitetura baseada em microsserviços (API, Worker, Client), a utilização correta do Entity Framework Core com SQLite em múltiplos contextos, e o tratamento de requisições HTTP assíncronas com DTOs demonstram maturidade técnica. O sistema é coeso, seguro e atende a 100% dos requisitos acadêmicos estipulados para a disciplina.
