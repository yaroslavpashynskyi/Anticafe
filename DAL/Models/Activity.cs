using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Activity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        // [ForeignKey(nameof(Hall))]
        public Guid HallId { get; set; }
        public Hall Hall { get; set; }
    }
}