using System.Collections.Generic;

namespace NotionV2.DataServices.Models
{
    public class User
    {
        public int Id { get; set; }
        public bool IsAdmin { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public List<SectionOnUserLinker> Linkers { get; set; }
    }
}