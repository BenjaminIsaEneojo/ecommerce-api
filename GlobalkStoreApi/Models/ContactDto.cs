using System.ComponentModel.DataAnnotations;

namespace GlobalkStoreApi.Models
{
    public class ContactDto
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; } = "";
        [Required, MaxLength(100)]
        public string LastName { get; set; } = "";
        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = "";
        [MinLength(11), MaxLength(15)]
        public string? Phone { get; set; } = "";
        public int SubjectId { get; set; }
        [Required, MinLength(20), MaxLength(4000)]
        public string Message { get; set; } = "";
    }
}
