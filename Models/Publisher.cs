﻿using System;
using System.Collections.Generic;

namespace ASP_projekt.Models;

public partial class Publisher
{
    public int Id { get; set; }

    public string? PublisherName { get; set; }

    public virtual ICollection<Superhero> Superheroes { get; set; } = new List<Superhero>();
}
