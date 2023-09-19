using PokeAPI.Models;

public class MestrePokemonService
{
    private readonly MestrePokemonRepository _repository;

    public MestrePokemonService(MestrePokemonRepository repository)
    {
        _repository = repository;
    }

    public MestrePokemon GetMestrePokemonByName(string name)
    {
        return _repository.GetMestrePokemonByName(name);
    }

    public void CreateMestrePokemon(MestrePokemon mestrePokemon, string plainTextPassword)
    {
        mestrePokemon.Password = BCrypt.Net.BCrypt.HashPassword(plainTextPassword);
        _repository.CreateMestrePokemon(mestrePokemon);
    }

    public IEnumerable<MestrePokemon> GetAllMestrePokemon()
    {
        return _repository.GetAllMestrePokemon();
    }

    public void DeleteMestrePokemonById(int id)
    {
        _repository.DeleteMestrePokemonById(id);
    }

    public MestrePokemon GetMestrePokemonById(int id)
    {
        return _repository.GetMestrePokemonById(id);
    }
}
