using System.ComponentModel.DataAnnotations;

namespace Movies.Api.Models;

/// <summary>
/// Třída reprezentující movie
/// </summary>
public class Movie
{
    
    /// <summary>
    /// Primární klíč
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Název movieu
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Popis movieu
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Jací herci hrají ve movieu
    /// </summary>
    public ICollection<Actor> Actors { get; set; }
}