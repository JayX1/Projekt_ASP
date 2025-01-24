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

        //public IActionResult Index()
        //{
        //    return View(_superheroService.GetAll());
        //}

        //public IActionResult Index(int page = 1, int size = 10)
        //{
        //    return View(PagingListAsync<Superhero>.Create(
        //    (p, s) =>
        //            _context.Superheroes
        //            .OrderBy(b => b.SuperheroName)
        //            .Skip((p - 1) * s)
        //            .Take(s)
        //        .AsAsyncEnumerable(),
        //        _context.Superheroes.Count(),
        //        page,
        //        size));
        //}

        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult Create(Superhero model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _superheroService.Add(model); 
        //        return RedirectToAction("Index");
        //    }
        //    return View(model);
        //}


        public async Task<IActionResult> Index(int page = 1, int size = 10)
        {
            try
            {
                var superheroesQuery = _context.Superheroes
                    .OrderBy(s => s.SuperheroName)
                    .Select(s => new Superhero
                    {
                        Id = s.Id,
                        SuperheroName = s.SuperheroName ?? "N/A", // Default to "N/A" for null values
                        FullName = s.FullName ?? "N/A",          // Default to "N/A" for null values
                        WeightKg = s.WeightKg ?? 0,             // Default to 0 for null values
                        HeightCm = s.HeightCm ?? 0,              // Default to 0 for null values
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
            // Pass available powers to the view
            ViewBag.AvailablePowers = _context.Superpowers
                .ToDictionary(p => p.Id, p => p.PowerName);

            return View(new Superhero()); // Use the Superhero model or ViewModel
        }


        //[HttpPost]
        //public IActionResult Create(Superhero model, List<int> selectedPowerIds)
        //{
        //    if (selectedPowerIds == null || !selectedPowerIds.Any())
        //    {
        //        // Add a model error for the superpower selection
        //        ModelState.AddModelError("selectedPowerIds", "Please select at least one superpower.");
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        // Save the superhero to the database
        //        _context.Superheroes.Add(model);
        //        _context.SaveChanges();

        //        // Save selected powers to the HeroPowers table
        //        foreach (var powerId in selectedPowerIds)
        //        {
        //            _context.HeroPowers.Add(new HeroPower
        //            {
        //                HeroId = model.Id,
        //                PowerId = powerId
        //            });
        //        }
        //        _context.SaveChanges();

        //        return RedirectToAction("Index");
        //    }

        //    // Reload available powers in case of an error
        //    ViewBag.AvailablePowers = _context.Superpowers
        //        .ToDictionary(p => p.Id, p => p.PowerName);

        //    return View(model);
        //}


        [HttpPost]
        public IActionResult Create(Superhero model, List<int> selectedPowerIds)
        {

            if (ModelState.IsValid)
            {

                

                // Add the selected powers to the HeroPowers collection of the Superhero
                foreach (var powerId in selectedPowerIds)
                {
                    var heroPower = new HeroPower
                    {
                        HeroId = model.Id,   // Assuming the superhero already has an ID at this point
                        PowerId = powerId    // This should correspond to an existing Superpower ID
                    };

                    //_context.HeroPowers.Add(heroPower);
                    //_context.SaveChanges();


                    //var power = new Superpower
                    //{
                    //    Id = powerId,
                    //};
                    //model.Powers.Add(power);
                }

                // Add the Superhero to the context
                _context.Superheroes.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            // If model is invalid, return to the view with validation errors
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
