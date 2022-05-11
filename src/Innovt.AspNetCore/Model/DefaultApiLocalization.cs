// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.AspNetCore
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Globalization;

namespace Innovt.AspNetCore.Model;

public class DefaultApiLocalization
{
    public DefaultApiLocalization()
    {
        RequestCulture = new CultureInfo("pt-BR");
        AddSupportedCulture("pt-br").AddSupportedCulture("en-US");
    }

    public IList<CultureInfo> SupportedCultures { get; private set; }

    public Type DefaultLocalizeResource { get; set; }
    public CultureInfo RequestCulture { get; set; }

    protected DefaultApiLocalization AddSupportedCulture(string name)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));

        SupportedCultures ??= new List<CultureInfo>();

        SupportedCultures.Add(CultureInfo.GetCultureInfo(name));

        return this;
    }
}