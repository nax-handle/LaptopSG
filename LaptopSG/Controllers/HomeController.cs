using Microsoft.AspNetCore.Mvc;
using LaptopSG.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LaptopSG.Controllers
{
    public class HomeController : Controller
    {
        private readonly StoreDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(StoreDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.Message = "Home Page";
            TempData["Message"] = "This is the home page!";
            return View();
        }

        // Test Database Connection
        public async Task<IActionResult> TestConnection()
        {
            try
            {
                _logger.LogInformation("Testing database connection from web interface...");
                
                var canConnect = await _context.Database.CanConnectAsync();
                
                if (canConnect)
                {
                    _logger.LogInformation("✅ Database connection test successful from web!");
                    
                    // Get database info
                    var connectionString = _context.Database.GetConnectionString();
                    var providerName = _context.Database.ProviderName;
                    var databaseName = _context.Database.GetDbConnection().Database;
                    
                    ViewBag.ConnectionStatus = "Success";
                    ViewBag.ConnectionString = connectionString;
                    ViewBag.ProviderName = providerName;
                    ViewBag.DatabaseName = databaseName;
                    ViewBag.Message = "Database connection is working!";
                    
                    // Test a simple query
                    var categoryCount = await _context.Categories.CountAsync();
                    ViewBag.CategoryCount = categoryCount;
                    
                    _logger.LogInformation($"Categories in database: {categoryCount}");
                }
                else
                {
                    _logger.LogError("❌ Database connection test failed from web!");
                    ViewBag.ConnectionStatus = "Failed";
                    ViewBag.Message = "Database connection failed!";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error testing database connection from web: {ErrorMessage}", ex.Message);
                ViewBag.ConnectionStatus = "Error";
                ViewBag.Message = $"Error: {ex.Message}";
                ViewBag.ErrorDetails = ex.ToString();
            }
            
            return View();
        }

        // GET: Categories
        public async Task<IActionResult> Categories()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }

        // GET: Create Category
        public IActionResult CreateCategory()
        {
            return View();
        }

        // POST: Create Category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory([Bind("Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                category.CreatedAt = DateTime.Now;
                _context.Add(category);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Category created successfully!";
                return RedirectToAction(nameof(Categories));
            }
            return View(category);
        }

        // GET: Edit Category
        public async Task<IActionResult> EditCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Edit Category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(int id, [Bind("Id,Name")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingCategory = await _context.Categories.FindAsync(id);
                    if (existingCategory != null)
                    {
                        existingCategory.Name = category.Name;
                        existingCategory.UpdatedAt = DateTime.Now;
                        await _context.SaveChangesAsync();
                    }
                    TempData["SuccessMessage"] = "Category updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Categories));
            }
            return View(category);
        }

        // GET: Delete Category
        public async Task<IActionResult> DeleteCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Delete Category
        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategoryConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Category deleted successfully!";
            }

            return RedirectToAction(nameof(Categories));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}