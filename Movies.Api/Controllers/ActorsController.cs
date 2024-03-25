using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Api.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Movies.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ActorsController : ControllerBase
{
    private readonly MoviesDbContext _dbContext;

    public ActorsController(MoviesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Načte herce k dispozici
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(Movie))]
    public async Task<IReadOnlyCollection<Actor>> Index(CancellationToken cancellationToken)
    {
        var actors = await _dbContext.Actors.ToListAsync(cancellationToken);
        return actors;
    }

}