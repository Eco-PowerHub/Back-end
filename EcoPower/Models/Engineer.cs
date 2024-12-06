namespace EcoPower.Models
{
    public class Engineer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Specialization { get; set; }
        public ICollection<Support> Supports { get; set; } = new List<Support>();
    }
}
