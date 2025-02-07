using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using recipeApi.Models;

namespace recipeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeSaverApiController : ControllerBase
    {

        private readonly RecipeSaverContext _context;

        //constructor for db context
        public RecipeSaverApiController(RecipeSaverContext context)
        {
            _context = context;
        }

        /****************
         * CRUD for Users
         ****************/

        //get all users
        [HttpGet("users")]
        public async Task<List<User>> getUsers()
        {
            return await _context.Users.ToListAsync();
        }

        //get user by email
        [HttpGet("user")]
        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserEmail == email);
            return user;
        }


        //add new user
        [HttpPost("users")]
        public async Task<User> addUser([Bind("UserID, UserName, UserEmail, UserPassword")] User newUser)
        {
            _context.Add(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }

        //delete existing user by id
        [HttpDelete("users/{id}")]
        public async Task<ActionResult<int>> deleteUser(int id)
        {
            var aUser = await _context.Users.FindAsync(id);

            if (aUser != null)
            {
                _context.Users.Remove(aUser);
                _context.SaveChanges();
            }
            return Ok();
        }

        //Update existing user
        [HttpPut("users")]
        public async Task<ActionResult<int>> updateUser([Bind("UserID, UserName, UserEmail, UserPassword")] User updatedUser)
        {
            try
            {
                _context.Update(updatedUser);
                await _context.SaveChangesAsync();
            }
            catch
            {
                if (UserExists(updatedUser.UserId))
                {
                    throw;
                }
                else
                {
                    return NotFound();
                }
            }
            return Ok();
        }

        //check if user exists
        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }

        /******************
         * CRUD for Recipes
         ******************/

        //get all recipes - WE May NOT NEED
        [HttpGet("recipes")]
        public async Task<List<Recipe>> getRecipes()
        {
            return await _context.Recipes.ToListAsync();
        }

        //Get Recipes by UserID
        [HttpGet("recipes/{id}")]
        public async Task<List<Recipe>> getRecipeByUserId(int id, bool? isOnList = null)
        {
            var recipes = _context.Recipes
                .Where(r => r.UserId == id);
            if (isOnList != null)
            {
                recipes = recipes.Where(r => r.IsOnList == isOnList.Value);
            }

            return await recipes.ToListAsync();
        }

        //Get Recipe by Recipe ID
        [HttpGet("recipe/{id}")]
        public async Task<List<Recipe>> getRecipeByRecipeId(int id)
        {
            var recipe = await _context.Recipes
                .Where(r => r.RecipeId == id)
                .ToListAsync();

            return recipe;
        }

        //Create new recipe
        [HttpPost("recipe")]
        public async Task<Recipe> addRecipe([Bind("RecipeID, RecipeName, RecipeCategory, IngredientList, IsOnList, UserID, User")] Recipe newRecipe)
        {
            _context.Add(newRecipe);
            await _context.SaveChangesAsync();
            return newRecipe;
        }
        //Edit Existing Recipe
        [HttpPut("recipe")]
        public async Task<ActionResult<int>> updateRecipe([Bind("RecipeID, RecipeName, RecipeCategory, IngredientList, IsOnList, UserID")] Recipe updatedRecipe)
        {
            try
            {
                _context.Update(updatedRecipe);
                await _context.SaveChangesAsync();
            }
            catch
            {
                if (RecipeExists(updatedRecipe.RecipeId))
                {
                    throw;
                }
                else
                {
                    return NotFound();
                }
            }
            return Ok();
        }

        //Check if Recipe Exists
        private bool RecipeExists(int id)
        {
            return (_context.Recipes?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
        //Delete Existing Recipe
        [HttpDelete("recipe/{id}")]
        public async Task<ActionResult<int>> deleteRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe != null)
            {
                _context.Recipes.Remove(recipe);
                _context.SaveChanges();
            }
            return Ok();
        }

        /**********************
         * CRUD for Ingredients
         **********************/

        //Get Ingredients by UserID
        [HttpGet("ingredients/{id}")]
        public async Task<List<Ingredient>> getIngredientByUserId(int id)
        {
            var ingredients = _context.Ingredients

                .Where(i => i.UserId == id);

            return await ingredients.ToListAsync();
        }

        //Create new ingredient
        [HttpPost("ingredient")]
        public async Task<Ingredient> addIngredient([Bind("IngredientID, IngredientName, IngredientQuantity, UserID, User")] Ingredient newIngredient)
        {
            _context.Add(newIngredient);
            await _context.SaveChangesAsync();
            return newIngredient;
        }

        //Delete Existing Ingredient
        [HttpDelete("ingredient/{id}")]
        public async Task<ActionResult<int>> deleteIngredient(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient != null)
            {
                _context.Ingredients.Remove(ingredient);
                _context.SaveChanges();
            }
            return Ok();
        }

        //Potentially not needed/admin only 

        //Get all ingredients
        [HttpGet("ingredients")]
        public async Task<List<Ingredient>> getIngredients()
        {
            return await _context.Ingredients.ToListAsync();
        }

        //Edit Existing Ingredient
        [HttpPut("ingredient")]
        public async Task<ActionResult<int>> updateIngredient([Bind("IngredientID, IngredientName, IngredientQuantity, UserID, User")] Ingredient updatedIngredient)
        {
            try
            {
                _context.Update(updatedIngredient);
                await _context.SaveChangesAsync();
            }
            catch
            {
                if (IngredientExists(updatedIngredient.IngredientId))
                {
                    throw;
                }
                else
                {
                    return NotFound();
                }
            }
            return Ok();
        }

        //Check if Ingredient Exists
        private bool IngredientExists(int id)
        {
            return (_context.Ingredients?.Any(e => e.IngredientId == id)).GetValueOrDefault();
        }

    }
}
