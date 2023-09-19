using PokeAPI.ErrorsMessage;
using PokeAPI.Models;

public class PokemonService
{
    private readonly PokemonRepository _pokemonRepository;
    private readonly MestrePokemonRepository _mestrePokemonRepository;

    public PokemonService(PokemonRepository pokemonRepository, MestrePokemonRepository mestrePokemonRepository)
    {
        _pokemonRepository = pokemonRepository;
        _mestrePokemonRepository = mestrePokemonRepository;
    }

    public async Task<Pokemon> GetPokemonByNameAsync(string name)
    {
        try
        {
            return await _pokemonRepository.GetPokemonEvolutionsByNameAsync(name.ToLower());
        }
        catch (HttpRequestException ex)
        {
            // Handle exception or log it
            throw ex;
        }
    }

    public async Task<List<PokemonListResult>> GetRandomTenPokemonsListAsync()
    {
        try
        {
            return await _pokemonRepository.GetRandomTenPokemonsListAsync();
        }
        catch (HttpRequestException ex)
        {
            // Handle exception or log it
            throw ex;
        }
    }

    public void CapturePokemon(MestrePokemon mestrePokemon, PokemonListResult capturedPokemon)
    {
        var captured = new CapturedPokemon
        {
            MestrePokemonId = mestrePokemon.Id,
            PokemonName = capturedPokemon.Name,
            capturedDate = DateTime.Now,
            BackDefault = capturedPokemon.sprites.back_default
        };

        _pokemonRepository.CreateCapturedPokemon(captured);

        mestrePokemon.NumberCaptured++;
        _mestrePokemonRepository.UpdateMestrePokemon(mestrePokemon);
    }
}
