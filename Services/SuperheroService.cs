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
            var heroPowers = _context.HeroPowers
                .Include(hp => hp.Power) 
                .Where(hp => hp.HeroId == superheroId) 
                .ToList();

            List<(string PowerName, int HeroCount)> items = new List<(string, int)>();

            foreach (var heroPower in heroPowers)
            {
                var powerName = heroPower.Power.PowerName; 
                var powerId = heroPower.PowerId; 

                var count = _context.HeroPowers
                    .Where(hp => hp.PowerId == powerId && hp.HeroId != superheroId) 
                    .Select(hp => hp.HeroId)
                    .Distinct()
                    .Count();

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
