# Pokemon Capture App

Este é um projeto de aplicativo para capturar Pokémon e armazená-los em um banco de dados SQLite. O aplicativo permite que você crie mestres Pokémon e registre os Pokémon capturados por cada mestre. O banco de dados SQLite é usado para armazenar informações sobre mestres Pokémon e os Pokémon capturados.

## Configuração do Banco de Dados SQLite

Para configurar o banco de dados SQLite para este projeto, siga estas etapas:

1. Abra o arquivo `appsettings.json` na raiz do projeto.

2. Dentro do arquivo `appsettings.json`, você encontrará configurações para diferentes ambientes, como `Development`, `Production`, etc. Certifique-se de configurar a string de conexão para o banco de dados SQLite em cada ambiente.

   Exemplo:

   `json
   "ConnectionStrings": {
       "DefaultConnection": "Data Source=local/path/to/your/database.db"
   }`

3. Substitua "local/path/to/your/database.db" pelo caminho absoluto ou relativo para o arquivo de banco de dados SQLite que você deseja usar.

- Certifique-se de que a string de conexão esteja corretamente configurada para cada ambiente em que deseja executar o aplicativo.

## Criação das Tabelas do Banco de Dados

Para criar as tabelas do banco de dados necessárias para este projeto, você pode usar os seguintes scripts SQL:

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

Você pode executar esses scripts em um cliente SQLite ou utilizar alguma ferramenta de gerenciamento de banco de dados SQLite para criar as tabelas (Recomendo Dbeaver).

## Executando o Aplicativo

Após configurar o banco de dados e criar as tabelas, você pode executar o aplicativo. Certifique-se de configurar o ambiente apropriado no arquivo appsettings.json para que o aplicativo use a string de conexão correta.

Para iniciar o aplicativo, abra um terminal na pasta do projeto e execute os seguintes comandos:

`dotnet restore
dotnet build
dotnet run`

O aplicativo será iniciado e estará disponível em http://localhost:5000 (ou outro endereço e porta, dependendo da configuração).


