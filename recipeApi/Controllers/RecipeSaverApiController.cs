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

        public RecipeSaverApiController(RecipeSaverContext context)
        {
            _context = context;
        }

        [HttpGet("users")]
        public async Task<List<User>> getUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpPost("users")]
        public async Task<User> addUser([Bind("UserID, UserName, UserEmail, UserPassword")] User newUser)
        {
            _context.Add(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }

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

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }

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
                //.Where(r => r.UserId == id && r.IsOnList == false)
                .Where(r => r.UserId == id);
                //.ToListAsync();
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
    }
}
