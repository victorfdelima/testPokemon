public class Pokemon
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public List<Abilityy> Abilities { get; set; }
    
    public SpriteSprites Sprites { get; set; }
    public List<Chain> Evolutions { get; set; }
}

public class EvolutionChain
{
    public Chain Chain { get; set; }
}

public class Chain
{
    public List<Chain> evolves_to { get; set; }
    public bool IsBaby { get; set; }
    public Species Species { get; set; }
}

public class Species
{
    public string Name { get; set; }
    public string Url { get; set; }
}

public class Trigger
{
    public string Name { get; set; }
    public string Url { get; set; }
}


public class Abilityy
{
    public AbilityDetail Ability { get; set; }
    public bool IsHidden { get; set; }
    public int Slot { get; set; }
}

public class AbilityDetail
{
    public string Name { get; set; }
    public string Url { get; set; }
}

public class SpriteSprites
{
    public string back_default { get; set; }
}

public class PokemonListResult
{
    public string Name { get; set; }
    public string Url { get; set; }
    
    public DateTime CapturedDate { get; set; } = DateTime.Now;
    public SpriteSprites sprites { get; set; }
}

public class PokemonListResponse
{
    public List<PokemonListResult> Results { get; set; }
}
