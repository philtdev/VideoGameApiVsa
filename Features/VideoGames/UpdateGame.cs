using Microsoft.AspNetCore.Mvc;
using VideoGameApiVsa.Data;
using MediatR;

namespace VideoGameApiVsa.Features.VideoGames;

public static class UpdateGame
{
    public record Command(int Id, string Title, string Genre, int ReleaseYear) : IRequest<Response?>;
    public record Response(int Id, string Title, string Genre, int ReleaseYear);

    public class Handler(VideoGameDbContext context) : IRequestHandler<Command, Response?>
    {
        public async Task<Response?> Handle(Command request, CancellationToken cancellationToken)
        {
            var videoGame = await context.VideoGames.FindAsync(request.Id);

            if (videoGame == null)
                return null;

            videoGame.Title = request.Title;
            videoGame.Genre = request.Genre;
            videoGame.ReleaseYear = request.ReleaseYear;

            await context.SaveChangesAsync(cancellationToken);

            return new Response(videoGame.Id, videoGame.Title, videoGame.Genre, videoGame.ReleaseYear);
        }
    }
}

[ApiController]
[Route("api/games")]
public class UpdateGameController(ISender sender) : ControllerBase
{
    [HttpPut("{id}")]
    public async Task<ActionResult<UpdateGame.Response>> UpdateGame(int id, UpdateGame.Command command)
    {
        if (id != command.Id)
            return BadRequest("Id in the URL does not match the Id in the request body.");

        var response = await sender.Send(command);

        if (response is null)
            return NotFound("Video game with given Id not found.");

        return Ok(response);
    }
}
