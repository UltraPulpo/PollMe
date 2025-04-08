using PollMeWebApi.Interfaces;
using PollMeWebApi.Models;

namespace PollMeWebApi.Services;

public class PollService : IPollService
{
    private static readonly List<Poll> Polls =
    [
        new() { Id = 1, Name = "poll-test-1", Description = "This is a test poll" },
        new() { Id = 2, Name = "poll-test-2", Description = "This is another test poll" },
    ];

    public List<Poll> GeneratePolls()
    {
        return Polls;
    }
}
