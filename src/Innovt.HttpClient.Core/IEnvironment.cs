using System;

namespace Innovt.HttpClient.Core
{
    public interface IEnvironment
    {   
        Uri TransactionUri { get; }

        Uri QueryUri { get; }
    }
}