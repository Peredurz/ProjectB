using SkiaSharp;
using System.Drawing;

class AuditoriumLogic
{
    public static int AuditoriumID;
    private ChairLogic _chairLogic = new ChairLogic();
    private List<AuditoriumModel> _auditoriums = new List<AuditoriumModel>();

    public AuditoriumLogic(int auditoriumID = 0)
    {
        _auditoriums = AuditoriumAccess.LoadAll();
        AuditoriumID = auditoriumID;
        _chairLogic = new ChairLogic();
    }

    public void InitializeSeats()
    {
        for (int i = 1; i <= 3; i++)
        {
            List<int> chairIds = AuditoriumLogic.ParsePNG(i);
            AuditoriumModel auditorium = _auditoriums[i - 1];
            auditorium.Chairs = chairIds;
            AuditoriumAccess.WriteAll(_auditoriums);
        }
    }

    public static List<int> ParsePNG(int auditoriumID)
    {
        // maak een string van de filepath waar de plattegrond van de zaal zit.
        // met dit plaatje wordt dan een lijst aan stoelen gegenereerd die in de zaal zitten.
        string filePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory,
                                                                            @"DataDocs/PixelZaal/PixelZaal" + auditoriumID + ".png"));
        List<ChairModel> chairModels = new List<ChairModel>();

        // maak een bitmap van de png file waar we door heen kunnen loopen en waar we data van kunnen maken.
        SKBitmap bmp = SKBitmap.Decode(filePath);
        for (int i = 0; i < bmp.Width; i++)
        {
            for (int j = 0; j < bmp.Height; j++)
            {
                // haal de pixel op van de huidige col (j) en row (i).
                // hier halen we een kleur uit om te bepalen wat voor stoel het is.
                SKColor pixelColour = bmp.GetPixel(i, j);
                // koppel een HTML kleur naam aan de hex code van de string om duidelijker te kunnen zoeken.
                string colourName = ColorTranslator.FromHtml(pixelColour.ToString()).Name;
                //// check of de kleur wit is want dan kan het geskipped worden
                if (colourName == "White")
                    continue;

                // bepaal de prijs van de stoel op basis van de kleur (dus locatie)
                double chairPrice = 0.0;
                if (colourName == "Blue")
                    chairPrice = 5.0;
                else if (colourName == "Orange")
                    chairPrice = 10.0;
                else if (colourName == "Red")
                    chairPrice = 15.0;

                // maak een nieuwe stoel aan en voeg die aan onze stoelen toe.
                chairModels.Add(new ChairModel(i, j, (int)chairPrice, colourName, Status.Available));
            }
        }
        // de lijst van de ids van de stoelen worden uiteindelijk opgeslagen die later voor andere
        // dingen gebruikt kunnen worden.
        List<int> chairIDs = new List<int>();
        chairModels.ForEach(x => chairIDs.Add(x.ID));

        ChairAccess.WriteAll(chairModels);

        return chairIDs;
    }

    public void Write(List<AuditoriumModel> auditoriums)
    {
        // De meegegeven lijst van auditoriums schrijven naar de json file via AuditoriumAccess
        AuditoriumAccess.WriteAll(auditoriums);
    }

    public void ChairPrint()
    {
        // Hoeveelheid stoelen in de lijst met stoelen checken
        int chairsAmount = this._chairLogic.Chairs.Count;
        // De lijst van integers in de Auditorium.json file met de key chairs meegeven waarbij is gesorteerd op het AuditoriumID
        // Dus alleen de lijst met stoelID's van een bepaald Auditorium wordt meegegeven
        List<int> chairs = _auditoriums[--AuditoriumID].Chairs;
        // Lengte van het auditorium
        int length = _auditoriums[AuditoriumID].TotalCols;
        // Positie van de rij zet ik op 0 omdat je begint met kolom 1 maar de eerste keer dat je een stoel print is de positie 0 (nog geen stoel geprint)
        int pos = 0;
        // String dat uiteindelijk wordt gereturned
        //string chairPrint = "";

        // Loopen totdat je bij de chairsamount komt.
        for (int i = 0; i < chairsAmount; i++)
        {
            // Als de lengte van de rij is behaald wordt er een nieuwe regel gestart en wordt de positie weer op 0 gezet
            if (pos == length)
            {
                Console.Write("\n");
                pos = 0;
            }

            // Stoel pakken uit de lijst van stoelen van de json file chairs.json op positie i.
            ChairModel chair = _chairLogic.Chairs[i];

            // Als de chairID overeenkomt met een van de chairID's in de chairs lijst
            // Want je wilt geen stoelen uit een andere zaal printen
            if (chairs.Contains(chair.ID))
            {
                // Op basis van de status van de stoel wordt er een andere string toegevoegd aan chairPrint
                string result = chair.Status switch
                {
                    Status.Available => "# ",
                    Status.Pending => "? ",
                    Status.Reserved => "X ",
                    _ => " ",
                };
                // De kleur van de stoel wordt bepaald op basis van de kleur van de stoel
                ConsoleColor color = chair.Color.ToLower() switch
                {
                    "red" => ConsoleColor.Red,
                    "blue" => ConsoleColor.Blue,
                    "orange" => ConsoleColor.DarkYellow,
                    "white" => ConsoleColor.White,
                    _ => ConsoleColor.White,
                };
                Console.ForegroundColor = color;
                Console.Write(result);
                pos++;
            }
        }
    }
}