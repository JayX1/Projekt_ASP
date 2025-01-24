using System;
using System.Collections.Generic;

namespace ASP_projekt.Models;

public partial class HeroPower
{
    public int? HeroId { get; set; }

    public int? PowerId { get; set; }

    public virtual Superhero? Hero { get; set; }

    public virtual Superpower? Power { get; set; }
}
