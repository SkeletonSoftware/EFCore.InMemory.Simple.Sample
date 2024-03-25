using System.ComponentModel.DataAnnotations;

namespace Movies.Api.RequestModel.Movie;

/// <summary>
/// Přidání movieu
/// </summary>
public class AddMovieRequest
{
    /// <summary>
    /// Název movieu
    /// </summary>
    [Required]
    public string? Title { get; set; }

    /// <summary>
    /// Popis
    /// </summary>
    [Required]
    public string? Description { get; set; }
}