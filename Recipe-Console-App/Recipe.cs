using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Console_App
{
	internal class Recipe
	{
		public String? Title { get; set; }
		public List<string> Ingredients { get; set; } = new List<string>();
		public List<string> Instructions { get; set; } = new List<string>();
		// temporarily making treating categories as strings
		// will change later to a more robust type like class or enum
		public List<string> Categories { get; set; } = new List<string>();
	}
}
