using System.ComponentModel.DataAnnotations;

namespace ShopApi.Core.Dtos
{
    public class CustomerDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        public DateOnly RegistrationDate { get; set; }
    }
}