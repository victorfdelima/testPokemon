using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokeAPI.Models;

public class MestrePokemon
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name { get; set; }

    public int Age { get; set; }

    public string Password { get; set; }

    public int NumberCaptured { get; set; }
    
    public List<CapturedPokemon> CapturedPokemon { get; set; }
}
