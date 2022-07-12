using System.Collections.Generic;
using NotionV2.DataServices.Models;
using NotionV2.Models.Account;

namespace NotionV2.Models
{
    public class HomeViewMode
    {
        public AccountViewModel User { get; set; } = new();
        public List<Section> Sections { get; set; } = new();
        public List<Note> Notes { get; set; } = new();
    }
}