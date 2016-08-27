using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace nearby_tickets_algorithm
{
    class MainProgram
    {
        private readonly int TOTAL_TICKETS = 5;
        private readonly int MIN_X = -10;
        private readonly int MAX_X = 10;
        private readonly int MIN_Y = -10;
        private readonly int MAX_Y = 10;
        private readonly int POINTS = 20;

        private Coordinate[,] data;

        private int orig_x;
        private int orig_y;

        // Holds selected Tickets. Not used now, but maybe in the future
        private List<Ticket> lst_tkt;

        public MainProgram()
        {
            lst_tkt = new List<Ticket>();
        }

        /// <summary>
        /// Run the program by asking for input and computing the nearest cheapest tickets.
        /// </summary>
        public void Run()
        {
            // Fill the grid with random data
            RandomFiller rndFill = new RandomFiller(MIN_X, MAX_X, MIN_Y, MAX_Y, POINTS);
            rndFill.CreateData();
            data = rndFill.Data;
            
            int x = 0, y = 0;
            string line;
            bool condition = false;
            Regex regex;
            Match match, match1;

            Console.Write("\n\n\n");
            do
            {
                Console.Write("Write down your location as pair of coordinates, e.g. (4, 2) with one whitespace: ");
                line = Console.ReadLine();

                // Analyse input pattern
                regex = new Regex(@"[(]-*\d+[,]\s-*\d+[)]+");
                match = regex.Match(line);
                if (match.Success)
                {
                    // Get x and y from input coordinates
                    regex = new Regex(@"-*\d+");
                    match1 = regex.Match(match.Value);
                    orig_x = Convert.ToInt32(Regex.Match((match1.Value), @"-*\d+").Value);
                    x = orig_x + Math.Abs(MIN_X);

                    regex = new Regex(@"[ ]-*\d+");
                    match1 = regex.Match(match.Value);
                    orig_y = Convert.ToInt32(match1.Value);
                    y = orig_y + Math.Abs(MIN_Y);

                    if(orig_x >= MIN_X && orig_x <= MAX_X &&
                        orig_y >= MIN_Y && orig_y <= MAX_Y)
                        condition = true;
                }
            } while (!condition);
            
            FindClosestEvents(x, y);
        }

        int count = 0;
        /// <summary>
        /// Find closest events to given starting coordinates.
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        private void FindClosestEvents(int x, int y)
        {
            Console.WriteLine("Closest events to " + "(" + orig_x + ", " + orig_y + ")");

            // Check possible matches on starting location first
            if (CheckPoint(x, y, 0) == 1)
                return;

            // Find highest height to go through
            int max_heightX = MAX_X + Math.Abs(MIN_X);
            int max_heightY = MAX_Y + Math.Abs(MIN_Y);
            int max_height = (max_heightX >= max_heightY) ? max_heightX : max_heightY;
            for (int h = 1; h < max_height; h++)
            {
                for (int i = 0; i < h + 1; i++)
                {
                    int x1 = x - h + i;
                    int y1 = y - i;

                    int x2 = x + h - i;
                    int y2 = y + i;

                    if (CheckPoint(x1, y1, h) == 1)
                        return;

                    if (CheckPoint(x2, y2, h) == 1)
                        return;
                }
                for (int i = 1; i < h; i++)
                {
                    int x1 = x - i;
                    int y1 = y + h - i;

                    int x2 = x + h - i;
                    int y2 = y - i;

                    if (CheckPoint(x1, y1, h) == 1)
                        return;

                    if (CheckPoint(x2, y2, h) == 1)
                        return;
                }
            }
        }

        /// <summary>
        /// Check particular location for presence of Tickets.
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <param name="h">Height (Manhattan Distance)</param>
        /// <param name="lst_tkt">List of selected tickets</param>
        /// <returns>-1 no match; 0 match found; 1 total number of tickets reached</returns>
        private int CheckPoint(int x, int y, int h)
        {
            
            int result = -1;
            // If x and y are within limits (axis)
            if (CheckBoundaries(x, y))
            {
                // If the location is not null (has something on it)
                if(data[x, y] != null)
                {
                    List<Event> current_evts = data[x, y].Events;
                    Ticket tkt;
                    foreach(Event evt in current_evts)
                    {
                        tkt = evt.GetMinTicket();
                        if (tkt != null)
                        {
                            // Add Ticket to List lst_tkt
                            lst_tkt.Add(tkt);
                            
                            // Print Ticket details
                            Console.WriteLine("Position (" + (x - Math.Abs(MIN_X)) + ", " + (y - Math.Abs(MIN_Y)) + ") \t" +
                                                "Event ID " + evt.ID + "\t" +
                                                "$" + tkt.Price.ToString("F") + "\t" +
                                                "Distance " + h);
                            count++;
                            if (count >= TOTAL_TICKETS) // Exit when count is reached
                                return 1;
                            result = 0;
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Private method only used on CheckPoint.
        /// It checks whether x and y are within boundaries.
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <returns>True if so; otherwise false</returns>
        private bool CheckBoundaries(int x, int y)
        {
            if (x > 0 && x < (MAX_X + Math.Abs(MIN_X)) &&
                y > 0 && y < (MAX_Y + Math.Abs(MIN_Y))) // boundaries
                return true;
            return false;
        }
    }
}
