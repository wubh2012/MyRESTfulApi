using System.ComponentModel.DataAnnotations;

namespace MyRestful.Core.Entity
{
    public class TodoItem
    {
        public long Id { get; set; }
        
        public string Name { get; set; }

        public bool IsComplete { get; set; }
    }
}