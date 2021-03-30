namespace Innovt.Domain.Core.Model
{
    public class SimpleVo<T> : ValueObject<T> where T : struct
    {
        public string Description { get; set; }

        public SimpleVo()
        {
        }

        public SimpleVo(T id, string description)
        {
            Id = id;
            Description = description;
        }
    }
}