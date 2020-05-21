
namespace Innovt.Data.Model
{   public class OrderBy
    {
        public string[] Columns{ get; private set; }
        public bool Ascending { get; private set; }

        public OrderBy(bool ascending, params string[] columns)
        {
            this.Ascending = ascending;
            this.Columns = columns;
        }
    }
}