using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using HtmlAgilityPack;
using ServiceStack.Html;

namespace Innovt.PublishToolbelt.Minify
{
    public class MinifyManager
    {
        private string appPath = string.Empty;
        private string assetsLocalPath = string.Empty;
        private string assetsDevelopmentUrl = string.Empty;
        private string assetsProductionUrl = string.Empty;
        private bool bundleFiles = false;
        private string bundleFolderName = "bundle";

        private string SaveBundleFile(string content, string format)
        {
            var md5 = MD5.Create();

            var contentByte = System.Text.Encoding.ASCII.GetBytes(content);

            byte[] hash = md5.ComputeHash(contentByte);

            StringBuilder sb = new StringBuilder();
            foreach (var t in hash)
            {
                sb.Append(t.ToString("X2"));
            }

            var bundleId = sb.ToString();

            var deployPath = Path.Combine(assetsLocalPath, bundleFolderName);
            var bundlePath = Path.Combine(deployPath, $"{bundleId}.{format}");

            if (File.Exists(bundlePath))
                return bundlePath;

            if (!Directory.Exists(deployPath))
                Directory.CreateDirectory(deployPath);

            File.WriteAllText(bundlePath, content);

            return bundlePath;
        }
        private void ReplaceElement(ref string htmlDocument, string elementOuterHtml, string replaceBy)
        {
            if (htmlDocument.IndexOf(elementOuterHtml) < 0)
            {
                elementOuterHtml = elementOuterHtml.Replace(" />", "/>");
            }


            if (htmlDocument.IndexOf(elementOuterHtml) < 0)
                throw new Exception("Element Not found.");

            htmlDocument = htmlDocument.Replace(elementOuterHtml, replaceBy);
        }

        private void BundleStyles(ref string htmlDocument, HtmlNode mainNode)
        {
            var styles = (mainNode.NodeType == HtmlNodeType.Document)
                ? mainNode.SelectNodes($"//link[@rel='stylesheet'][starts-with(@href,'{assetsLocalPath}')]")
                : mainNode.SelectNodes($"{mainNode.XPath}//link[@rel='stylesheet'][starts-with(@href,'{assetsLocalPath}')]");

            if (styles == null || !styles.Any())
                return;

            var bundleContent = new StringBuilder();

            for (int i = 0; i < styles.Count; i++)
            {
                var styleNode = styles[i];

                var isMediaPrint = (styleNode.Attributes["media"] != null && styleNode.Attributes["media"].Value == "print");

                if (isMediaPrint)
                    continue;

                var filePath = styleNode.Attributes["href"].Value;

                var isLast = (i == styles.Count - 1);

                if (File.Exists(filePath))
                {
                    var rawContent = File.ReadAllText(filePath);
                    bundleContent.Append(rawContent);
                    if (!isLast)
                    {
                        ReplaceElement(ref htmlDocument, styleNode.OuterHtml, "");
                    }
                }
                else
                {
                    Console.WriteLine($"File {filePath} not found at local Asset");
                }

                //if is last element
                if (isLast)
                {
                    var finalBundle = Minifiers.Css.Compress(bundleContent.ToString());
                    var bundleUrl = SaveBundleFile(finalBundle, "css");
                    ReplaceElement(ref htmlDocument, styleNode.OuterHtml, $@"<link rel='stylesheet' href='{bundleUrl}'/>");
                }
            }
        }

        private void BundleScripts(ref string htmlDocument, HtmlNode mainNode)
        {
            var scripts = (mainNode.NodeType == HtmlNodeType.Document)
                ? mainNode.SelectNodes($"//script[starts-with(@src,'{assetsLocalPath}')]")
                : mainNode.SelectNodes($"{mainNode.XPath}//script[starts-with(@src,'{assetsLocalPath}')]");

            if (scripts == null || !scripts.Any())
                return;

            var bundleContent = new StringBuilder();

            for (int i = 0; i < scripts.Count; i++)
            {
                var scriptNode = scripts[i];
                var isLast = (i == scripts.Count - 1);

                var filePath = scriptNode.Attributes["src"].Value;

                if (File.Exists(filePath))
                {
                    var rawContent = File.ReadAllText(filePath);
                    bundleContent.Append(rawContent);

                    if (!isLast)
                    {
                        ReplaceElement(ref htmlDocument, scriptNode.OuterHtml, "");
                    }
                }
                else
                {
                    Console.WriteLine($"File {filePath} not found at local Asset");
                }

                //if is last element
                if (isLast)
                {
                    var finalBundle = bundleContent.ToString();
                    finalBundle = Minifiers.JavaScript.Compress(finalBundle);
                    var bundleUrl = SaveBundleFile(finalBundle, "js");

                    ReplaceElement(ref htmlDocument, scriptNode.OuterHtml, $@"<script src='{bundleUrl}'></script>");
                }
            }
        }

        private void BundleAssets(ref string htmlDocument, HtmlNode node)
        {
            BundleStyles(ref htmlDocument, node);

            BundleScripts(ref htmlDocument, node);
        }

        public void Minify(List<Argument> args)
        {
            appPath = ArgumentHelper.Find("-AppPath", args);
            assetsLocalPath = ArgumentHelper.Find("-AssetsLocalPath", args);
            assetsDevelopmentUrl = ArgumentHelper.Find("-AssetsDevUrl", args);
            assetsProductionUrl = ArgumentHelper.Find("-AssetsProdUrl", args);

            if (string.IsNullOrEmpty(appPath) || string.IsNullOrEmpty(assetsLocalPath) ||
                string.IsNullOrEmpty(assetsDevelopmentUrl) ||
                string.IsNullOrEmpty(assetsProductionUrl))
            {
                throw new Exception("The parameres are not defined. -AppPath,-AssetsLocalPath,-AssetsDevUrl,-AssetsProdUrl, BundleFiles( optional, false is default)");
            }

            var hasBundleArg = ArgumentHelper.Find("-BundleFiles", args);

            if (hasBundleArg != null)
            {
                bool.TryParse(hasBundleArg, out bundleFiles);
            }

            var htmlFiles = Directory.GetFiles(appPath, "*.cshtml", SearchOption.AllDirectories);

            foreach (var htmlFile in htmlFiles)
            {
                string rawContent = File.ReadAllText(htmlFile, Encoding.UTF8);

                //Will change the url to work loccally
                rawContent = rawContent.Replace(assetsDevelopmentUrl, assetsLocalPath);

                if (bundleFiles)
                {
                    var document = new HtmlDocument() { OptionCheckSyntax = false, OptionWriteEmptyNodes = true };
                    document.LoadHtml(rawContent);

                    var head = document.DocumentNode.SelectSingleNode("/html/head");

                    if (head != null)
                    {
                        BundleAssets(ref rawContent, head);
                    }

                    var body = document.DocumentNode.SelectSingleNode("/html/body") ?? document.DocumentNode;

                    BundleAssets(ref rawContent, body);
                }

                rawContent = rawContent.Replace(assetsLocalPath + $@"\{bundleFolderName}\", assetsLocalPath + $@"/{bundleFolderName}/");
                rawContent = rawContent.Replace(assetsLocalPath, assetsDevelopmentUrl);
                rawContent = rawContent.Replace(assetsDevelopmentUrl, assetsProductionUrl);

                var htmlCompressor = (HtmlCompressor)Minifiers.HtmlAdvanced;
                htmlCompressor.RemoveComments = true;
                htmlCompressor.PreserveLineBreaks = true;
                rawContent = htmlCompressor.Compress(rawContent);
                File.WriteAllText(htmlFile, rawContent, Encoding.UTF8);
            }
        }
    }
}
