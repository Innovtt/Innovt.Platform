// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Threading.Tasks;

namespace Innovt.Core.Cache
{
    public interface ICacheService
    {
        Task<T> GetObject<T>(string key);

        Task PutObject<T>(string key, T obj);
    }
}