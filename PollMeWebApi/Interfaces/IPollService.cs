using PollMeWebApi.Models;

namespace PollMeWebApi.Interfaces;

public interface IPollService
{
    Poll CreatePoll(Poll newPoll);
    bool DeletePoll(int id);
    List<Poll> GetPolls();
    Poll? GetPollById(int id);
}
