using PollMeWebApi.Interfaces;
using PollMeWebApi.Models;
using System.Text.Json;

namespace PollMeWebApi.Services;

public class PollService : IPollService
{
    private readonly string _dataFilePath;
    private readonly FileSystemWatcher _fileWatcher;
    private DateTime _lastReadTime = DateTime.MinValue;
    private readonly object _lock = new object();
    private List<Poll> _polls;

    // Cache the JsonSerializerOptions instance
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        WriteIndented = true // Makes the JSON file more readable
    };

    public PollService(string dataFilePath)
    {
        _dataFilePath = dataFilePath;
        _polls = LoadPolls();
        _lastReadTime = DateTime.MinValue;

        Console.WriteLine($"Watching directory: {Path.GetDirectoryName(_dataFilePath)}");
        Console.WriteLine($"Watching file: {Path.GetFileName(_dataFilePath)}");

        _fileWatcher = new FileSystemWatcher(Path.GetDirectoryName(_dataFilePath) ?? string.Empty)
        {
            Filter = Path.GetFileName(_dataFilePath),
            NotifyFilter = NotifyFilters.LastWrite
        };

        _fileWatcher.Changed += OnFileChanged;
        _fileWatcher.Renamed += OnFileChanged;

        _fileWatcher.EnableRaisingEvents = true;
    }

    public Poll CreatePoll(Poll newPoll)
    {
        lock (_lock)
        {
            newPoll.Id = _polls.Count > 0 ? _polls.Max(p => p.Id) + 1 : 1;
            newPoll.Name += newPoll.Id.ToString();
            _polls.Add(newPoll);
        }

        SavePollsToFile();

        return newPoll;
    }

    public bool DeletePoll(int id)
    {
        lock (_lock)
        {
            var pollToDelete = _polls.FirstOrDefault(p => p.Id == id);
            if (pollToDelete == null)
                return false;
            _polls.Remove(pollToDelete);
        }
        
        SavePollsToFile();

        return true;
    }

    public Poll? GetPollById(int id)
    {
        return _polls.FirstOrDefault(p => p.Id == id);
    }

    public List<Poll> GetPolls()
    {
        return _polls;
    }

    private List<Poll> LoadPolls()
    {
        try
        {
            var jsonData = File.ReadAllText(_dataFilePath);
            var polls = JsonSerializer.Deserialize<List<Poll>>(jsonData);

            Console.WriteLine("Polls reloaded from file.");
            return polls ?? [];
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"File '{_dataFilePath}' not found: {ex.Message}");
            return [];
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Invalid JSON format: {ex.Message}");
            return [];
        }
        catch (IOException ex)
        {
            Console.WriteLine($"IO error when loading polls from file: {ex.Message}");
            return [];
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while loading polls: {ex.Message}");
            return [];
        }
    }

    private void OnFileChanged(object sender, FileSystemEventArgs e)
    {
        Console.WriteLine($"File change detected: {e.FullPath}");
        var now = DateTime.Now;
        if ((now - _lastReadTime).TotalMilliseconds > 500) // 500ms debounce
        {
            Console.WriteLine($"Processing file change: {e.FullPath}");
            _polls = LoadPolls();
            _lastReadTime = now;
        }
    }

    private void SavePollsToFile()
    {
        try
        {
            _fileWatcher.EnableRaisingEvents = false;

            var jsonData = JsonSerializer.Serialize(_polls, _jsonSerializerOptions);

            File.WriteAllText(_dataFilePath, jsonData);

            Console.WriteLine("Polls successfully saved to file.");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Permission denied when saving polls to file: {ex.Message}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"IO error when saving polls to file: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred while saving polls:: {ex.Message}");
        }
        finally
        {
            _fileWatcher.EnableRaisingEvents = true;
        }
    }
}
