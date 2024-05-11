namespace Cards.Models.DTOs
{
    public class AddNumericRequestDto
    {
        public int Value { get; set; }
        public required string Name { get; set; }


    }

    public class UpdateNumericRequestDto
    {
        public int Value { get; set; }
        public required string Name { get; set; }


    }

    public class AddMoneyRequestDto
    {
        public int Value { get; set; }

    }

}
