using System;
using System.Collections.Generic;
using System.Linq;

namespace nearby_tickets_algorithm
{
    class Event
    {
        private readonly int MIN_ID = 1;
        private readonly int MAX_ID = 3;

        public int ID { get; private set; }
        public List<Ticket> Tickets { get; private set; }

        private Random rnd;

        /// <summary>
        /// Constructor for Event.
        /// It auto-generates an unique identifier, not present on the
        /// list passed as parameter.
        /// </summary>
        /// <param name="usedIDs">List of integers of used IDs.</param>
        public Event(List<int> usedIDs)
        {
            do
            {
                rnd = new Random();
                ID = rnd.Next(MIN_ID, MAX_ID + 1); // 1 - 3
            } while (usedIDs.Contains(ID));
            usedIDs.Add(ID);
            
            Tickets = new List<Ticket>();
        }

        /// <summary>
        /// Add new ticket to current Event, with a price greater than 0.
        /// </summary>
        /// <param name="price"></param>
        /// <returns>True if successful, False otherwise.</returns>
        public bool AddTicket(double price)
        {
            if(price > 0)
            {
                Ticket tkt = new Ticket();
                tkt.Price = price;
                Tickets.Add(tkt);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Print out all tickets for current Event in the form of
        /// "Ticket for event ID: price".
        /// </summary>
        public void PrintTickets()
        {
            foreach (Ticket tick in Tickets)
                Console.Write("\n\tTicket for event " + ID + ": " + tick.Price.ToString("F"));
            Console.WriteLine();
        }

        /// <summary>
        /// Get the Ticket with the least price.
        /// If the list is empty, it returns null.
        /// </summary>
        /// <returns>Ticket object with the least price; null if there are no tickets.</returns>
        public Ticket GetMinTicket()
        {
            if (Tickets.Count > 0)
            {
                double min = Tickets.Min(ticket => ticket.Price);
                return (Tickets.Count > 0) ? Tickets.First(ticket => ticket.Price == min) : null;
            }
            else
                return null;
        }
    }
}
