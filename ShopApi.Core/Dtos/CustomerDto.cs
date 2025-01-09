namespace ShopApi.Core.Dtos
{
    public class CustomerDto
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public DateOnly RegistrationDate { get; set; }
    }

    public class CustomerCreateDto
    {
        public string FullName { get; set; }

        public DateOnly DateOfBirth { get; set; }
    }

    public class CustomerUpdateDto
    {
        public int Id { get; set; }

        public string? FullName { get; set; }

        public DateOnly? DateOfBirth { get; set; }
    }
}