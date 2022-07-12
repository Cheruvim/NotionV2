using System.Collections.Generic;

namespace NotionV2.DataServices.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
        
        public int UserId { get; set; }
        public int SectionId { get; set; }
    }
}