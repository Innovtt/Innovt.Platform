using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Razor;

namespace HtmlAgilityPack
{
    public class RazorServerSideDocument : IServerSideDocument
    {

        private ParserResults parseResults = null;

        public bool IsServerSideCode(int charPosition, int line)
        {
            if (parseResults == null)
                return false;

            var document = parseResults.Document;

            var isCode = document.Start.CharacterIndex == charPosition && document.Start.LineIndex == line;

            //second check
            if (!isCode)
            {
                isCode =  document.Children.Any(c => c.Start.CharacterIndex == charPosition);
            }

            return isCode;
        }

        public void Load(string content)
        {
            var host = new RazorEngineHost(new CSharpRazorCodeLanguage());

            var engine = new RazorTemplateEngine(host);

            parseResults=  engine.ParseTemplate(new StringReader(content));
        }
    }



}
