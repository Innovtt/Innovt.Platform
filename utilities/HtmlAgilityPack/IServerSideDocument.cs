using System;
using System.Collections.Generic;
using System.Text;

namespace HtmlAgilityPack
{
    interface IServerSideDocument
    {
        void Load(string content);

        bool IsServerSideCode(int charPosition, int line);
    }
}
