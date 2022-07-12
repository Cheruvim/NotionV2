using System.Collections.Generic;

namespace NotionV2.DataServices.Models
{
    public class Section
    {
        public int Id { get; set; }
        public string IconPath { get; set; }
        public string Name { get; set; }

        public List<SectionOnUserLinker> Linkers { get; set; }
    }
}