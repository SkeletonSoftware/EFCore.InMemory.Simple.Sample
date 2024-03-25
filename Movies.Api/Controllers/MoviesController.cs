using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Api.Models;
using Movies.Api.RequestModel.Movie;
using Swashbuckle.AspNetCore.Annotations;

namespace Movies.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MoviesController : ControllerBase
{
    private readonly MoviesDbContext _dbContext;
    private const string MovieIdKey = "movieId";

    public MoviesController(MoviesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    #region Movies.Api
    
    /// <summary>
    /// Načte moviey
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(Movie))]
    public async Task<IReadOnlyCollection<Movie>> Index(CancellationToken cancellationToken)
    {
        var movies = await _dbContext.Movies
            .Include(m => m.Actors)
            .ToListAsync(cancellationToken);
        return movies;
    }
    
    /// <summary>
    /// Přidá movie
    /// </summary>
    /// <param name="movie"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(int))]
    public async Task<int> Add([FromBody] AddMovieRequest movie, CancellationToken cancellationToken)
    {
        var newMovie = new Movie
        {
            Title = movie.Title,
            Description = movie.Description
        };

        await _dbContext.Movies.AddAsync(newMovie, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return newMovie.Id;
    }
    
    /// <summary>
    /// Smaže movie
    /// </summary>
    /// <param name="movieId"></param>
    /// <param name="cancellationToken"></param>
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(int))]
    [HttpDelete($"{{{MovieIdKey}}}")]
    public async Task<ActionResult> DeleteMovie(
        [FromRoute(Name = MovieIdKey)] [Required] int? movieId, 
        CancellationToken cancellationToken)
    {
        var movie = await _dbContext.Movies.FindAsync(movieId);
        if (movie == null)
        {
           return BadRequest("Film nenalezen");
        }

        _dbContext.Movies.Remove(movie);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok("Film smazany");
    }

    #endregion

    #region Actor

    /// <summary>
    /// Přidá herce do movieu
    /// </summary>
    /// <param name="movieId"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(int))]
    [HttpPost($"{{{MovieIdKey}}}/Actor")]
    public async Task<ActionResult> AddActor(
        [FromRoute(Name = MovieIdKey)] [Required] int movieId, 
        [FromBody] AddActorRequest request, CancellationToken cancellationToken)
    {
        // Movie
        var movie = await _dbContext.Movies
            .Include(m => m.Actors)
            .FirstOrDefaultAsync(m => m.Id == movieId, cancellationToken);
        if (movie == null)
        {
            return BadRequest("Movie nenalezen");
        }
        
        // Herec
        var actor = await _dbContext.Actors.FindAsync(request.ActorId);
        if (actor == null)
        {
            return BadRequest("Herec nenalezen");
        }

        // Zda ve movieu hraje
        if (IsActorInMovie(request.ActorId, movieId))
        {
            return BadRequest("Herec je jiz obsazen");
        }

        // Pridam herce
        movie.Actors.Add(actor);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok("Herec pridan");
    }

    /// <summary>
    /// Odstranó herce z movieu
    /// </summary>
    /// <param name="movieId"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(int))]
    [HttpDelete($"{{{MovieIdKey}}}/Actor")]
    public async Task<ActionResult> DeleteActor(
        [FromRoute(Name = MovieIdKey)] [Required] int movieId, 
        [FromQuery] DeleteActorRequest request, CancellationToken cancellationToken)
    {
        // Kontrola zda je vubec obsazen
        if (!IsActorInMovie(request.ActorId, movieId))
        {
            return BadRequest("Herec v tomto filmu neni obsazen");
        }

        // Najdu movie
        var movie = await _dbContext.Movies
            .Include(movie => movie.Actors)
            .FirstOrDefaultAsync(m => m.Id == movieId, cancellationToken);
        if (movie == null)
        {
            return BadRequest("Film nenalezen");
        }

        // Najdu herce
        var actor = movie.Actors.FirstOrDefault(a => a.Id == request.ActorId);
        if (actor == null)
        {
            return BadRequest("Tento herec v danem filmu nehraje");
        }

        // Odstranim herce
        movie.Actors.Remove(actor);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok("Herec smazan");

    }

    /// <summary>
    /// Načte herce ve movieu
    /// </summary>
    /// <param name="idActor"></param>
    /// <param name="idMovie"></param>
    /// <returns></returns>
    private bool IsActorInMovie(int idActor, int idMovie)
    {
        return _dbContext.Movies.Any(movie => movie.Id == idMovie && movie.Actors.Any(actor =>  actor.Id == idActor));
    } 

    #endregion
    
}