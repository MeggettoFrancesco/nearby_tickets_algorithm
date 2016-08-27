using System;
using System.Collections.Generic;

namespace nearby_tickets_algorithm
{
    class RandomFiller
    {
        private readonly int MAX_NUMBER_EVENTS = 2;
        private readonly int MAX_NUMBER_TICKETS = 5;
        private readonly double MAX_PRICE_TICKETS = 100;
        private readonly double MIN_PRICE_TICKETS = 0.1;

        private int size_x;
        private int size_y;

        private Random rnd;
        public Coordinate[,] Data { get; private set; }
        private int min_x;
        private int max_x;
        private int min_y;
        private int max_y;
        private int points;

        /// <summary>
        /// Constructor for RandomFiller.
        /// </summary>
        /// <param name="min_x">Minimum value of X</param>
        /// <param name="max_x">Maximum value of X</param>
        /// <param name="min_y">Minimum value of Y</param>
        /// <param name="max_y">Maximum value of Y</param>
        /// <param name="points">Number of generated points</param>
        public RandomFiller(int min_x, int max_x, int min_y, int max_y, int points)
        {
            this.min_x = min_x;
            this.max_x = max_x;
            this.min_y = min_y;
            this.max_y = max_y;
            this.points = points;

            // Calculate sizes for Matrix initialization
            size_x = max_x + Math.Abs(min_x); // MAX_X + (-MIN_X)
            size_y = max_y + Math.Abs(min_y);

            Data = new Coordinate[size_x + 1, size_y + 1];
            rnd = new Random();
        }

        /// <summary>
        /// Populate with random data.
        /// It adds Locations, Events and Tickets.
        /// </summary>
        public void CreateData()
        {
            int x, x1;
            int y, y1;
            Coordinate coord;
            List<Event> events;
            int iterations;

            for(int i = 0; i < points; i++)
            {
                // make x and y positive by adding offset (= |min x| or |min y|)
                x = rnd.Next(min_x, max_x + 1);
                x1 = x + Math.Abs(min_x);
                y = rnd.Next(min_y, max_y + 1);
                y1 = y + Math.Abs(min_y);

                // if that location is already filled, repeat
                if (Data[x1, y1] != null)
                    i--;
                else
                {
                    coord = new Coordinate(x1, y1);
                    // Add 1+ Events on Location
                    iterations = rnd.Next(MAX_NUMBER_EVENTS + 1);
                    for (int j = 0; j < iterations; j++)
                        coord.AddEvent();
                    events = coord.Events;

                    // Add 0+ Tickets to each Event
                    foreach(Event evt in events)
                    {
                        iterations = rnd.Next(MAX_NUMBER_TICKETS + 1);
                        for (int j = 0; j < iterations; j++)
                            evt.AddTicket(rnd.NextDouble() * (MAX_PRICE_TICKETS) + MIN_PRICE_TICKETS);
                    }
                    
                    Data[x1, y1] = coord;

                    // Print out all generated data
                    Console.Write((i + 1) + ": (" + x + ", " + y + ") ");
                    foreach (Event evt in events)
                        evt.PrintTickets();
                    Console.WriteLine();
                }
            }
        }
    }
}
