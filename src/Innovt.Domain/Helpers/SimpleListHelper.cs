using System.Collections.Generic;
using Innovt.Domain.Model;

namespace Innovt.Domain.Helpers
{
    public static class SimpleListHelper
    {
        public static List<SimpleVO> YesNoList => new List<SimpleVO>()
        {
            new SimpleVO("1", "Sim"),
            new SimpleVO("0", "Não")
        };

        public static List<SimpleVO> InactiveActiveList => new List<SimpleVO>()
        {
            new SimpleVO("1", "Ativo"),
            new SimpleVO("0", "Inativo")
        };
    }
}