using System.Net;
using Microsoft.AspNetCore.Mvc;
using NOS.Engineering.Challenge.API.Models;
using NOS.Engineering.Challenge.Managers;

namespace NOS.Engineering.Challenge.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ContentController : Controller
{
    private readonly IContentsManager _manager;
    private readonly ILogger<ContentController> _logger;
    public ContentController(IContentsManager manager)
    {
        _manager = manager;

        using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
            .SetMinimumLevel(LogLevel.Trace)
            .AddConsole());

        _logger = loggerFactory.CreateLogger<ContentController>();
    }

    [HttpGet]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetManyContents()
    {
        _logger.LogInformation("Called GetManyContents endpoint at {DT}",
            DateTime.UtcNow.ToLongTimeString());

        var contents = await _manager.GetManyContents().ConfigureAwait(false);

        if (!contents.Any())
            return NotFound();

        return Ok(contents);
    }

    [HttpGet("filter")]
    public async Task<IActionResult> GetManyContentsFilter(
        [FromQuery] ContentFilterInput filters
        )
    {
        _logger.LogInformation("Called GetManyContentsFilter endpoint at {DT}",
            DateTime.UtcNow.ToLongTimeString());

        var contents = await _manager.GetManyContentsFilter(filters.Title ?? "", filters.Genre ?? "").ConfigureAwait(false);

        if (!contents.Any())
            return NotFound();

        return Ok(contents);
    }

    [HttpGet("{id}")]
    [ResponseCache(Duration = 10)]
    public async Task<IActionResult> GetContent(Guid id)
    {
        _logger.LogInformation("Called GetContent endpoint at {DT}",
            DateTime.UtcNow.ToLongTimeString());

        var content = await _manager.GetContent(id).ConfigureAwait(false);

        if (content == null)
            return NotFound();

        return Ok(content);
    }

    [HttpPost]
    public async Task<IActionResult> CreateContent(
        [FromBody] ContentInput content
        )
    {
        _logger.LogInformation("Called CreateContent endpoint at {DT}",
            DateTime.UtcNow.ToLongTimeString());

        var createdContent = await _manager.CreateContent(content.ToDto()).ConfigureAwait(false);

        return createdContent == null ? Problem() : Ok(createdContent);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateContent(
        Guid id,
        [FromBody] ContentInput content
        )
    {
        _logger.LogInformation("Called UpdateContent endpoint at {DT}",
            DateTime.UtcNow.ToLongTimeString());

        var updatedContent = await _manager.UpdateContent(id, content.ToDto()).ConfigureAwait(false);

        return updatedContent == null ? NotFound() : Ok(updatedContent);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContent(
        Guid id
    )
    {
        _logger.LogInformation("Called DeleteContent endpoint at {DT}",
            DateTime.UtcNow.ToLongTimeString());

        var deletedId = await _manager.DeleteContent(id).ConfigureAwait(false);
        return Ok(deletedId);
    }

    [HttpPost("{id}/genre")]
    public async Task<IActionResult> AddGenres(
        Guid id,
        [FromBody] IEnumerable<string> genre
    )
    {
        _logger.LogInformation("Called AddGenres endpoint at {DT}",
            DateTime.UtcNow.ToLongTimeString());

        var updatedContent = await _manager.AddGenres(id, genre).ConfigureAwait(false);

        return updatedContent == null ? NotFound() : Ok(updatedContent);
    }

    [HttpDelete("{id}/genre")]
    public async Task<IActionResult> RemoveGenres(
        Guid id,
        [FromBody] IEnumerable<string> genre
    )
    {
        _logger.LogInformation("Called RemoveGenres endpoint at {DT}",
            DateTime.UtcNow.ToLongTimeString());

        var updatedContent = await _manager.RemoveGenres(id, genre).ConfigureAwait(false);

        return updatedContent == null ? NotFound() : Ok(updatedContent);
    }
}