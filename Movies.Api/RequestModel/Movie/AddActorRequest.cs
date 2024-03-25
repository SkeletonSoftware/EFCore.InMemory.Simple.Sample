using System.ComponentModel.DataAnnotations;

namespace Movies.Api.RequestModel.Movie;

/// <summary>
/// Přidání herce
/// </summary>
public class AddActorRequest
{
    [Required]
    public int ActorId { get; set; }
}