﻿namespace task.Models
{
    public class FolderModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int? ParentId { get; set; }

    }
}
