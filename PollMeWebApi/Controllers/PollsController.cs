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
        Polls = _pollService.GetPolls();
    }

    [HttpGet]
    public ActionResult<IEnumerable<Poll>> GetAllPolls()
    {
        return Ok(_pollService.GetPolls());
    }

    [HttpGet("{id}")]
    public ActionResult<Poll> GetPollById(int id)
    {
        var poll = _pollService.GetPollById(id);
        if (poll == null)
            return NotFound();

        return Ok(poll);
    }

    [HttpPost]
    public ActionResult<Poll> CreatePoll(Poll newPoll)
    {
        var createdPoll = _pollService.CreatePoll(newPoll);
        
        return CreatedAtAction(nameof(GetPollById), new { id = newPoll.Id }, newPoll);
    }

    [HttpDelete("{id}")]
    public ActionResult DeletePoll(int id)
    {
        var isDeleted = _pollService.DeletePoll(id);
        if (!isDeleted)
            return NotFound();
        
        return NoContent();
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


}
