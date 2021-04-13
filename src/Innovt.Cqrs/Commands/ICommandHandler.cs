// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cqrs
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Core.Cqrs.Commands;

namespace Innovt.Cqrs.Commands
{
    public interface ICommandHandler<in T> where T : ICommand
    {
        void Handle(T command);
    }

    public interface ICommandHandler<in T, out TResult> where T : ICommand
    {
        TResult Handle(T command);
    }
}