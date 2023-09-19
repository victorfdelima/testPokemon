CREATE TABLE IF NOT EXISTS MestrePokemon
CREATE TABLE IF NOT EXISTS MestrePokemon (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Age INTEGER,
    Password TEXT NOT NULL,
    NumberCaptured INT NULL
);

 CREATE TABLE IF NOT EXISTS captured_pokemon
CREATE TABLE IF NOT EXISTS captured_pokemon (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    MestrePokemonId INTEGER NOT NULL,
    PokemonName TEXT NOT NULL,
    capturedDate DATETIME NULL,
    BackDefault TEXT,
    FOREIGN KEY (MestrePokemonId) REFERENCES MestrePokemon(Id)
);