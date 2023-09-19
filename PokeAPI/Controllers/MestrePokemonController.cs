using System.Security.Claims;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using PokeAPI.Models;


[Route("api/mestrepokemon")]
public class MestrePokemonController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly MestrePokemonService _mestrePokemonService;
    private readonly PokemonService _pokemonService;
    private readonly string _connectionString;
    private readonly PokemonRepository _pokemonRepository;


    public MestrePokemonController(MestrePokemonService mestrePokemonService, AuthService authService,
        PokemonService pokemonService, PokemonRepository pokemonRepository, string connectionString)
    {
        _connectionString = connectionString;
        _mestrePokemonService = mestrePokemonService;
        _pokemonService = pokemonService;
        _authService = authService;
        _pokemonRepository = pokemonRepository;
    }

    private bool AutenticacaoBemSucedida(string Name, string Password)
    {
        var mestrePokemon = _mestrePokemonService.GetMestrePokemonByName(Name);

        if (mestrePokemon != null) return BCrypt.Net.BCrypt.Verify(Password, mestrePokemon.Password);

        return false;
    }

    [HttpPost]
    [Route("login")]
    public IActionResult Login([FromBody] LoginRequest model)
    {
        var user = _mestrePokemonService.GetMestrePokemonByName(model.Name);

        if (user != null && AutenticacaoBemSucedida(model.Name, model.Password))
        {
            var token = _authService.GenerateJwtToken(model.Name);
            return Ok(new { Token = token });
        }

        return Unauthorized();
    }

    [HttpGet]
    [Route("getByName/{name}")]
    public IActionResult GetMestrePokemonByName(string name)
    {
        var query = $@"
        SELECT cp.*, mp.Name AS NomeMestrePokemon
        FROM captured_pokemon cp
        JOIN MestrePokemon mp ON cp.MestrePokemonId = mp.Id
        WHERE mp.Name = @MestrePokemonName;
    ";

        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            // Execute a consulta SQL e mapeie os resultados para objetos.
            var capturedPokemonDetails
                = connection.Query<CapturedPokemon>(query, new { MestrePokemonName = name }).ToList();
            var response = new { PokemonsCapturados = capturedPokemonDetails };
            
            return Ok(response);
        }
    }

    [HttpPost]
    [Route("create")]
    public IActionResult CreateMestrePokemon([FromBody] MestrePokemon mestrePokemon)
    {
        if (mestrePokemon == null) return BadRequest();

        if (string.IsNullOrEmpty(mestrePokemon.Password)) return BadRequest("A senha é obrigatória.");

        _mestrePokemonService.CreateMestrePokemon(mestrePokemon, mestrePokemon.Password);

        mestrePokemon.Password = mestrePokemon.Password;

        return CreatedAtAction("GetMestrePokemonByName", new { name = mestrePokemon.Name }, mestrePokemon);
    }

    [HttpGet]
    [Route("getAll")]
    public IActionResult GetAllMestrePokemon()
    {
        var mestrePokemonList = _mestrePokemonService.GetAllMestrePokemon();
        return Ok(mestrePokemonList);
    }

    [HttpDelete]
    [Route("delete/{id}")]
    public IActionResult DeleteMestrePokemon(int id)
    {
        try
        {
            _mestrePokemonService.DeleteMestrePokemonById(id);
            return NoContent();
        }
        catch
        {
            return NotFound();
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetMestrePokemonById(int id)
    {
        var mestrePokemon = _mestrePokemonService.GetMestrePokemonById(id);

        if (mestrePokemon != null) return Ok(mestrePokemon);

        return NotFound();
    }

    [HttpPost]
    [Route("capturePokemon")]
    [Authorize]
    public IActionResult CapturePokemon()
    {
        try
        {
            // Obtém o usuário autenticado a partir do contexto do JWT
            var user = HttpContext.User;

            // Obtém o nome do usuário a partir das claims do token
            string username = user.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Nome do usuário não encontrado no token.");
            }

            var randomPokemons = _pokemonRepository.GetRandomTenPokemonsListAsync().Result;

            var random = new Random();
            if (random.Next(1, 11) <= 9) // 90% de chance de sucesso
            {
                var mestrePokemon = _mestrePokemonService.GetMestrePokemonByName(username);

                if (mestrePokemon == null)
                {
                    return BadRequest("MestrePokemon não encontrado para o usuário atual.");
                }

                var capturedPokemon = randomPokemons[random.Next(0, 10)];

                // Captura o Pokémon
                _pokemonService.CapturePokemon(mestrePokemon, capturedPokemon);

                return Ok(new { Message = "Você capturou um Pokémon!", CapturedPokemon = capturedPokemon });
            }
            else
            {
                return Ok(new { Message = "A captura falhou. Tente novamente." });
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
