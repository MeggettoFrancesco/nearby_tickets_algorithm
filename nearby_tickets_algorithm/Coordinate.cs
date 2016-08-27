using System.Collections.Generic;

namespace nearby_tickets_algorithm
{
    class Coordinate
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public List<Event> Events { get; private set; }
        private List<int> EventsIDs;

        /// <summary>
        /// Constructor for Coordinate.
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;

            Events = new List<Event>();
            EventsIDs = new List<int>();
        }

        /// <summary>
        /// Add new Event for location.
        /// </summary>
        /// <returns>List of all Events per location.</returns>
        public List<Event> AddEvent()
        {
            Events.Add(new Event(EventsIDs));
            return Events;
        }
    }
}
