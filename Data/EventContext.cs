using Microsoft.EntityFrameworkCore;
using EventManagementApp.Models;

namespace EventManagementApp.Data
{
    public class EventContext : DbContext
    {
        public EventContext(DbContextOptions<EventContext> options) : base(options) { }

        public DbSet<Event> Events { get; set; }
    }
}
