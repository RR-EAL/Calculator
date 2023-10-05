using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class UserRequestModel
    {
        [Required]
        public string? ForeignKey { get; set; }
        [Required]
        public string? Forename { get; set; }
        [Required]
        public string? Surname { get; set; }
        public int? CardId { get; set; }
        public int? Pin { get; set; }
        public DateTime? PinExpiryDate { get; set; }
        public Boolean? ActiveFlag { get; set; }
        public DateTime? ActiveDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? Detail { get; set; }


    }
}
