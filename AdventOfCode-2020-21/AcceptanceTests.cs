using System.Collections.Immutable;
using Xunit;
using FluentAssertions;
using AdventOfCode_2020_21.Extensions;

namespace AdventOfCode_2020_21;

using static Solver;

public class ExampleTests
{
    private const string Example1Input = 
@"mxmxvkd kfcds sqjhc nhms (contains dairy, fish)
trh fvjkl sbzzf mxmxvkd (contains dairy)
sqjhc fvjkl (contains soy)
sqjhc mxmxvkd sbzzf (contains fish)";

    [Fact]
    public void InputString_FoodListFrom_FoodList()
    {
        var expected = new[] { 
            new FoodListEntry(
            ImmutableList.Create(
                new Ingredient("mxmxvkd"),
                    new Ingredient("kfcds"),
                    new Ingredient("sqjhc"),
                    new Ingredient("nhms")
                ),
                ImmutableList.Create(
                    new Allergen("dairy"),
                    new Allergen("fish")
                )
            )
        };
        var foodList = FoodList.From("mxmxvkd kfcds sqjhc nhms (contains dairy, fish)");

        foodList.Entries.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Example1_GetIngredientsWithoutAllergens_List()
    {
        var expected = new[] { "kfcds", "nhms", "sbzzf", "trh" }.Select(name => new Ingredient(name));
        var foodList = FoodList.From(Example1Input);

        var actual = GetDistinctIngredientsWithoutAllergens(foodList);

        actual.Should().Contain(expected);
    }

    [Fact]
    public void Example1_GetAmountOfIngredientsWithoutAllergens_Amount()
    {
        const int expected = 5;
        var foodList = FoodList.From(Example1Input);

        var actual = GetAmountOfIngredientsWithoutAllergens(foodList);

        actual.Should().Be(expected);
    }

    [Fact]
    public void Sets_IntersectAll_Intersection()
    {
        var expected = new[] { "mxmxvkd" };
        var sets = new[] { 
            new[]{ "mxmxvkd", "kfcds", "sqjhc", "nhms" },
            new[]{ "trh", "fvjkl", "sbzzf", "mxmxvkd" },
        };

        var actual = sets.IntersectAll();

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void String_Clear_CharsRemoved()
    {
        const string expected = "bdf";
        const string seed = "abcdefg";

        var actual = seed.Clear("aceg");

        actual.Should().BeEquivalentTo(expected);
    }

    
    [Fact]
    public void Puzzle1()
    {
        const int expected = 1977;
        var foodList = FoodList.From(File.ReadAllText("puzzle-input.txt"));

        var actual = GetAmountOfIngredientsWithoutAllergens(foodList);

        actual.Should().Be(expected);
    }

    [Fact]
    public void FoodList_SolveProblem_AllergenLookup()
    {
        var foodList = FoodList.From(Example1Input);

        var actual = Solve(ProblemInstance.From(foodList));

        actual.AllergensByIngredient[new Ingredient("mxmxvkd")].Should().Be(new Allergen("dairy"));
        actual.AllergensByIngredient[new Ingredient("sqjhc")].Should().Be(new Allergen("fish"));
        actual.AllergensByIngredient[new Ingredient("fvjkl")].Should().Be(new Allergen("soy"));
    }

    [Fact]
    public void FoodList_GetCanonicalDangerousIngredientList_CanonicalDangerousIngredientList()
    {
        var expected = new[] { new Ingredient("mxmxvkd"), new Ingredient("sqjhc"), new Ingredient("fvjkl") };
        var foodList = FoodList.From(Example1Input);

        var actual = GetCanonicalDangerousIngredientList(foodList);

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Puzzle2()
    {
        const string expected = "dpkvsdk,xmmpt,cxjqxbt,drbq,zmzq,mnrjrf,kjgl,rkcpxs";
        var foodList = FoodList.From(File.ReadAllText("puzzle-input.txt"));

        var actual = string.Join(",", GetCanonicalDangerousIngredientList(foodList).Select(ingredient => ingredient.Name));

        actual.Should().BeEquivalentTo(expected);
    }
}