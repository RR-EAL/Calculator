using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class UserResponseModel
    {
        public string? PrimaryKey { get; set; }
        [Required]
        public string? ForeignKey { get; set; }
        [Required]
        public string? Forename { get; set; }
        public string? Surname { get; set; }
        public string? CardId { get; set; }
    }
}
