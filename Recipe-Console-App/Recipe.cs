using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Console_App
{
	internal class Recipe
	{
		public String Title { get; set; }
		public List<string> Ingredients { get; set; } = new List<string>();
		public List<string> Instructions { get; set; } = new List<string>();
		public List<Category> Categories { get; set; } = new List<Category>();
	}
}
