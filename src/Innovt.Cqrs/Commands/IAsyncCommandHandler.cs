// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cqrs
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Core.Cqrs.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Cqrs.Commands;

public interface IAsyncCommandHandler<in T> where T : ICommand
{
    Task Handle(T command, CancellationToken cancellationToken = default);
}

public interface IAsyncCommandHandler<in T, TResult> where T : ICommand
{
    Task<TResult> Handle(T command, CancellationToken cancellationToken = default);
}