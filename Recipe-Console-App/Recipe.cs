using Spectre.Console;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Console_App;

[TypeConverter(typeof(RecipeTypeConverter))]
internal class Recipe
{
	public String Title { get; set; } = String.Empty;
	public List<string> Ingredients { get; set; } = new();
	public List<string> Instructions { get; set; } = new();
	public List<Category> Categories { get; set; } = new();
}
