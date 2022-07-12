namespace NotionV2.DataServices.Models
{
    public class SectionOnUserLinker
    {
        public int Id { get; set; }
        public int SectionId { get; set; }
        public int UserId { get; set; }
        public int Role { get; set; }
    }
}