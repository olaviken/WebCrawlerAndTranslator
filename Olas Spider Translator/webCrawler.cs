using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Olas_Spider_Translator
{
    internal class webCrawler
    {
        /*Var = PascalCase*/
        /*Object and functions = camelCase*/

        public List<string> getTextFromSite(string Url)
        {
            /*returns all text from one URL*/
            var web = new HtmlWeb();
            var document = web.Load(Url);
            List<string> text = new List<string>();

            /*Locates the DIV classes named class=Container*/
            string HtmlDefinition = "//div[@class='container']";
            var InnerText = document.DocumentNode.SelectNodes(HtmlDefinition);
            try
            {
                if (InnerText == null)
                {
                    throw new InvalidOperationException("No nodes with class equals to container found");

                }
                else
                {
                    foreach (var node in InnerText)
                    {
                        string Text = node.InnerText;
                        string CleanText = Regex.Replace(Text, @"\s+", " ").Trim();
                        text.Add(CleanText);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return text;
        }

        public string getLangToTranslate(List<string> text)
        {
            /* This function will locate the link for switching between languages norwegian and english. For all NTNU pages this is located in the second DIV with class=Container. More precisely the last word in this DIV. */
            string Language = string.Empty;
            string Header = text[1]; //Header is named so because text[1] is the individual headers div block of the given page.
            string[] HeaderWords = Header.Split(' ');
            string TranslateTo = HeaderWords[HeaderWords.Length - 1];
            try
            {
                if ((TranslateTo == "Norsk") || (TranslateTo == "English"))
                {
                    if (TranslateTo == "Norsk")
                    {
                        Language = "no";
                    }
                    if (TranslateTo == "English")
                    {
                        Language = "en";
                    }
                }
                else
                {
                    throw new InvalidOperationException("Neither english or norwegian was found as a choice on the site");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return Language;
        }

        public string getTextForTranslation(List<string> text)
        {
            /*This function should read locate the specific text to be translated. For all NTNU pages at 11.10.2024 has 4 different DIV with class=container.
             * first is the menu, second= header of page, third = content and fourth is the footer. this function takes 3rd DIV which is the content and delivers it into a string. Error handling connected to check for empty string*/
            string Text = string.Empty;
            string Translatetext = text[2];
            try
            {
                if ((string.IsNullOrEmpty(Translatetext)))
                {
                    throw new InvalidOperationException("The node is null or empty");
                }
                else
                {
                    Text = Translatetext;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return Text;
        }
    }
}
