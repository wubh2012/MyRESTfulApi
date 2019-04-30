﻿using System.ComponentModel.DataAnnotations;

namespace MyRESTfulApi.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }
}