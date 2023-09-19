using Microsoft.AspNetCore.Mvc;

namespace PokeAPI.Controllers;

[Route("api/pokemon")]
public class PokemonController : ControllerBase
{
    private readonly PokemonService _pokemonService;

    public PokemonController(PokemonService pokemonService)
    {
        _pokemonService = pokemonService;
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetPokemonByName(string name)
    {
        try
        {
            var pokemon = await _pokemonService.GetPokemonByNameAsync(name);

            if (pokemon != null)
                return Ok(pokemon);
            return NotFound(); // Pokemon não encontrado
        }
        catch (HttpRequestException ex)
        {
            return BadRequest($"Erro ao buscar o Pokemon: {ex.Message}");
        }
    }


    [HttpGet]
    public async Task<IActionResult> GetRandomTenPokemons()
    {
        try
        {
            var results = await _pokemonService.GetRandomTenPokemonsListAsync();
            return Ok(results);
        }
        catch (HttpRequestException ex)
        {
            return BadRequest($"Erro ao buscar os Pokémon: {ex.Message}");
        }
    }
}
