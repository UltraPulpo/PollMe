using Microsoft.AspNetCore.Mvc;
using PollMeWebApi.Interfaces;
using PollMeWebApi.Models;

namespace PollMeWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PollsController : ControllerBase
{
    private readonly IPollService _pollService;
    private List<Poll> Polls;

    public PollsController(IPollService pollService)
    {
        _pollService = pollService;
        Polls = _pollService.GeneratePolls();
    }

    [HttpGet]
    public ActionResult<IEnumerable<Poll>> GetAllPolls()
    {
        return Ok(Polls);
    }

    [HttpGet("{id}")]
    public ActionResult<Poll> GetPollById(int id)
    {
        var poll = Polls.Find(p => p.Id == id);
        if (poll == null)
        {
            return NotFound();
        }
        return Ok(poll);
    }

    [HttpPost]
    public ActionResult<Poll> CreatePoll(Poll poll)
    {
        poll.Id = Polls.Count + 1;
        Polls.Add(poll);
        return CreatedAtAction(nameof(GetPollById), new { id = poll.Id }, poll);
    }

    [HttpPut("{id}")]
    public ActionResult UpdatePoll(int id, Poll updatedPoll)
    {
        var poll = Polls.Find(p => p.Id == id);
        if (poll == null)
        {
            return NotFound();
        }
        poll.Name = updatedPoll.Name;
        poll.Description = updatedPoll.Description;
        // Update other properties as needed
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeletePoll(int id)
    {
        var poll = Polls.Find(p => p.Id == id);
        if (poll == null)
        {
            return NotFound();
        }
        Polls.Remove(poll);
        return NoContent();
    }
}
