namespace Innovt.Domain.Model
{
    public class SimpleVO
    {
        public string Id  { get; set; }
        public string Description { get; set; }

        public SimpleVO()
        {
            
        }

        public SimpleVO(string id, string description)
        {
            Id = id;
            Description = description;
        }
    }
}