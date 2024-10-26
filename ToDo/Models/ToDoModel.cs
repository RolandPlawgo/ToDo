using System.ComponentModel.DataAnnotations;

namespace ToDo.Models
{
    public class ToDoModel
    {
        public int Id { get; set; }
        public DateTime DateAndTimeOfExpiry { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Description { get; set; }
        [Required]
        [Range(0, 100)]
        public int PercentComplete { get; set; }
    }
}
