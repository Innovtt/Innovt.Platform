// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core.Test

using System;
using Innovt.Core.Utilities;
using NUnit.Framework;

namespace Innovt.Core.Test;

[TestFixture]
public class ExtensionsTests
{
    [Test]
    public void CheckRemoveAccents()
    {
        var actual = ("Hoje é o dia mais feliz da minha vida. Espero que isso funcione. " +
                      "Esse código foi baixado da WEB e ainda não tenho como testar sem´aspas '").NormalizeText();

        var expected =
            "Hoje e o dia mais feliz da minha vida Espero que isso funcione Esse codigo foi baixado da WEB e ainda nao tenho como testar sem aspas";


        Assert.That(expected, Is.EqualTo(expected));
    }
}