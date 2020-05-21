namespace Innovt.Core.Collections
{
    public class ParamsWrappers<T>
    {
        public T[] Parameters { get; set; }
        public ParamsWrappers(params T[] parameters)
        {
            this.Parameters = parameters;
        } 
    }
}