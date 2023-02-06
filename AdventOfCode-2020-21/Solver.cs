using System.Collections.Immutable;
using AdventOfCode_2020_21.Extensions;

namespace AdventOfCode_2020_21
{
    internal static class Solver
    {

        public static IImmutableList<Ingredient> GetCanonicalDangerousIngredientList(FoodList foodList)
        {
            var solution = Solve(ProblemInstance.From(foodList));
            var withAllergens = GetIngredientsWithAllergens(foodList, solution);
            return withAllergens.OrderBy(ingredient => solution.AllergensByIngredient[ingredient].Name).ToImmutableList();
        }

        public static int GetAmountOfIngredientsWithoutAllergens(FoodList foodList)
        {
            var withAllergens = GetIngredientsWithAllergens(foodList);
            return foodList.Entries.Select(entry => entry.Ingredients.Except(withAllergens).Count()).Sum();
        }

        public static IEnumerable<Ingredient> GetDistinctIngredientsWithoutAllergens(FoodList foodList)
        {
            var solution = Solve(ProblemInstance.From(foodList));
            return GetDistinctIngredientsWithoutAllergens(foodList, solution);
        }

        internal static SolutionInstance Solve(ProblemInstance problem)
        {
            var reducedProblemInstance = CanonicalReduction(problem);
            return new SolutionInstance(reducedProblemInstance.Determined);
        }

        internal record ProblemInstance(IImmutableDictionary<Ingredient, Allergen> Determined, IImmutableList<FoodListEntry> UndeterminedEntries)
        {
            public static ProblemInstance From(FoodList foodList) => new(ImmutableDictionary.Create<Ingredient, Allergen>(), foodList.Entries);
        };

        internal record SolutionInstance(IImmutableDictionary<Ingredient, Allergen> AllergensByIngredient);

        private static IEnumerable<Ingredient> GetDistinctIngredientsWithoutAllergens(FoodList foodList, SolutionInstance solution)
        {
            return foodList.Entries.
                SelectMany(entry => entry.Ingredients).
                Where(ingredient => !solution.AllergensByIngredient.ContainsKey(ingredient)).
                Distinct().
                ToArray();
        }

        private static IImmutableList<Ingredient> GetIngredientsWithAllergens(FoodList foodList, SolutionInstance solution)
        {
            var withoutAllergens = GetDistinctIngredientsWithoutAllergens(foodList, solution);
            return foodList.Entries.
                SelectMany(entry => entry.Ingredients).
                Except(withoutAllergens).
                Distinct().
                ToImmutableList();
        }

        private static IImmutableList<Ingredient> GetIngredientsWithAllergens(FoodList foodList)
        {
            var solution = Solve(ProblemInstance.From(foodList));
            return GetIngredientsWithAllergens(foodList, solution);
        }

        private static IImmutableDictionary<Allergen, IImmutableList<Ingredient>> GetPotentialsFrom(ProblemInstance problem){
            
            var allergens = problem.UndeterminedEntries.SelectMany(entry => entry.Allergens).Distinct().ToImmutableList();
            
            var potentialIngredientsByAllergen = allergens.ToDictionary(
                allergen => allergen,
                allergen => (IImmutableList<Ingredient>)
                    problem.UndeterminedEntries.Where(entry => entry.Has(allergen)).
                    Select(entry => entry.Ingredients).
                    IntersectAll().
                    ToImmutableList()
            );
            
            return potentialIngredientsByAllergen.ToImmutableDictionary();
        }

        private static ProblemInstance CanonicalReduction(ProblemInstance problem)
        {
            while (true)
            {
                var potentials = GetPotentialsFrom(problem);

                var newlyDeterminedByValue = potentials.
                    Where(kv => kv.Value.Count == 1).
                    Select(kv => (kv.Key, kv.Value.Single())).
                    ToImmutableDictionary(kv => kv.Item2, kv => kv.Key);

                if (!newlyDeterminedByValue.Any())
                {
                    return problem;
                }

                problem = SetAsGiven(problem, newlyDeterminedByValue);
            }
        }

        private static ProblemInstance SetAsGiven(ProblemInstance problem ,IImmutableDictionary<Ingredient, Allergen> newlyDeterminedByValue){ 
            var newlyDeterminedAllergens = newlyDeterminedByValue.Values.ToImmutableHashSet();
            var newlyDeterminedIngredients = newlyDeterminedByValue.Keys.ToImmutableHashSet();

            foreach(var (key,value) in problem.Determined)
                newlyDeterminedByValue = newlyDeterminedByValue.Add(key, value);

            var newFoodEntries = problem.UndeterminedEntries.
                Select(entry => new FoodListEntry(
                    entry.Ingredients.Except(newlyDeterminedIngredients).ToImmutableList(),
                    entry.Allergens.Except(newlyDeterminedAllergens).ToImmutableList()
                    )
                ).
                Where(entry => entry.Allergens.Any() && entry.Ingredients.Any()).
                ToImmutableList();

            return new ProblemInstance(newlyDeterminedByValue.ToImmutableDictionary(), newFoodEntries);
        }

    }
}
