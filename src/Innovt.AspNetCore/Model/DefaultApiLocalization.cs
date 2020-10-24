using System;
using System.Collections.Generic;
using System.Globalization;

namespace Innovt.AspNetCore.Model
{
    public class DefaultApiLocalization
    {
        public List<CultureInfo> SupportedCultures{ get; set; }

        public Type DefaultLocalizeResource { get; set; }
        public CultureInfo RequestCulture { get; set; }

       public DefaultApiLocalization()
       {
           RequestCulture = new CultureInfo("pt-BR");
           AddSupportedCulture("pt-br").AddSupportedCulture("en-US");
       }

        protected DefaultApiLocalization AddSupportedCulture(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            SupportedCultures ??= new List<CultureInfo>();

            SupportedCultures.Add(CultureInfo.GetCultureInfo(name));

            return this;
        }
    }
}