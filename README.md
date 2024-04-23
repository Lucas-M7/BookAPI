# API de Cadastro de Usuários e Livros 📚

Esta é uma API desenvolvida para gerenciar o cadastro de usuários e livros. Ela permite o registro de dois tipos de usuários: administradores (ADM) e usuários comuns (Common). Além disso, utiliza Token JWT para autenticação e autorização durante o processo de login.

<a href="https://ibb.co/Z1FfWp0"><img src="https://i.ibb.co/5xS1BgP/Foto-1.png" alt="Foto-1" border="0" /></a>

## Funcionalidades ⚙️

- Cadastro, leitura, atualização e exclusão de usuários
- Cadastro, leitura, atualização e exclusão de livros
- Autenticação de usuários via JWT

## Tecnologias Utilizadas 🧑‍💻

- C#
- ASP.NET Core
- Entity Framework
- JSON Web Tokens (JWT)
- SQL Server

## Pré-requisitos ❗

Antes de iniciar, certifique-se de ter instalado em sua máquina:

- .NET
- SQL Server

## Rotas da API 🛣️

### Usuários 👤

- `POST /api/users/`: Cadastrar um novo usuário. Requer um corpo JSON contendo `name`, `email`, `password` e `profile` (ADM ou Common).
- `POST /api/users/login`: Autenticar um usuário. Requer um corpo JSON contendo `email` e `password`. Retorna um token JWT válido por um 1 dia.
- `GET /api/users/`: Retorna todos os perfis cadastrados com uma paginação.
- `GET /api/users/{id}/`: Retorna o usuário especificado pelo ID.
- `PUT /api/users/{id}/`: Atualizar o perfil do usuário autenticado.
- `DELETE /api/users/`: Excluir o perfil do usuário autenticado.
### Livros 📚

- `POST /api/`: Cadastrar um novo livro. Requer um corpo JSON contendo `title`, `category`, `author`, `daterelease` e `username`.
- `GET /api/`: Obter todos os livros cadastrados com paginação.
- `GET /api/{id}`: Obter detalhes de um livro específico por ID.
- `PUT /api/{id}`: Atualizar os detalhes de um livro específico por ID. Requer um corpo JSON contendo os campos a serem atualizados.
- `DELETE /api/{id}`: Excluir um livro específico por ID.

## Contribuição 🤝

Contribuições são bem-vindas! Sinta-se à vontade para abrir problemas (issues) ou enviar pull requests com melhorias.
