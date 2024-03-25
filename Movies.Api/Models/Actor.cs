using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Movies.Api.Models;

/// <summary>
/// Třída reprezentující tabulku "Herec"
/// </summary>
public class Actor
{
    /// <summary>
    /// Primární klíč
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Jméno
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Přijmení
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Ve kterých movieech hraje herec
    /// </summary>
    [JsonIgnore]
    public ICollection<Movie> Movies { get; set; } = null!;
}