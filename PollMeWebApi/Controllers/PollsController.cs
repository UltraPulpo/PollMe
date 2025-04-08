using Microsoft.AspNetCore.Mvc;
using PollMeWebApi.Models;

namespace PollMeWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PollsController : ControllerBase
{
    private static List<Poll> Polls =
    [
        new() { Id = 1, Name = "poll-test-1", Description = "This is a test poll" },
        new() { Id = 2, Name = "poll-test-2", Description = "This is another test poll" },
    ];

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
