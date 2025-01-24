using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;

namespace ASP_projekt.Models;
using System.ComponentModel.DataAnnotations;
public partial class Superhero
{
    [HiddenInput]
    public int Id { get; set; }

    [Required(ErrorMessage = "Proszę podać nazwę!")]
    
    public string? SuperheroName { get; set; }

    public string? FullName { get; set; }

    public int? GenderId { get; set; }

    public int? EyeColourId { get; set; }

    public int? HairColourId { get; set; }

    public int? SkinColourId { get; set; }

    public int? RaceId { get; set; }

    public int? PublisherId { get; set; }

    public int? AlignmentId { get; set; }

    public int? HeightCm { get; set; }

    public int? WeightKg { get; set; }

    public virtual Alignment? Alignment { get; set; }

    public virtual Colour? EyeColour { get; set; }

    public virtual Gender? Gender { get; set; }

    public virtual Colour? HairColour { get; set; }

    public virtual Publisher? Publisher { get; set; }

    public virtual Race? Race { get; set; }

    public virtual Colour? SkinColour { get; set; }
}
