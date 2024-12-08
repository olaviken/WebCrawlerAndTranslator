using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Olas_Spider_Translator
{
    internal class googleTranslater
    {
        /*Var = PascalCase*/
        /*Object and functions = camelCase*/
        public async Task<string> translateText(string Text, string TargetLang)
        {
            /*This function sends a text which is 5000 less and send it to google for translation. Unofficial version of API takes only 5000 char long text. Get a JsonArray in return*/
            string Url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl=auto&tl={TargetLang}&dt=t&q={Uri.EscapeDataString(Text)}";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string Result = await client.GetStringAsync(Url);
                    string Translated = extractTranslation(Result);
                    return Translated;
                }
                catch (HttpIOException ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
        }
        public async Task<string> longTextTranslation(string Text, string TargetLang)
        {
            /*For longer text to be translated we breaks it up into chunks and send each chunk seperately. When the translation returns, it is concatanated with previuosly text. */
            int CharLim = 5000;
            StringBuilder translatedTextBuilder = new StringBuilder();


            for (int i = 0; i < Text.Length; i += CharLim)
            {
                string Chunk = (i + CharLim > Text.Length) ? Text.Substring(i) : Text.Substring(i, CharLim);
                string TranslatedChunk = await translateText(Chunk, TargetLang);
                translatedTextBuilder.Append(TranslatedChunk).Append(" ");
            }
            return translatedTextBuilder.ToString().Trim();
        }

        public static string extractTranslation(string TranslatedRaw)
        {
            /*This takes the json array from google and returns the translated text.*/
            StringBuilder textBuilder = new StringBuilder();
            JArray jsonArray = JArray.Parse(TranslatedRaw);
            foreach (var SentencePair in jsonArray[0])
            {
                if (SentencePair.Type == JTokenType.Array && SentencePair.Count() > 0)
                {
                    textBuilder.Append(SentencePair[0].ToString());
                }
            }
            return textBuilder.ToString().Trim();
        }
    }
}
