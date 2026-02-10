# UsuariosApp

UsuariosApp é uma API desenvolvida em **.NET 10**, projetada para gerenciamento de usuários com foco em **autenticação**, **segurança** e **arquitetura em camadas**.  
O projeto segue boas práticas de desenvolvimento corporativo, utilizando **JWT**, **Entity Framework Code First** e **testes de integração**.

---

## 🏗️ Arquitetura do Projeto

O projeto está organizado em camadas bem definidas:

- **UsuariosApp.API**  
  Camada de apresentação responsável por expor os endpoints HTTP, autenticação JWT e configuração da aplicação.

- **UsuariosApp.Domain**  
  Contém as entidades, regras de negócio e contratos (interfaces).  
  Não possui dependência de infraestrutura.

- **UsuariosApp.Infra.Data**  
  Responsável pelo acesso a dados, implementações de repositórios e configuração do Entity Framework Code First.

- **UsuariosApp.Tests**  
  Contém testes de integração da API utilizando **xUnit** e **FluentAssertions**.

---

## 🔐 Funcionalidades

A API possui três serviços principais:

1. **Autenticar Usuário**
   - Geração de token JWT
   - Validação de credenciais

2. **Criar Usuário**
   - Cadastro de novos usuários
   - Persistência via Entity Framework

3. **Obter Dados do Usuário**
   - Retorno de informações do usuário autenticado
   - Protegido por autenticação JWT

---

## 🛠️ Tecnologias Utilizadas

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core (Code First)
- JWT (JSON Web Token)
- xUnit
- FluentAssertions
- Testes de Integração
- Swagger / Scalar

---

## 🧪 Testes

Foram implementados **testes de integração na API**, validando:

- Fluxo de autenticação
- Criação de usuários
- Acesso a endpoints protegidos
- Respostas HTTP e regras de negócio

Ferramentas utilizadas:
- **xUnit**
- **FluentAssertions**

---

## 🚀 Executando o Projeto

1. Clone o repositório:
```bash
git clone <url-do-repositorio>
```

2. Configure a string de conexão no `appsettings.json`

3. Execute as migrações (se aplicável)

4. Inicie a aplicação:
```bash
dotnet run
```

---

## 📘 Documentação da API

A documentação interativa da API está disponível em:

🔗 https://usuariosapp-sergio-ardgh2dbfregdzf6.canadacentral-01.azurewebsites.net/scalar/v1

---

## 📌 Observações

- Projeto desenvolvido com foco educacional e arquitetural
- Ideal para estudos de:
  - Arquitetura em camadas
  - Autenticação com JWT
  - Testes de integração em APIs .NET

---

## 👨‍💻 Autor

**Sergio Mendes**  
Professor e Desenvolvedor .NET
