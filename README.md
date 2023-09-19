# Pokemon Capture App

Este � um projeto de aplicativo para capturar Pok�mon e armazen�-los em um banco de dados SQLite. O aplicativo permite que voc� crie mestres Pok�mon e registre os Pok�mon capturados por cada mestre. O banco de dados SQLite � usado para armazenar informa��es sobre mestres Pok�mon e os Pok�mon capturados.

## Configura��o do Banco de Dados SQLite

Para configurar o banco de dados SQLite para este projeto, siga estas etapas:

1. Abra o arquivo `appsettings.json` na raiz do projeto.

2. Dentro do arquivo `appsettings.json`, voc� encontrar� configura��es para diferentes ambientes, como `Development`, `Production`, etc. Certifique-se de configurar a string de conex�o para o banco de dados SQLite em cada ambiente.

   Exemplo:

   `json
   "ConnectionStrings": {
       "DefaultConnection": "Data Source=local/path/to/your/database.db"
   }`

3. Substitua "local/path/to/your/database.db" pelo caminho absoluto ou relativo para o arquivo de banco de dados SQLite que voc� deseja usar.

- Certifique-se de que a string de conex�o esteja corretamente configurada para cada ambiente em que deseja executar o aplicativo.

## Cria��o das Tabelas do Banco de Dados

Para criar as tabelas do banco de dados necess�rias para este projeto, voc� pode usar os seguintes scripts SQL:

-- CREATE TABLE IF NOT EXISTS MestrePokemon
CREATE TABLE IF NOT EXISTS MestrePokemon (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Age INTEGER,
    Password TEXT NOT NULL,
    NumberCaptured INT NULL
);

-- CREATE TABLE IF NOT EXISTS captured_pokemon
CREATE TABLE IF NOT EXISTS captured_pokemon (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    MestrePokemonId INTEGER NOT NULL,
    PokemonName TEXT NOT NULL,
    capturedDate DATETIME NULL,
    BackDefault TEXT,
    FOREIGN KEY (MestrePokemonId) REFERENCES MestrePokemon(Id)
);

Voc� pode executar esses scripts em um cliente SQLite ou utilizar alguma ferramenta de gerenciamento de banco de dados SQLite para criar as tabelas (Recomendo Dbeaver).

## Executando o Aplicativo

Ap�s configurar o banco de dados e criar as tabelas, voc� pode executar o aplicativo. Certifique-se de configurar o ambiente apropriado no arquivo appsettings.json para que o aplicativo use a string de conex�o correta.

Para iniciar o aplicativo, abra um terminal na pasta do projeto e execute os seguintes comandos:

`dotnet restore
dotnet build
dotnet run`

O aplicativo ser� iniciado e estar� dispon�vel em http://localhost:5000 (ou outro endere�o e porta, dependendo da configura��o).


