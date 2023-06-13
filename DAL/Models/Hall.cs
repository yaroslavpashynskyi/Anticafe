namespace DAL.Models
{
    public class Hall
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Turnkey { get; set; } = false;
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
    }
}