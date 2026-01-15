namespace DMS.Auth.Feature.JobCard.Entity
{
    public class JobCard
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Status { get; set; }
        public Guid? DealerId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
