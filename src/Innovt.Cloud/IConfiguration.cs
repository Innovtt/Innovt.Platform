﻿// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

namespace Innovt.Cloud;

public interface IConfiguration
{
    string SecretKey { get; set; }

    string AccessKey { get; set; }

    string Region { get; set; }
}