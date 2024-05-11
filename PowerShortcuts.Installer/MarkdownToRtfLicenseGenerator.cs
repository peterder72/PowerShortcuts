using System;
using System.IO;
using RTFExporter;

namespace PowerShortcuts.Installer
{
    internal static class MarkdownToRtfLicenseGenerator
    {
        private const string Separator = "\r\n\r\n";

        public static void ConvertMarkdownToRtf(string markdownPath, string rtfPath)
        {
            var markdownLicense = File.ReadAllText(markdownPath);
            
            using (var doc = new RTFDocument(rtfPath))
            {
                var lastParagraphEndIx = 0;
                int nextParagraphEndIx;
                
                while ((nextParagraphEndIx =
                           markdownLicense.IndexOf(Separator, lastParagraphEndIx, StringComparison.Ordinal)) != -1)
                {
                    var paragraphText =
                        markdownLicense.Substring(
                            lastParagraphEndIx,
                            nextParagraphEndIx - lastParagraphEndIx);

                    GenerateParagraph(doc, paragraphText);

                    lastParagraphEndIx = nextParagraphEndIx + 4;
                }

                GenerateParagraph(doc, markdownLicense.Substring(lastParagraphEndIx));
            }
        }

        private static void GenerateParagraph(RTFDocument doc, string paragraphText)
        {
            var p = doc.AppendParagraph();
            p.style = new RTFParagraphStyle(Alignment.Left, new Indent(0, 0, 0));

            var text = p.AppendText(paragraphText);
            text.SetStyle(new Color(0, 0, 0), 10);
        }
    }
}