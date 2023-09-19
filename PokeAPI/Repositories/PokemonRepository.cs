using System.Net.Http.Headers;
using Dapper;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using PokeAPI.ErrorsMessage;

public class PokemonRepository
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private readonly string _connectionString;

    public PokemonRepository(IConfiguration configuration, string connectionString)
    {
        _connectionString = connectionString;
        _httpClient = new HttpClient();
        var ApiBaseUrl = configuration["ConnectionAPI:BaseUrl"];
        _httpClient.BaseAddress = new Uri(ApiBaseUrl);
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        _configuration = configuration;
    }

    public async Task<Pokemon> GetPokemonEvolutionsByNameAsync(string name)
    {
        try
        {
            var response = await _httpClient.GetAsync($"pokemon/{name.ToLower()}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var pokemon = JsonConvert.DeserializeObject<Pokemon>(json);
            var pokemonId = pokemon.Id;

            var evolutionResponse = await _httpClient.GetAsync($"evolution-chain/{pokemonId}");
            evolutionResponse.EnsureSuccessStatusCode();
            var evolutionJson = await evolutionResponse.Content.ReadAsStringAsync();

            var evolutionChain = JsonConvert.DeserializeObject<EvolutionChain>(evolutionJson);

            pokemon.Evolutions = new List<Chain> { evolutionChain.Chain };

            AddEvolutionsToChain(pokemon.Evolutions);

            return pokemon;
        }
        catch (HttpRequestException ex)
        {
            // Trate a exceção ou a registre
            throw ex;
        }
    }

    private void AddEvolutionsToChain(List<Chain> chains)
    {
        foreach (var chain in chains)
            if (chain.evolves_to != null && chain.evolves_to.Any())
                AddEvolutionsToChain(chain.evolves_to);
    }


    public void CreateCapturedPokemon(CapturedPokemon capturedPokemon)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var sql = "INSERT INTO captured_pokemon (MestrePokemonId, PokemonName, CapturedDate, BackDefault) " +
                      "VALUES (@MestrePokemonId, @PokemonName, @CapturedDate, @BackDefault)";
            connection.Execute(sql,
                new
                {
                    MestrePokemonId = capturedPokemon.MestrePokemonId,
                    PokemonName = capturedPokemon.PokemonName,
                    CapturedDate = capturedPokemon.capturedDate,
                    BackDefault = capturedPokemon.BackDefault
                });

            // Atualize o número de Pokémon capturados do mestre Pokémon.
            var updateSql = "UPDATE MestrePokemon SET NumberCaptured = NumberCaptured + 1 " +
                            "WHERE Id = @MestrePokemonId";
            connection.Execute(updateSql, new
            {
                MestrePokemonId = capturedPokemon.MestrePokemonId
            });
        }
    }

    public async Task<List<PokemonListResult>> GetRandomTenPokemonsListAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("pokemon?limit=10000");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<PokemonListResponse>(json);

            var totalPokemonCount = result.Results.Count;

            var random = new Random();
            var randomOffset = random.Next(0, totalPokemonCount - 10);

            var tenPokemons = result.Results.Skip(randomOffset).Take(10).ToList();

            foreach (var pokemon in tenPokemons)
            {
                response = await _httpClient.GetAsync(pokemon.Url);
                response.EnsureSuccessStatusCode();
                json = await response.Content.ReadAsStringAsync();
                var detailedPokemon = JsonConvert.DeserializeObject<PokemonListResult>(json);
                pokemon.sprites = detailedPokemon.sprites;
            }

            return tenPokemons;
        }
        catch (HttpRequestException ex)
        {
            throw ex;
        }
    }
}


public class Evolution
{
    public Chain Chain { get; set; }
    public bool IsBaby { get; set; }
    public Species Species { get; set; }
}
