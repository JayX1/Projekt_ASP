using Microsoft.AspNetCore.Mvc;
using ASP_projekt.Services;
using ASP_projekt.Interfaces;
using ASP_projekt.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP_projekt.Controllers
{
    public class SuperheroController : Controller
    {
        protected readonly ISuperheroService _superheroService;
        protected readonly AppDbContext _context;
        private readonly ILogger<SuperheroController> _logger;


        public SuperheroController(ISuperheroService superheroService, AppDbContext context, ILogger<SuperheroController> logger)
        {
            _superheroService = superheroService;
            _context = context;
            _logger = logger;
        }


        public async Task<IActionResult> Index(int page = 1, int size = 10)
        {
            try
            {
                var superheroesQuery = _context.Superheroes
                    .OrderBy(s => s.SuperheroName)
                    .Select(s => new Superhero
                    {
                        Id = s.Id,
                        SuperheroName = s.SuperheroName ?? "N/A", 
                        FullName = s.FullName ?? "N/A",         
                        WeightKg = s.WeightKg ?? 0,             
                        HeightCm = s.HeightCm ?? 0,             
                    });

                var view = View(PagingListAsync<Superhero>.Create(
                   (p, s) => superheroesQuery
                       .Skip((p - 1) * s)
                       .Take(s)
                       .AsAsyncEnumerable(),
                    await superheroesQuery.CountAsync(),
                    page,
                    size
                ));
                
                
                return view;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving superheroes.");
                throw;
            }
        }



        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.AvailablePowers = _context.Superpowers
                .ToDictionary(p => p.Id, p => p.PowerName);

            return View(new Superhero());
        }



        [HttpPost]
        public IActionResult Create(Superhero model, List<int> selectedPowerIds)
        {

            if (ModelState.IsValid)
            {

                foreach (var powerId in selectedPowerIds)
                {
                    var heroPower = new HeroPower
                    {
                        HeroId = model.Id,   
                        PowerId = powerId    
                    };

                }

                _context.Superheroes.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }




        public IActionResult PowerName(int id)
        {
            var powers = _superheroService.GetPower(id);
            if (powers == null || !powers.Any())
            {
                return NotFound();
            }
            else
            {
                return View(powers);
            }
        }
    }
}
