using System.ComponentModel.DataAnnotations;

namespace Movies.Api.RequestModel.Movie;

/// <summary>
/// Smazání herce
/// </summary>
public class DeleteActorRequest
{
    
    /// <summary>
    /// ID Herce
    /// </summary>
    [Required]
    public int ActorId { get; set; }
}