using System;

namespace MyTracker
{
    public enum EventType
    {
        noType, sessionStart, sessionEnd, gameStart, gameEnd, roundStart, cardStateChange
    }
    public abstract class TrackerEvent
    {
        protected EventType eventType = EventType.noType;
        public EventType EventType { get => eventType; private set => eventType = value; }

        protected long timeStamp = 0;
        public long TimeStamp { get => timeStamp; private set => timeStamp = value; }

        public TrackerEvent() 
        {
            DateTimeOffset dto = new DateTimeOffset(DateTime.UtcNow);
            TimeStamp = dto.ToUnixTimeMilliseconds();
        }
    }
}
