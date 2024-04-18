using Newtonsoft.Json;

//Los comentarios está para que compile y no pete.
namespace MyTracker
{
    public interface ISerializer
    {
        public string serialize(TrackerEvent trackerEvent);
    }

    public class JsonSerializer : ISerializer
    {
        public string serialize(TrackerEvent trackerEvent)
        {
            return JsonConvert.SerializeObject(trackerEvent);
        }
    }
}
