using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olas_Spider_Translator
{
    internal class urlInfo
    {
        public string URL { get; set; } = string.Empty;
        public string OriginalText { get; set; } = string.Empty;
        public string TranslateTo { get; set; } = string.Empty;
        public string TranslatedText { get; set; } = string.Empty;
    }
}
