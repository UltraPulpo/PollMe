using PollMeWebApi.Models;

namespace PollMeWebApi.Interfaces;

public interface IPollService
{
    List<Poll> GeneratePolls();
}
