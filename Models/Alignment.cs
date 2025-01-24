using System;
using System.Collections.Generic;

namespace ASP_projekt.Models;

public partial class Alignment
{
    public int Id { get; set; }

    public string? Alignment1 { get; set; }

    public virtual ICollection<Superhero> Superheroes { get; set; } = new List<Superhero>();
}
