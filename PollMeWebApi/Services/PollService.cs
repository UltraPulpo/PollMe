using PollMeWebApi.Interfaces;
using PollMeWebApi.Models;
using System.Text.Json;

namespace PollMeWebApi.Services;

public class PollService : IPollService
{
    private readonly string _dataFilePath;
    private List<Poll> _polls;
    private readonly FileSystemWatcher _fileWatcher;
    private DateTime _lastReadTime = DateTime.MinValue;

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

    public List<Poll> GeneratePolls()
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
            // Handle the specific case where the file is missing
            Console.WriteLine($"File '{_dataFilePath}' not found: {ex.Message}");
            return []; // Return an empty list as a fallback
        }
        catch (JsonException ex)
        {
            // Handle invalid JSON format
            Console.WriteLine($"Invalid JSON format: {ex.Message}");
            return [];
        }
        catch (Exception ex)
        {
            // Handle any other unexpected errors
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
}
