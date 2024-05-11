namespace Cards.Models.DTOs
{
    public class NumericsDTO
    {
        //exposing attributes from the model
        public int Id { get; set; }
        public int Value { get; set; }

        public required string Name { get; set; }
    }
}
