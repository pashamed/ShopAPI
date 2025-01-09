namespace ShopApi.Core.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public DateOnly RegistrationDate { get; set; }
        public ICollection<Purchase> Purchases { get; set; }
    }
}