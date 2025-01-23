using System;
using System.Collections.Generic;

namespace recipeApi.Models;

public partial class Recipe
{
    public int RecipeId { get; set; }

    public string RecipeName { get; set; } = null!;

    public string RecipeCategory { get; set; } = null!;

    public string IngredientList { get; set; } = null!;

    public bool? IsOnList { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
