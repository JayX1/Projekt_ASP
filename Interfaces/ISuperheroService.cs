using ASP_projekt.Models;
namespace ASP_projekt.Interfaces
{
    public interface ISuperheroService
    {
        int Add(Superhero superhero);
        List<Superhero> GetAll();
        public int saveToDb(Superhero item);
        public List<(string, int)> GetPower(int superheroId);
        public Dictionary<int, string> AvailablePowers();
    }
}
