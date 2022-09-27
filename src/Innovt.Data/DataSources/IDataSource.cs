﻿// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data

using Innovt.Data.Model;

namespace Innovt.Data.DataSources;

public interface IDataSource
{
    string Name { get; set; }

    Provider Provider { get; }

    string GetConnectionString();
}