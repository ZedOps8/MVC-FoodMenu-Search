using Microsoft.AspNetCore.Mvc;
using Menu.Data;
using Menu.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Menu.Controllers
{
    public class MenuController : Controller
    {
        private readonly MenuContext _context;

        // Constructor to initialize the context
        public MenuController(MenuContext context)
        {
            _context = context;
        }

        // The SearchString parameter should be a string and passed in as a parameter.
        public async Task<IActionResult> Index(string SearchString)
        {
            // Start with all dishes
            var dishes = from d in _context.Dishes
                         select d;

            // If a search string is provided, filter the results
            if (!string.IsNullOrEmpty(SearchString))
            {
                dishes = dishes.Where(d => d.Name.Contains(SearchString));
            }

            // Return the filtered or unfiltered list of dishes to the view
            return View(await dishes.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the dish along with its ingredients based on the provided ID
            var dish = await _context.Dishes
                .Include(di => di.DishIngredients)
                .ThenInclude(i => i.Ingredient)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }
    }
}
