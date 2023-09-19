using Dapper;
using Microsoft.Data.Sqlite;
using PokeAPI.Models;

public class MestrePokemonRepository
{
    private readonly string _connectionString;

    public MestrePokemonRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public MestrePokemon GetMestrePokemonByName(string name)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var sql = "SELECT * FROM MestrePokemon WHERE Name = @Name";
            var mestrePokemon = connection.QueryFirstOrDefault<MestrePokemon>(sql, new { Name = name });

            return mestrePokemon;
        }
    }
    
    public void UpdateMestrePokemon(MestrePokemon mestrePokemon)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var sql = "UPDATE MestrePokemon " +
                      "SET NumberCaptured = @NumberCaptured " +
                      "WHERE Id = @Id";
            connection.Execute(sql, new
            {
                Id = mestrePokemon.Id,
                NumberCaptured = mestrePokemon.NumberCaptured
            });
        }
    }

    public void CreateMestrePokemon(MestrePokemon mestrePokemon)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var sql = "INSERT INTO MestrePokemon (Name, Age, Password) VALUES (@Name, @Age, @Password)";
            connection.Execute(sql, mestrePokemon);
        }
    }

    public IEnumerable<MestrePokemon> GetAllMestrePokemon()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var sql = "SELECT * FROM MestrePokemon";
            var mestrePokemonList = connection.Query<MestrePokemon>(sql);

            return mestrePokemonList;
        }
    }

    public MestrePokemon GetMestrePokemonById(int id)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var sql = "SELECT * FROM MestrePokemon WHERE Id = @Id";
            var mestrePokemon = connection.QueryFirstOrDefault<MestrePokemon>(sql, new { Id = id });

            return mestrePokemon;
        }
    }

    public void DeleteMestrePokemonById(int id)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var sql = "DELETE FROM MestrePokemon WHERE Id = @Id";
            connection.Execute(sql, new { Id = id });
        }
    }
}
