// Innovt Company
// Author: Michel Borges
// Project: ConsoleAppTest

using System.Collections.Generic;

namespace ConsoleAppTest.Domain;

public class User
{
    public int Id { get; set; }

    public string Name { get; set; }

    public List<Contact> Contacts { get; set; }
}