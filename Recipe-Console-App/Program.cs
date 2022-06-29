using Recipe_Console_App;
using Spectre.Console;
using System.Text.Json;

void AddRecipe()
{
	var title = AnsiConsole.Ask<string>("What is the [green]recipe[/] called?");
	var recipe = new Recipe();
	recipe.Title = title;

	AnsiConsole.WriteLine("Enter all the [green]ingredients[/]. Once done, leave the ingredient field [red]empty[/] and press enter");
	var ingredient = AnsiConsole.Ask<string>("Enter ingredient: ");
	while (ingredient != "")
	{
		recipe.Ingredients.Add(ingredient);
		ingredient = AnsiConsole.Prompt(new TextPrompt<string>("Enter ingredient: ").AllowEmpty());
	};

	AnsiConsole.WriteLine("Enter all the [green]instructions[/]. Once done, leave the instruction field [red]empty[/] and press enter");
	var instruction = AnsiConsole.Ask<string>("Enter instruction: ");
	while (instruction != "")
	{
		recipe.Instructions.Add(instruction);
		instruction = AnsiConsole.Prompt(new TextPrompt<string>("Enter instruction: ").AllowEmpty());
	};

	recipe.Ingredients.ForEach(Console.WriteLine);
	Console.WriteLine(recipe.Ingredients.Count);
};