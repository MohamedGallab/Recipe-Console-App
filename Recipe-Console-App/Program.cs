using Recipe_Console_App;
using Spectre.Console;
using System.Text;
using System.Text.Json;

// load previous categories if exists
string categoriesFile = "categories.json";
string jsoncategoriesString;
var categoriesList = new List<Category>();
var availableCategories = new List<string>();

if (File.Exists(categoriesFile))
{
	if (new FileInfo(categoriesFile).Length > 0)
	{
		jsoncategoriesString = File.ReadAllText(categoriesFile);
		categoriesList = JsonSerializer.Deserialize<List<Category>>(jsoncategoriesString)!;
		UpdateAvailablecategories();
	}
}

// load previous recipes if exists
string recipesFile = "Recipes.json";
string jsonRecipesString;
var recipesList = new List<Recipe>();
var availableRecipes = new List<string>();

if (File.Exists(recipesFile))
{
	if (new FileInfo(recipesFile).Length > 0)
	{
		jsonRecipesString = File.ReadAllText(recipesFile);
		recipesList = JsonSerializer.Deserialize<List<Recipe>>(jsonRecipesString)!;
		UpdateAvailableRecipes();
	}
}

var favorites = AnsiConsole.Prompt(
    new SelectionPrompt<Recipe>()
        .PageSize(10)
        .Title("What are your [green]favorite fruits[/]?")
        .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
        .AddChoices(recipesList));

while (true)
{
	var command = AnsiConsole.Prompt(
	   new SelectionPrompt<string>()
		   .Title("What would you like to do?")
		   .AddChoices(new[]
		   {
			   "List all Recipes",
			   "Add a Recipe",
			   "Delete a Recipe",
			   "Edit a Recipe",
			   "Add a Category",
			   "Delete a Category",
			   "Edit a Category",
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
	var categories = new List<Category>();

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
	var selectedcategories = new List<String>();
	if (availableCategories.Count > 0)
	{
		selectedcategories = AnsiConsole.Prompt(
		new MultiSelectionPrompt<string>()
		.PageSize(10)
		.Title("Which [green]categories[/] does this recipe belong to?")
		.MoreChoicesText("[grey](Move up and down to reveal more categories)[/]")
		.InstructionsText("[grey](Press [blue]Space[/] to toggle a category, [green]Enter[/] to accept)[/]")
		.AddChoices(availableCategories));

		foreach (var selectedCategory in selectedcategories)
		{
			Category result = categoriesList.Find(category => category.Title == selectedCategory);
			categories.Add(result);
		}
	}
	var recipe = new Recipe()
	{
		Title = title,
		Ingredients = ingredients,
		Instructions = instructions,
		Categories = categories
	};
	recipesList.Add(recipe);
	UpdateAvailableRecipes();
}

void RemoveRecipe()
{
	if (availableRecipes.Count == 0)
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
	UpdateAvailableRecipes();
}

void EditRecipe()
{
	if(availableRecipes.Count == 0)
	{
		AnsiConsole.MarkupLine("There are no Categories");
		return;
	}

	var chosenRecipe = AnsiConsole.Prompt(
	   new SelectionPrompt<string>()
		   .Title("Which Recipe would you like to edit?")
		   .AddChoices(availableRecipes));

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
			ListRecipes();
			break;
		case "Edit Ingredients":
			AddRecipe();
			break;
		case "Edit Instructions":
			RemoveRecipe();
			break;
		case "Edit Categories":
			RemoveRecipe();
			break;
	}



	var recipe = recipesList.Find(recipe => recipe.Title == chosenRecipe);

	UpdateAvailableRecipes();
}

void AddCategory()
{
	var category = new Category()
	{
		Title = AnsiConsole.Ask<string>("What is the [green]category[/] called?")
	};
	categoriesList.Add(category);
	UpdateAvailablecategories();
}

void RemoveCategory()
{
	if (availableCategories.Count == 0)
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
	.AddChoices(availableCategories));

	foreach (var categoryName in selectedcategories)
	{
		Category result = categoriesList.Find(category => category.Title == categoryName);
		foreach (var recipe in recipesList)
		{
			Category recipeResult = recipe.Categories.Find(category => category.Title == categoryName);
			recipe.Categories.Remove(recipeResult);
		}
		categoriesList.Remove(result);
	}

	UpdateAvailablecategories();
}

void EditCategory()
{
	if (availableCategories.Count == 0)
	{
		AnsiConsole.MarkupLine("There are no Categories");
		return;
	}
	var chosenCategory = AnsiConsole.Prompt(
	   new SelectionPrompt<string>()
		   .Title("Which Category would you like to edit?")
		   .AddChoices(availableCategories));
	String newCategoryName = AnsiConsole.Prompt(new TextPrompt<string>("What would you like to change the name to?"));

	Category result = categoriesList.Find(category => category.Title == chosenCategory);
	result.Title = newCategoryName;
	foreach (var recipe in recipesList)
	{
		Category recipeResult = recipe.Categories.Find(category => category.Title == chosenCategory);
		recipeResult.Title = newCategoryName;
	}

	UpdateAvailablecategories();
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
		foreach (String ingredient in recipe.Ingredients)
			ingredients.Append("- " + ingredient + "\n");
		var instructions = new StringBuilder();
		foreach (String instruction in recipe.Instructions)
			instructions.Append("- " + instruction + "\n");
		var categories = new StringBuilder();
		foreach (Category category in recipe.Categories)
			categories.Append("- " + category.Title + "\n");
		table.AddRow(recipe.Title, ingredients.ToString(), instructions.ToString(), categories.ToString());
	}
	AnsiConsole.Write(table);
}

void Save()
{
	File.WriteAllText(recipesFile, JsonSerializer.Serialize(recipesList));
	File.WriteAllText(categoriesFile, JsonSerializer.Serialize(categoriesList));
}

void UpdateAvailablecategories()
{
	availableCategories.Clear();
	foreach (Category category in categoriesList)
	{
		availableCategories.Add(category.Title);
	}
}

void UpdateAvailableRecipes()
{
	availableRecipes.Clear();
	foreach (Recipe recipe in recipesList)
	{
		availableRecipes.Add(recipe.Title);
	}
}