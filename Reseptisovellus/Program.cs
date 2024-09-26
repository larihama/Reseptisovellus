using System;
using System.Collections.Generic;
using System.Linq;

namespace Reseptisovellus
{
    public abstract class Item
    {
        public string Name { get; set; }
        public string Instructions { get; set; }

        public Item(string name, string instructions)
        {
            Name = name;
            Instructions = instructions;
        }

        public abstract void Display();
    }

    public class Recipe : Item
    {
        public string Category { get; set; }
        public List<string> Ingredients { get; set; }
        public string DietaryInfo { get; set; }

        public Recipe(string name, string category, List<string> ingredients, string instructions, string dietaryInfo)
            : base(name, instructions)
        {
            Category = category;
            Ingredients = ingredients;
            DietaryInfo = dietaryInfo;
        }

        public override void Display()
        {
            Console.WriteLine($"Resepti: {Name}");
            Console.WriteLine($"Ruokalaji: {Category}");
            Console.WriteLine($"Ainesosat: {string.Join(", ", Ingredients)}");
            Console.WriteLine($"Valmistusohjeet: {Instructions}");
            Console.WriteLine($"Ruokavalio: {DietaryInfo}");
        }

        public void DisplayIngredients()
        {
            Console.WriteLine($"Ainesosat reseptille '{Name}':");
            foreach (var ingredient in Ingredients)
            {
                Console.WriteLine($"- {ingredient}");
            }
        }

        public void DisplayStepByStepInstructions()
        {
            Console.WriteLine($"Vaihe vaiheelta -ohjeet reseptille '{Name}':");
            var steps = Instructions.Split('.');
            for (int i = 0; i < steps.Length; i++)
            {
                Console.WriteLine($"Vaihe {i + 1}: {steps[i].Trim()}");
            }
        }

        public bool MatchesIngredients(List<string> ingredients)
        {
            return ingredients.All(i => Ingredients.Contains(i, StringComparer.OrdinalIgnoreCase));
        }

        public bool MatchesCategory(string category)
        {
            return string.Equals(Category, category, StringComparison.OrdinalIgnoreCase);
        }

        public bool MatchesDietaryInfo(string dietaryInfo)
        {
            return string.Equals(DietaryInfo, dietaryInfo, StringComparison.OrdinalIgnoreCase);
        }
    }

    public class MainCourseRecipe : Recipe
    {
        public MainCourseRecipe(string name, List<string> ingredients, string instructions, string dietaryInfo)
            : base(name, "pääruoka", ingredients, instructions, dietaryInfo) { }

        public override void Display()
        {
            Console.WriteLine("### Pääruoka ###");
            base.Display();
        }
    }

    public class DessertRecipe : Recipe
    {
        public DessertRecipe(string name, List<string> ingredients, string instructions, string dietaryInfo)
            : base(name, "jälkiruoka", ingredients, instructions, dietaryInfo) { }

        public override void Display()
        {
            Console.WriteLine("### Jälkiruoka ###");
            base.Display();
        }
    }

    public class RecipeBook
    {
        private List<Recipe> recipes = new List<Recipe>();

        public void AddRecipe(Recipe recipe)
        {
            recipes.Add(recipe);
            Console.WriteLine("Resepti lisätty onnistuneesti!");
        }

        public void DisplayAllRecipes()
        {
            for (int i = 0; i < recipes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {recipes[i].Name}");
            }
        }

        public Recipe GetRecipeByIndex(int index)
        {
            if (index < 1 || index > recipes.Count)
            {
                return null;
            }
            return recipes[index - 1];
        }

        public List<Recipe> GetRecipes()
        {
            return recipes;
        }

        public void SearchByIngredients(List<string> ingredients)
        {
            var foundRecipes = recipes.Where(r => r.MatchesIngredients(ingredients)).ToList();
            DisplaySearchResults(foundRecipes);
        }

        public void SearchByCategory(string category)
        {
            var foundRecipes = recipes.Where(r => r.MatchesCategory(category)).ToList();
            DisplaySearchResults(foundRecipes);
        }

        public void SearchByDietaryInfo(string dietaryInfo)
        {
            var foundRecipes = recipes.Where(r => r.MatchesDietaryInfo(dietaryInfo)).ToList();
            DisplaySearchResults(foundRecipes);
        }

        private void DisplaySearchResults(List<Recipe> foundRecipes)
        {
            if (foundRecipes.Any())
            {
                foreach (var recipe in foundRecipes)
                {
                    recipe.Display();
                    Console.WriteLine("-----------------------");
                }
            }
            else
            {
                Console.WriteLine("Reseptejä ei löytynyt.");
            }
        }
    }

    public class AdminUser
    {
        private RecipeBook recipeBook;

        public AdminUser(RecipeBook recipeBook)
        {
            this.recipeBook = recipeBook;
        }

        public void Start()
        {
            while (true)
            {
                ShowMainMenu();
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("");
                        recipeBook.DisplayAllRecipes();
                        break;
                    case "2":
                        AddRecipe();
                        break;
                    case "3":
                        SearchByIngredients();
                        break;
                    case "4":
                        SearchByCategory();
                        break;
                    case "5":
                        SearchByDietaryInfo();
                        break;
                    case "6":
                        Console.WriteLine("Palataan päävalikkoon.");
                        return;
                    default:
                        Console.WriteLine("Virheellinen valinta, yritä uudelleen.");
                        break;
                }
            }
        }

        private void ShowMainMenu()
        {
            Console.WriteLine("");
            Console.WriteLine("1. Näytä kaikki reseptit");
            Console.WriteLine("2. Lisää uusi resepti");
            Console.WriteLine("3. Hae reseptejä ainesosien perusteella");
            Console.WriteLine("4. Hae reseptejä ruokalajin perusteella");
            Console.WriteLine("5. Hae reseptejä ruokavalion perusteella");
            Console.WriteLine("6. Lopeta ohjelma ja palaa käyttäjävalikkoon");
            Console.Write("Syötä valintasi: ");
        }

        private void AddRecipe()
        {
            Console.Write("Syötä reseptin nimi: ");
            var name = Console.ReadLine();

            Console.Write("Syötä ruokalaji (esim. pääruoka, jälkiruoka): ");
            var category = Console.ReadLine();

            Console.Write("Syötä ainesosat (erota pilkulla): ");
            var ingredients = Console.ReadLine().Split(',').Select(i => i.Trim()).ToList();

            Console.Write("Syötä valmistusohjeet: ");
            var instructions = Console.ReadLine();

            Console.Write("Syötä ruokavalioinformaatio (esim. gluteeniton, maidoton) tai jätä tyhjäksi: ");
            var dietaryInfo = Console.ReadLine();

            Recipe recipe = null;
            if (string.Equals(category, "pääruoka", StringComparison.OrdinalIgnoreCase))
            {
                recipe = new MainCourseRecipe(name, ingredients, instructions, dietaryInfo);
            }
            else if (string.Equals(category, "jälkiruoka", StringComparison.OrdinalIgnoreCase))
            {
                recipe = new DessertRecipe(name, ingredients, instructions, dietaryInfo);
            }
            else
            {
                recipe = new Recipe(name, category, ingredients, instructions, dietaryInfo);
            }

            recipeBook.AddRecipe(recipe);
        }

        private void SearchByIngredients()
        {
            Console.Write("Syötä ainesosat (erota pilkulla): ");
            var ingredients = Console.ReadLine().Split(',').Select(i => i.Trim()).ToList();
            recipeBook.SearchByIngredients(ingredients);
        }

        private void SearchByCategory()
        {
            Console.Write("Syötä ruokalaji (esim. pääruoka, jälkiruoka): ");
            var category = Console.ReadLine();
            recipeBook.SearchByCategory(category);
        }

        private void SearchByDietaryInfo()
        {
            Console.Write("Syötä ruokavalioinformaatio (esim. gluteeniton, maidoton, vegaaninen): ");
            var dietaryInfo = Console.ReadLine();
            recipeBook.SearchByDietaryInfo(dietaryInfo);
        }
    }

    public class KotikokkiUser
    {
        private RecipeBook recipeBook;

        public KotikokkiUser(RecipeBook recipeBook)
        {
            this.recipeBook = recipeBook;
        }

        public void Start()
        {
            bool running = true;

            while (running)
            {
                Console.WriteLine("");
                Console.WriteLine("Saatavilla olevat reseptit:");
                recipeBook.DisplayAllRecipes();
                Console.WriteLine("Valitse reseptin numero nähdäksesi lisätiedot tai ohjeet.");
                Console.WriteLine("Voit myös syöttää 0 palataksesi päävalikkoon.");
                Console.Write("Syötä reseptin numero tai 0: ");
                if (int.TryParse(Console.ReadLine(), out int recipeNumber))
                {
                    if (recipeNumber == 0)
                    {
                        running = false;
                        Console.WriteLine("Palataan päävalikkoon.");
                        break;
                    }

                    Recipe selectedRecipe = recipeBook.GetRecipeByIndex(recipeNumber);
                    if (selectedRecipe != null)
                    {
                        DisplayRecipeDetails(selectedRecipe);
                        Console.WriteLine("\nHaluatko valita toisen reseptin? (k/e): ");
                        string response = Console.ReadLine()?.ToLower();
                        if (response == "e")
                        {
                            running = false;
                            Console.WriteLine("Palataan päävalikkoon.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Virheellinen reseptinumero, yritä uudelleen.");
                    }
                }
                else
                {
                    Console.WriteLine("Virheellinen syöte, yritä uudelleen.");
                }
            }
        }

        private void DisplayRecipeDetails(Recipe recipe)
        {
            Console.WriteLine("\n### Valittu resepti ###");
            recipe.Display();
            Console.WriteLine("\nAinesosat:");
            recipe.DisplayIngredients();
            Console.WriteLine("\nValmistusohjeet:");
            recipe.DisplayStepByStepInstructions();
        }
    }

    public class CLI
    {
        private RecipeBook recipeBook = new RecipeBook();

        public void Start()
        {
            AddSampleRecipes();
            Console.WriteLine("Ohjelman oletusreseptit ladattu onnistuneesti!");

            bool running = true;
            while (running)
            {
                Console.WriteLine("");
                Console.WriteLine("### Reseptisovellus ###");
                Console.WriteLine("");
                Console.WriteLine("Tervetuloa! Valitse käyttäjä:");
                Console.WriteLine("1. Admin käyttäjä");
                Console.WriteLine("2. Kotikokki käyttäjä");
                Console.WriteLine("3. Lopeta ohjelma");
                Console.Write("Syötä valinta: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AdminUser adminUser = new AdminUser(recipeBook);
                        adminUser.Start();
                        break;
                    case "2":
                        KotikokkiUser kotikokkiUser = new KotikokkiUser(recipeBook);
                        kotikokkiUser.Start();
                        break;
                    case "3":
                        Console.WriteLine("Ohjelma suljetaan. Kiitos käytöstä!");
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Virheellinen valinta. Yritä uudelleen.");
                        break;
                }
            }
        }

        private void AddSampleRecipes()
        {
            recipeBook.AddRecipe(new MainCourseRecipe(
                "Kermainen lohipasta",
                new List<string> { "Lohi", "Pasta", "Kerma", "Valkosipuli", "Suola" },
                "Keitä pasta. Paista lohi ja valkosipuli pannulla. Sekoita kerma joukkoon. Yhdistä pasta ja kastike.",
                "Gluteeniton"));

            recipeBook.AddRecipe(new DessertRecipe(
                "Mokkapalat",
                new List<string> { "Kananmunat", "Sokeri", "Kahvi", "Kaakaojauhe", "Vehnäjauho" },
                "Sekoita ainekset ja paista uunissa 175 asteessa 20 minuuttia. Lisää kuorrutus.",
                "Ei ruokavaliorajoituksia"));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            CLI cli = new CLI();
            cli.Start();
        }
    }
}
