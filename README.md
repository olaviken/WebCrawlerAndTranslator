# Web crawler and translator
The main goal of this project is to collect all text from given websites, send them to google translate for translation and display translated text for use in other applications. 

## How it works
The program works by the user opens a FileDialog window and chooses the correct CSV file. The system can only read CSV files for this purpose. During reading of the CSV file the program will show which URL it is collecting at the bottom of the software and all URL will show in the listbox at the left side of the user interface. To collect the text from the website the user presses the button named "Retrieve text for translation". This function will go through the list of URLS collect the text and store it a list. After the text is collected the user can ask the software to translate all the texts collected. The software will then go through the list and send the original text to google translate for translation. With the translated text the software has to parse trough returned data to find the complete text before storing it in the list and showing it to user.

## Graphic user interface (GUI)
The GUI is divided into to tabs. One part is for the main controls and the other is a simple user manual/help tab. The main control tab starts where the filepath will go and a button to open the Filedialog window. Below this area there are one large listbox window to the left of two large textboxes. These three are the main area of the control tab. The listbox will show all the URL that is being translated. It is interactive and if a URL is marked it will show the original text, and translation when these operation is done. Also above each textbox it will show word count for both original and translated text along with which language it is translated to. Below the listbox and textboxes the controls are located in form of four buttons. The left most button scrapes all the websites for original text. 2nd from the left sends all original text to translation, 3rd button copies the text in the translated textbox to windows clipboard. Lastly the fourth button stores all data into a JSON file located at a chosen place by the user.

## Technical aspects.

### Dependencies
To handle different parts of the project, different external libraries and framework was used:
1. CsvHelper was used to load and read through the CSV file. (https://joshclose.github.io/CsvHelper/)
2. HtmlAgilityPack was used for scraping the websites and retrive original text. (https://html-agility-pack.net/)
3. Newtonsoft.Json was used to parse translated array and store the data as JSON file. (https://www.newtonsoft.com/json)

### Project classes
Besides the main class in a C# WPF project this project has four other classes. Each focusing on one area of the project. These classes are as following:

urlInfo:
This is a simple data object it makes the backbone of the project list where all the data is stored. It is made of four different string variables, URL, original text, translate to, and translated text. 

fileHandling:
This class handles all the functions with loading and storing data. It finds and loads data from the CSV file. Also, storing the completed data into the JSON file. 

webCrawler:
This class has the functions for scraping the URLs all the text on the websites, finding the language it is translating to, and finding relevant text to translate. Since this scraper is tailormade to scrape through websites spesifically at the Norwegian University of Science and Technology it has to be modified for other sites. Specifically which html containers that are relevant, where to find the language toggler, and where in return array the relevant text is located.

googleTranslator:
This class handles all the work with sending text to google translate through the unoffical web API. This API might be changed by Google in the future. The API uses autodetect on the original language and uses language code from webCrawler to translate. Since the text on some of the sites might exceed the max character of the API. This class has a function that breaks such large text into managable chunks of text. These chunks are sendt seperately and are welded together after translation. The result from Google translate is a JSON array so this class also has a function that parse through this array to find the translated text. 

## Other uses for this project or part of the project
Part of this project could be used in other projects such as:

1. Data aggregation for research
2. Sentiment analysis
3. Automated summary generator
4. Competitive market analysis
5. Multilingual dataset generation for natural language processing (NLP)
