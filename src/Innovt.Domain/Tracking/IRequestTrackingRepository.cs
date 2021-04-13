// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Threading.Tasks;

namespace Innovt.Domain.Tracking
{
    public interface IRequestTrackingRepository
    {
        Task AddTracking(RequestTracking tracking);
    }
}