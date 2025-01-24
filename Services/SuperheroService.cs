using System;
using ASP_projekt.Models;
using ASP_projekt.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASP_projekt.Services
{
    public class SuperheroService : ISuperheroService
    {
        private readonly AppDbContext _context;

        public SuperheroService(AppDbContext context)
        {
            _context = context;
        }
        

        private readonly List<Superhero> _heroes = new List<Superhero>();
        private readonly List<HeroPower> _powers = new List<HeroPower>();


        public int Add(Superhero item)
        {
            //int id = _heroes.Count + 1;
            //item.Id = id;
            //_heroes.Add(item);
            //return id;

           //int count = _context.Superheroes.Count();
           // item.Id = count + 1;
            _context.Superheroes.Add(item);
            _context.SaveChanges();
            return item.Id;
        }
        public int saveToDb(Superhero item)
        {
            _context.Superheroes.Add(item);
            _context.SaveChanges();
            return item.Id;
        }


        public List<Superhero> GetAll()
        {
            return _context.Superheroes.ToList();
        }

        

        public List<(string, int)> GetPower(int superheroId)
        {
            // Step 1: Get all powers associated with the given superhero
            var heroPowers = _context.HeroPowers
                .Include(hp => hp.Power) // Include related Superpower entity
                .Where(hp => hp.HeroId == superheroId) // Filter by superhero ID
                .ToList();

            // Step 2: Create a list to hold the results
            List<(string PowerName, int HeroCount)> items = new List<(string, int)>();

            // Step 3: For each power, calculate the number of other superheroes sharing the same power
            foreach (var heroPower in heroPowers)
            {
                var powerName = heroPower.Power.PowerName; // Get the power name
                var powerId = heroPower.PowerId; // Get the power ID

                // Count other superheroes sharing this power
                var count = _context.HeroPowers
                    .Where(hp => hp.PowerId == powerId && hp.HeroId != superheroId) // Exclude the given superhero
                    .Select(hp => hp.HeroId)
                    .Distinct()
                    .Count();

                // Add the result to the list
                items.Add((powerName, count));
            }

            return items;
        }

        public Dictionary<int, string> AvailablePowers()
        {
            return _context.Superpowers.ToDictionary(p => p.Id, p => p.PowerName);
        }
    }
}
