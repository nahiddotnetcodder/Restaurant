namespace RMS.Models
{
    public class NameIdPair
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class NameIdAccountGroupPair
    {
        public int Id { get; set; }
        public int AccountGroupId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public string Unit { get; set; }
    }
}
