using RecipeConsoleApp;
using Spectre.Console;
using System.Text;
using System.Text.Json;

// load previous categories if it exists
string categoriesFile = "categories.json";
string jsonCategoriesString;
var categoriesList = new List<string>();

if (File.Exists(categoriesFile))
{
	if (new FileInfo(categoriesFile).Length > 0)
	{
		jsonCategoriesString = File.ReadAllText(categoriesFile);
		categoriesList = JsonSerializer.Deserialize<List<string>>(jsonCategoriesString)!;
	}
}

// load previous recipes if it exists
string recipesFile = "Recipes.json";
string jsonRecipesString;
var recipesList = new List<Recipe>();

if (File.Exists(recipesFile))
{
	if (new FileInfo(recipesFile).Length > 0)
	{
		jsonRecipesString = File.ReadAllText(recipesFile);
		recipesList = JsonSerializer.Deserialize<List<Recipe>>(jsonRecipesString)!;
	}
}

while (true)
{
	var command = AnsiConsole.Prompt(
	   new SelectionPrompt<string>()
		   .Title("What would you like to do?")
		   .AddChoices(new[]
		   {
			   "List all Recipes",
		   })
		   .AddChoiceGroup("Recipes", new[]
		   {
			   "Add a Recipe",
			   "Delete a Recipe",
			   "Edit a Recipe"
		   })
		   .AddChoiceGroup("Categories", new[]
		   {
			   "Add a Category",
			   "Delete a Category",
			   "Edit a Category"
		   })
		   .AddChoices(new[]
		   {
			   "Save & Exit"
		   }));
	AnsiConsole.Clear();
	switch (command)
	{
		case "List all Recipes":
			ListRecipes();
			break;
		case "Add a Recipe":
			AddRecipe();
			break;
		case "Delete a Recipe":
			RemoveRecipe();
			break;
		case "Edit a Recipe":
			EditRecipe();
			break;
		case "Add a Category":
			AddCategory();
			break;
		case "Delete a Category":
			RemoveCategory();
			break;
		case "Edit a Category":
			EditCategory();
			break;
		default:
			Save();
			return;
	}
}

void AddRecipe()
{
	var title = AnsiConsole.Ask<string>("What is the [green]recipe[/] called?");
	var ingredients = new List<string>();
	var instructions = new List<string>();
	var categories = new List<string>();

	AnsiConsole.MarkupLine("Enter all the [green]ingredients[/]. Once done, leave the ingredient field [red]empty[/] and press enter");
	var ingredient = AnsiConsole.Ask<string>("Enter ingredient: ");
	while (ingredient != "")
	{
		ingredients.Add(ingredient);
		ingredient = AnsiConsole.Prompt(new TextPrompt<string>("Enter ingredient: ").AllowEmpty());
	};

	AnsiConsole.MarkupLine("Enter all the [green]instructions[/]. Once done, leave the instruction field [red]empty[/] and press enter");
	var instruction = AnsiConsole.Ask<string>("Enter instruction: ");
	while (instruction != "")
	{
		instructions.Add(instruction);
		instruction = AnsiConsole.Prompt(new TextPrompt<string>("Enter instruction: ").AllowEmpty());
	};

	var recipe = new Recipe()
	{
		Title = title,
		Ingredients = ingredients,
		Instructions = instructions,
		Categories = categories
	};

	recipesList.Add(recipe);

	if (categoriesList.Count == 0)
	{
		return;
	}

	var selectedcategories = AnsiConsole.Prompt(
	new MultiSelectionPrompt<string>()
	.PageSize(10)
	.Title("Which [green]categories[/] does this recipe belong to?")
	.MoreChoicesText("[grey](Move up and down to reveal more categories)[/]")
	.InstructionsText("[grey](Press [blue]Space[/] to toggle a category, [green]Enter[/] to accept)[/]")
	.AddChoices(categoriesList));

	recipe.Categories = selectedcategories;
}

void RemoveRecipe()
{
	if (recipesList.Count == 0)
	{
		AnsiConsole.MarkupLine("There are no Recipes");
		return;
	}
	var selectedRecipes = AnsiConsole.Prompt(
	new MultiSelectionPrompt<Recipe>()
	.PageSize(10)
	.Title("Which [green]recipes[/] does this recipe belong to?")
	.MoreChoicesText("[grey](Move up and down to reveal more recipes)[/]")
	.InstructionsText("[grey](Press [blue]Space[/] to toggle a recipe, [green]Enter[/] to accept)[/]")
	.AddChoices(recipesList));

	foreach (var recipe in selectedRecipes)
	{
		recipesList.Remove(recipe);
	}
}

void EditRecipe()
{
	if (recipesList.Count == 0)
	{
		AnsiConsole.MarkupLine("There are no Recipes");
		return;
	}

	var chosenRecipe = AnsiConsole.Prompt(
	   new SelectionPrompt<Recipe>()
		   .Title("Which Recipe would you like to edit?")
		   .AddChoices(recipesList));

	var command = AnsiConsole.Prompt(
	   new SelectionPrompt<string>()
		   .Title("What would you like to do?")
		   .AddChoices(new[]
		   {
			   "Edit title",
			   "Edit Ingredients",
			   "Edit Instructions",
			   "Edit Categories"
		   }));

	AnsiConsole.Clear();
	switch (command)
	{
		case "Edit title":
			chosenRecipe.Title = AnsiConsole.Ask<string>("What is the [green]recipe[/] called?");
			break;
		case "Edit Ingredients":
			chosenRecipe.Ingredients.Clear();
			AnsiConsole.MarkupLine("Enter all the [green]ingredients[/]. Once done, leave the ingredient field [red]empty[/] and press enter");
			var ingredient = AnsiConsole.Ask<string>("Enter ingredient: ");
			while (ingredient != "")
			{
				chosenRecipe.Ingredients.Add(ingredient);
				ingredient = AnsiConsole.Prompt(new TextPrompt<string>("Enter ingredient: ").AllowEmpty());
			};
			break;
		case "Edit Instructions":
			chosenRecipe.Instructions.Clear();
			AnsiConsole.MarkupLine("Enter all the [green]instructions[/]. Once done, leave the instruction field [red]empty[/] and press enter");
			var instruction = AnsiConsole.Ask<string>("Enter instruction: ");
			while (instruction != "")
			{
				chosenRecipe.Instructions.Add(instruction);
				instruction = AnsiConsole.Prompt(new TextPrompt<string>("Enter instruction: ").AllowEmpty());
			};
			break;
		case "Edit Categories":
			var selectedcategories = AnsiConsole.Prompt(
			new MultiSelectionPrompt<string>()
			.PageSize(10)
			.Title("Which [green]categories[/] does this recipe belong to?")
			.MoreChoicesText("[grey](Move up and down to reveal more categories)[/]")
			.InstructionsText("[grey](Press [blue]Space[/] to toggle a category, [green]Enter[/] to accept)[/]")
			.AddChoices(categoriesList));

			chosenRecipe.Categories = selectedcategories;
			break;
	}
}

void AddCategory()
{
	string category = AnsiConsole.Ask<string>("What is the [green]category[/] called?");
	categoriesList.Add(category);
}

void RemoveCategory()
{
	if (categoriesList.Count == 0)
	{
		AnsiConsole.MarkupLine("There are no Categories");
		return;
	}
	var selectedcategories = AnsiConsole.Prompt(
	new MultiSelectionPrompt<string>()
	.PageSize(10)
	.Title("Which [green]categories[/] does this recipe belong to?")
	.MoreChoicesText("[grey](Move up and down to reveal more categories)[/]")
	.InstructionsText("[grey](Press [blue]Space[/] to toggle a category, [green]Enter[/] to accept)[/]")
	.AddChoices(categoriesList));

	foreach (string category in selectedcategories)
	{
		foreach (Recipe recipe in recipesList)
		{
			recipe.Categories.Remove(category);
		}
		categoriesList.Remove(category);
	}
}

void EditCategory()
{
	if (categoriesList.Count == 0)
	{
		AnsiConsole.MarkupLine("There are no Categories");
		return;
	}
	var chosenCategory = AnsiConsole.Prompt(
	   new SelectionPrompt<string>()
		   .Title("Which Category would you like to edit?")
		   .AddChoices(categoriesList));

	string newCategoryName = AnsiConsole.Prompt(new TextPrompt<string>("What would you like to change the name to?"));

	categoriesList.Remove(chosenCategory);
	categoriesList.Add(newCategoryName);

	foreach (var recipe in recipesList)
	{
		recipe.Categories.Remove(chosenCategory);
		recipe.Categories.Add(newCategoryName);
	}

}

void ListRecipes()
{
	var table = new Table();
	table.AddColumn("Recipe Name");
	table.AddColumn("Ingredients");
	table.AddColumn("Instructions");
	table.AddColumn("Categories");

	foreach (var recipe in recipesList)
	{
		var ingredients = new StringBuilder();
		foreach (string ingredient in recipe.Ingredients)
			ingredients.Append("- " + ingredient + "\n");
		var instructions = new StringBuilder();
		foreach (string instruction in recipe.Instructions)
			instructions.Append("- " + instruction + "\n");
		var categories = new StringBuilder();
		foreach (string category in recipe.Categories)
			categories.Append("- " + category + "\n");
		table.AddRow(recipe.Title, ingredients.ToString(), instructions.ToString(), categories.ToString());
	}
	AnsiConsole.Write(table);
}

void Save()
{
	File.WriteAllText(recipesFile, JsonSerializer.Serialize(recipesList));
	File.WriteAllText(categoriesFile, JsonSerializer.Serialize(categoriesList));
}
