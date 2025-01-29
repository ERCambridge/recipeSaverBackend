using System;
using System.Collections.Generic;

namespace recipeApi.Models;

public partial class Ingredient
{
    public int IngredientId { get; set; }

    public string IngredientName { get; set; } = null!;

    public string IngredientQuantity { get; set; } = null!;

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
