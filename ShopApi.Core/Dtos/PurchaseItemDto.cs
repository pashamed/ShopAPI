using System.ComponentModel.DataAnnotations;

namespace ShopApi.Core.Dtos
{
    public class PurchaseItemDto
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}