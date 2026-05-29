# Lab Prótese

Sistema web para gerenciamento de pedidos entre **dentistas** e **protéticos**, desenvolvido em ASP.NET Core MVC com .NET 10 e Entity Framework Core + SQL Server.

---

## Funcionalidades

- **Autenticação** — cadastro e login separados para Dentistas (CRO) e Protéticos (CNPJ), com sessão HTTP
- **Serviços** — protéticos cadastram e gerenciam os serviços oferecidos (valor, descrição)
- **Carrinho** — dentistas adicionam serviços ao carrinho antes de confirmar o pedido
- **Pedidos** — criação de pedidos a partir do carrinho, com controle de itens, valor total e status
- **Status do pedido** — ciclo de vida: `Pendente → EmProducao → Concluido → Entregue / Recolhido`
- **Coleta e Entrega** — modelos de logística associados a cada pedido
- **Perfil** — atualização e exclusão de conta pelo próprio usuário

---

## Tecnologias

| Camada | Tecnologia |
|---|---|
| Framework | ASP.NET Core MVC (.NET 10) |
| ORM | Entity Framework Core 8 |
| Banco de dados | SQL Server (LocalDB / SQL Express) |
| Front-end | Razor Views + Bootstrap 5 + jQuery |
| Sessão | `ISession` nativa do ASP.NET Core |

---

## Estrutura do Projeto

```
Lab_Protese/
├── Controllers/
│   ├── AuthController.cs       # Login, Cadastro, Update, Delete, Logout
│   ├── CarrinhoController.cs   # Adicionar/remover itens do carrinho
│   ├── PedidoController.cs     # CRUD de pedidos + atualização de status
│   └── ServicoController.cs    # CRUD de serviços
├── Data/
│   ├── AppDbContext.cs          # Contexto do EF Core
│   └── Lista.cs                 # Estado em memória do carrinho
├── Enums/
│   └── StatusPedido.cs          # Pendente, EmProducao, Concluido, Entregue, Recolhido
├── Migrations/                  # Migrations geradas pelo EF Core
├── Models/
│   ├── Pessoa.cs / Dentista.cs / Protetico.cs
│   ├── Servico.cs
│   ├── Pedido.cs / ItemPedido.cs
│   ├── Endereco.cs
│   ├── Entrega.cs / Coleta.cs / Entregador.cs
├── ViewModels/Auth/
│   ├── CadastroViewModel.cs
│   └── LoginViewModel.cs
├── Views/                       # Razor Views (Auth, Carrinho, Pedidos, Servicos, Shared)
├── wwwroot/                     # CSS, JS, Bootstrap, jQuery
├── appsettings.json             # Configurações de produção (connection string)
└── Program.cs                   # Entry point e configuração do pipeline
```

---

## Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server ou SQL Server Express (LocalDB também funciona)

---

## Configuração e execução

### 1. Clone o repositório

```bash
git clone <url-do-repositorio>
cd Lab_Protese
```

### 2. Configure a connection string

Edite `appsettings.json` (ou crie `appsettings.Development.json`) com os dados do seu banco:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=LabProtese;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 3. Aplique as Migrations

```bash
dotnet ef database update
```

### 4. Execute a aplicação

```bash
dotnet run
```

Acesse em `https://localhost:<porta>` — a rota padrão redireciona para a tela de Login.

---

## Banco de dados

O modelo usa **herança por tabela** (TPT) para `Pessoa`:

- `Pessoas` — tabela base (Id, Nome, Email, Senha, Telefone, Endereco)
- `Dentistas` — herda de Pessoa, adiciona `Cro`
- `Proteticos` — herda de Pessoa, adiciona `Cnpj`

A chave composta de `ItemPedido` é `(PedidoId, ServicoId)`.

---

## Observações

- O carrinho é mantido em uma lista estática em memória (`Lista.Carrinho`). Em produção, considere substituir por sessão ou banco de dados.
- As senhas são armazenadas em texto plano. Recomenda-se aplicar hashing (ex.: BCrypt ou `PasswordHasher` do ASP.NET Identity) antes de um deploy real.
- A autenticação é baseada em sessão simples, sem ASP.NET Identity ou JWT.
