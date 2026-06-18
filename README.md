# Web crawler and translator

Er et spesialisert verktøy utviklet for å automatisere prosessen med å samle inn tekst fra nettsider, oversette den og klargjøre resultatet for videre bruk. 
Systemet er bygget som en C# WPF-applikasjon som kombinerer web-skraping med automatisk oversettelse via skybaserte tjenester.

**Systemets funksjonalitet**

Programmet fungerer ved at en bruker laster inn en CSV-fil som inneholder en liste over nettadresser (URL-er).
Gjennom et grafisk brukergrensesnitt kan man deretter utføre følgende handlinger:

* **Innsamling:** Systemet går gjennom listen, "crawler" (skraper) nettsidene og henter ut all relevant tekst.
* **Oversettelse:** Den originale teksten sendes til Google Translate via HTTP-forespørsler.
* **Håndtering av store datamengder:** For å omgå tekniske begrensninger i oversettelsestjenesten, inneholder programmet logikk som automatisk deler opp store tekster i mindre biter før de sendes, for så å sette dem sammen igjen etter at oversettelsen er fullført.
* **Eksport:** Brukeren kan se ordtelling, velge målspråk og lagre de ferdige resultatene som en JSON-fil.

**Teknisk oppbygging**

Prosjektet er organisert i fire sentrale klasser som håndterer hver sin del av arbeidsflyten:
* **urlInfo:** Et dataobjekt som holder på informasjonen om URL, originaltekst og oversatt tekst.
* **fileHandling:** Håndterer lesing av CSV-filer og lagring av JSON-data.
* **webCrawler:** Inneholder logikken for å identifisere og hente ut tekst fra HTML-containere på nettsider.
* **googleTranslator:** Kommuniserer med Google Translate-API-et og sørger for parsing av de returnerte dataene.

For å løse disse oppgavene benyttes eksterne biblioteker som HtmlAgilityPack for skraping, CsvHelper for filbehandling og Newtonsoft.Json for datastrukturering.

**Praktisk anvendelse**

Teknologien har blitt brukt profesjonelt ved NTNU for å automatisere oversettelsen av instituttets nettsider fra norsk til engelsk. 
Ved å tilpasse skrapeløsningen til universitetets spesifikke HTML-struktur, kunne man effektivt generere engelsk innhold.
Systemet er ellers velegnet for oppgaver som datainnsamling til forskning, sentimentanalyse, eller generering av flerspråklige datasett for maskinlæring.
