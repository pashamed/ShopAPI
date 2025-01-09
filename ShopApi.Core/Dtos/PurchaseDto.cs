using System.ComponentModel.DataAnnotations;

namespace ShopApi.Core.Dtos
{
    public class PurchaseDto
    {
        public int Id { get; set; }

        [Required]
        public DateOnly Date { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal TotalCost { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public List<PurchaseItemDto> PurchaseItems { get; set; }
    }
}