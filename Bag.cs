using System.Collections.Generic;
using System.Text.RegularExpressions;

public record Bag
{
    public string Attribute { get; }
    public string Color { get; }
    public Dictionary<Bag, int> Holds { get; } = new Dictionary<Bag, int>();
    public List<Bag> HeldBy { get; } = new List<Bag>();

    public Bag(Match match)
    {
        Attribute = match.Attribute();
        Color = match.Color();
    }

    public override string ToString() => $"{Attribute} {Color}";
}