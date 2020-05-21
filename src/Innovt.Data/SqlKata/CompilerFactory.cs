using Innovt.Data.DataSources;
using Innovt.Data.Model;
using SqlKata.Compilers;

namespace Innovt.Data.SqlKata
{
    public static class CompilerFactory
    {
        public static Compiler Create(IDataSource dataSource)
        {
            return dataSource.Provider switch
            {
                Provider.MsSql => (Compiler) new SqlServerCompiler(),
                Provider.PostgreSqL => new PostgresCompiler(),
                _ => new SqlServerCompiler()
            };
        }
    }
}