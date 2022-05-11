using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppTest.Domain;

public class User
{
    public int Id { get; set; }

    public string Name { get; set; }

    public List<Contact> Contacts { get; set; }
}