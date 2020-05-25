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
                Provider.MsSql => new SqlServerCompiler() { UseLegacyPagination = false },
                Provider.PostgreSqL => new PostgresCompiler(),
                _ => new SqlServerCompiler(){ UseLegacyPagination = false },
            };
        }
    }
}