using System.Collections.Immutable;
using AdventOfCode_2020_21.Extensions;

namespace AdventOfCode_2020_21;

internal record FoodList(IImmutableList<FoodListEntry> Entries){ 
    
    public static FoodList From(string input)
    {
        var entries = input.Split('\n').Select(line => line.Split('(')).
            Select(split => (split[0].Trim(), split[1].Replace("contains ", "").Clear(")\r,").Trim())).
            Select(split => (split.Item1.Split(" "), split.Item2.Split(" "))).
            Select(split => (split.Item1.Select(item => new Ingredient(item)), split.Item2.Select(item => new Allergen(item)))).
            Select(split => new FoodListEntry(split.Item1.ToImmutableList(), split.Item2.ToImmutableList())).
            ToImmutableList();

        return new FoodList(entries);
    }
};
internal record FoodListEntry(IImmutableList<Ingredient> Ingredients, IImmutableList<Allergen> Allergens){
    public bool Has(Allergen allergen) => Allergens.Contains(allergen);    
};
internal record Ingredient(string Name);
internal record Allergen(string Name);
