using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Core.Dtos;
using ShopApi.Core.Interfaces;
using ShopApi.Web;
using Swashbuckle.AspNetCore.Annotations;

namespace ShopAPI.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;
        private readonly IValidator<PurchaseCreateDto> purchaseCreateDtoValidator;
        private readonly IValidator<PurchaseUpdateDto> purchaseUpdateDtoValidator;

        public PurchasesController(
            IPurchaseService purchaseService,
            IValidator<PurchaseCreateDto> purchaseCreateDtoValidator,
            IValidator<PurchaseUpdateDto> purchaseUpdateDtoValidator)
        {
            _purchaseService = purchaseService;
            this.purchaseCreateDtoValidator = purchaseCreateDtoValidator;
            this.purchaseUpdateDtoValidator = purchaseUpdateDtoValidator;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Gets all purchases", Description = "Retrieves a list of all purchases.")]
        [SwaggerResponse(200, "A list of purchases", typeof(IEnumerable<PurchaseDto>))]
        public async Task<IActionResult> GetAllPurchases()
        {
            var purchases = await _purchaseService.GetAllPurchasesAsync();
            return Ok(purchases);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Gets a purchase by ID", Description = "Retrieves a purchase by its ID.")]
        [SwaggerResponse(200, "The purchase with the specified ID", typeof(PurchaseDto))]
        [SwaggerResponse(404, "Purchase not found")]
        public async Task<IActionResult> GetPurchaseById(int id)
        {
            var purchase = await _purchaseService.GetPurchaseByIdAsync(id);
            if (purchase == null) return NotFound();
            return Ok(purchase);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new purchase", Description = "Creates a new purchase with the provided data.")]
        [SwaggerResponse(201, "The created purchase", typeof(PurchaseDto))]
        [SwaggerResponse(400, "Invalid purchase data")]
        public async Task<IActionResult> CreatePurchase([FromBody] PurchaseCreateDto purchaseDto)
        {
            var res = await purchaseCreateDtoValidator.ValidateAsync(purchaseDto);
            if (!res.IsValid) return BadRequest(res.ToValidationErrorResponse());

            var createdPurchase = await _purchaseService.CreatePurchaseAsync(purchaseDto);
            return CreatedAtAction(nameof(GetPurchaseById), new { id = createdPurchase.Id }, createdPurchase);
        }

        [HttpPut()]
        [SwaggerOperation(Summary = "Updates an existing purchase", Description = "Updates an existing purchase with the provided data.")]
        [SwaggerResponse(200, "The updated purchase", typeof(PurchaseDto))]
        [SwaggerResponse(400, "Invalid purchase data")]
        [SwaggerResponse(404, "Purchase not found")]
        public async Task<IActionResult> UpdatePurchase([FromBody] PurchaseUpdateDto purchaseDto)
        {
            var res = await purchaseUpdateDtoValidator.ValidateAsync(purchaseDto);
            if (!res.IsValid) return BadRequest(res.ToValidationErrorResponse());

            var updatedPurchase = await _purchaseService.UpdatePurchaseAsync(purchaseDto);
            if (updatedPurchase == null) return NotFound();
            return Ok(updatedPurchase);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Deletes a purchase by ID", Description = "Deletes a purchase by its ID.")]
        [SwaggerResponse(204, "Purchase deleted")]
        [SwaggerResponse(404, "Purchase not found")]
        public async Task<IActionResult> DeletePurchase(int id)
        {
            var result = await _purchaseService.DeletePurchaseAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}