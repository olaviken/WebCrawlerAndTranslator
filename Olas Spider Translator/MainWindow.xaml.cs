using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Olas_Spider_Translator.urlInfo;
using static Olas_Spider_Translator.webCrawler;
using static Olas_Spider_Translator.fileHandling;
using static Olas_Spider_Translator.googleTranslater;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Olas_Spider_Translator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /*Var = PascalCase*/
        /*Object and functions = camelCase*/
        private List<urlInfo> dataOnWebsite = new List<urlInfo>();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private async void btnGetUrlCsv_Click(object sender, RoutedEventArgs e)
        {
            ProgressText.Text = "Henter nettadresser";
            await System.Threading.Tasks.Task.Delay(20);
            fileHandling fileHandling = new fileHandling();

            string FilePath = fileHandling.getCSVFilePath();

            Filepath.Text = FilePath;

            if(string.IsNullOrEmpty(FilePath) )
            {
                ProgressText.Text = "Ingen fil er valgt";
                return;
            }

            List<string> urls = fileHandling.getURL(FilePath);
            foreach (string url in urls)
            {
                urlInfo data = new urlInfo();
                data.URL = url;
                dataOnWebsite.Add(data);
                await System.Threading.Tasks.Task.Delay(20);
                ProgressText.Text = url;
            }
            URLList.ItemsSource = dataOnWebsite;
            ProgressText.Text = "Ferdig";
        }

        private async void btnTranslate_Click(object sender, RoutedEventArgs e)
        {
            ProgressText.Text = "Starter oversettelse";
            await System.Threading.Tasks.Task.Delay(20);

            googleTranslater translater = new googleTranslater();
            for (int i = 0; i < dataOnWebsite.Count; i++)
            {
                dataOnWebsite[i].TranslatedText = await translater.longTextTranslation(dataOnWebsite[i].OriginalText, dataOnWebsite[i].TranslateTo);
                ProgressText.Text = $"Translating text: {i + 1}/{dataOnWebsite.Count}";
                await System.Threading.Tasks.Task.Delay(20);
            }
            if (URLList.SelectedItem != null)
            {
                urlInfo seletedItem = URLList.SelectedItem as urlInfo;
                OriginalLang.Text = seletedItem.OriginalText;
                Translation.Text = seletedItem.TranslatedText;
                Language.Text = seletedItem.TranslateTo;
            }
            ProgressText.Text = "Ferdig oversette";
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            fileHandling fileHandling = new fileHandling();
            fileHandling.saveJsonFile(dataOnWebsite);
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            string Text = Translation.Text;
            if (!string.IsNullOrEmpty(Text))
            {
                Clipboard.SetText(Text);
            }
            else
            {
                MessageBox.Show("There is no text in the field for translated text");
            }
        }

        private async void btnGetText_Click(object sender, RoutedEventArgs e)
        {
            ProgressText.Text = "Henter tekst fra nettsider";
            await System.Threading.Tasks.Task.Delay(20);

            webCrawler crawler = new webCrawler();

            for (int i = 0; i < dataOnWebsite.Count; i++)
            {

                List<string> TextFromSite = crawler.getTextFromSite(dataOnWebsite[i].URL);
                string UncleanText = crawler.getTextForTranslation(TextFromSite);
                UncleanText = UncleanText.Replace(@"\\", @"\");
                UncleanText = Regex.Unescape(UncleanText);

                dataOnWebsite[i].OriginalText = UncleanText.Replace("&nbsp;", " ");
                dataOnWebsite[i].TranslateTo = crawler.getLangToTranslate(TextFromSite);
                ProgressText.Text = $"Hentet tekst fra nettsider: {i + 1}/{dataOnWebsite.Count}";
                await System.Threading.Tasks.Task.Delay(20);
            }
            if (URLList.SelectedItem != null)
            {
                urlInfo seletedItem = URLList.SelectedItem as urlInfo;
                OriginalLang.Text = seletedItem.OriginalText;
                Translation.Text = seletedItem.TranslatedText;
                Language.Text = seletedItem.TranslateTo;
            }
            ProgressText.Text = "Ferdig med å hente tekst";
        }


        private void URLList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (URLList.SelectedItem != null)
            {
                urlInfo seletedItem = URLList.SelectedItem as urlInfo;
                OriginalLang.Text = seletedItem.OriginalText;
                Translation.Text = seletedItem.TranslatedText;
                Language.Text = seletedItem.TranslateTo;
                int WordcountOriginal = seletedItem.OriginalText.Split(new char[] { ' ', '\t','\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
                int WordcountTranslated = seletedItem.TranslatedText.Split(new char[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries ).Length;

                LengthOriginalText.Text = $"Lengde på opprinnelig tekst: {WordcountOriginal}";
                LengthTranslatedText.Text = $"Lengde på oversatt tekst: {WordcountTranslated}";
            }
        }

        private void BtnHelpCsv_Click(object sender, RoutedEventArgs e)
        {
            HelpText.Text = "Når du skal legge nettadressene til CSV filen så skal den inneholde hele URL. Eksempelvis: https://www.ntnu.no/inb/. De legges i første kollonne i filen hvis man jobber i Excel. Det er ikke nødvendig med overskrift på filen så man kan legge første adresse i celle A1. En ting man må være oppmerksom på når man laster inn CSV filen. Denne kan ikke være åpen samtidig som man kjører programmet. Lukk derfor CSV filen før man laster den inn i programmet";

        }

        private void BtnHelpUse_Click(object sender, RoutedEventArgs e)
        {
            HelpText.Text = "Når du har lastet inn CSV filen så vil det i venstre felt komme opp alle nettadressene som lå i denne filen. For å hente teksten som skal oversettes trykker du på Hent tekst til oversettelse. Den vil da hente alt av tekst fra nettsidene. Ved å bruke listen av nettadresser i programmet kan man se hva systemet har liggende på hver enkelt nettside. For å oversette teksten så trykker du på Oversett. Den fullstendige oversettelsen vil komme i høyre felt og hvilket språk vil komme over dette feltet. HUSK!!: Dette programmet bruker språkvelgeren til nettsidene aktivt for å bestemme hvilket språk nettsidene skal oversettes til. Hvis siden har slått av språkvelgeren så må denne aktiveres før den kjøres gjennom dette programmet.";
        }

        private void BtnHelpJson_Click(object sender, RoutedEventArgs e)
        {
            HelpText.Text = "Du har muligheten til å kopiere oversatt tekst for å lime direkte inn til ny nettside, eller lagre alt av oversatt data til en JSON fil. JSON fil er en enkel tekst fil som lagrer opplysningene strukturert. Filen vil ha nettadressen på første linje så opprinnelig tekst, språk oversatt til og oversatt tekst. Språk oversatt til er standard forkortelse av språket. For eksempel engelsk forkortes til en, og norsk til no. Oversettelsen vil komme på siste linje. Alle linjene er merket med titler i starten av linjen ";
        }
    }
}