using System.Collections.Generic;
using NotionV2.DataServices.Models;

namespace NotionV2.Models
{
    public class HomeViewMode
    {
        public User User { get; set; } = new();
        public List<Note> Notes { get; set; } = new();
    }
}