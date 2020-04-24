namespace Innovt.Data
{
    public interface IDataSource
    {
        string GetConnectionString();
    }

    public interface IDataSource<T> :IDataSource where  T: class
    {
      
    }
}
