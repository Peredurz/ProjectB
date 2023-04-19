using SkiaSharp;
using System.Drawing;

class AuditoriumLogic
{
    private ChairLogic _chairLogic = new ChairLogic();
    private static List<AuditoriumModel> _auditoriums = new List<AuditoriumModel>();

    public AuditoriumLogic()
    {
        _auditoriums = AuditoriumAccess.LoadAll();
        _chairLogic = new ChairLogic();
    }

    public void InitializeSeats()
    {
        List<ChairModel> chairModels = new List<ChairModel>();
        List<AuditoriumModel> auditoriumModels = new List<AuditoriumModel>();
        for (int i = 1; i <= 3; i++)
        {
            List<ChairModel> chairModelList = AuditoriumLogic.ParsePNG(i);
            chairModelList.ForEach(x => chairModels.Add(x));

            // de lijst van de ids van de stoelen worden uiteindelijk opgeslagen die later voor andere
            // dingen gebruikt kunnen worden.
            List<int> chairIDs = new List<int>();
            chairModelList.ForEach(x => chairIDs.Add(x.ID));

            AuditoriumModel auditorium = _auditoriums[i - 1];
            auditorium.Chairs = chairIDs;
            auditoriumModels.Add(auditorium);
        }
        AuditoriumAccess.WriteAll(_auditoriums);
        ChairAccess.WriteAll(chairModels);
    }

    public static List<ChairModel> ParsePNG(int auditoriumID)
    {
        // maak een string van de filepath waar de plattegrond van de zaal zit.
        // met dit plaatje wordt dan een lijst aan stoelen gegenereerd die in de zaal zitten.
        string filePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory,
                                                                            @"DataDocs/PixelZaal/PixelZaal" + auditoriumID + ".png"));
        List<ChairModel> chairModels = new List<ChairModel>();

        // maak een bitmap van de png file waar we door heen kunnen loopen en waar we data van kunnen maken.
        SKBitmap bmp = SKBitmap.Decode(filePath);
        for (int i = 0; i < bmp.Height; i++)
        {
            for (int j = 0; j < bmp.Width; j++)
            {
                // haal de pixel op van de huidige col (j) en row (i).
                // hier halen we een kleur uit om te bepalen wat voor stoel het is.
                SKColor pixelColour = bmp.GetPixel(j, i);
                // koppel een HTML kleur naam aan de hex code van de string om duidelijker te kunnen zoeken.
                string colourName = ColorTranslator.FromHtml(pixelColour.ToString()).Name;
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

        return chairModels;
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
        List<int> chairs = _auditoriums[--Movie.AuditoriumID].Chairs;
        // Lengte van het auditorium
        int length = _auditoriums[Movie.AuditoriumID].TotalCols;
        // Positie van de rij zet ik op 0 omdat je begint met kolom 1 maar de eerste keer dat je een stoel print is de positie 0 (nog geen stoel geprint)
        int pos = 0;
        // String dat uiteindelijk wordt gereturned
        //string chairPrint = "";

        // Rij bijhouden van de stoelen
        int rij = 1;

        // Loopen totdat je bij de chairsamount komt.
        for (int i = 0; i < chairsAmount + 1; i++)
        {
            if (i == 0)
            {
                for (int j = 1; j <= length; j++)
                {
                    if (j > 26)
                    {
                        Console.Write($"{Convert.ToChar(j + 70)} ");

                    }
                    else
                    {
                        Console.Write($"{Convert.ToChar(j + 64)} ");
                    }
                }
                Console.Write("\n");
                continue;
            }

            // Stoel pakken uit de lijst van stoelen van de json file chairs.json op positie i.
            ChairModel chair = _chairLogic.Chairs[i - 1];

            // Als de chairID overeenkomt met een van de chairID's in de chairs lijst
            // Want je wilt geen stoelen uit een andere zaal printen
            if (chairs.Contains(chair.ID))
            {
                // Op basis van de status van de stoel wordt er een andere string toegevoegd aan chairPrint
                string result = chair.Status switch
                {
                    Status.Available => "∩ ",
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
                 // Als de lengte van de rij is behaald wordt er een nieuwe regel gestart en wordt de positie weer op 0 gezet en word de rij nummer geprint
                if (pos == length)
                {
                    // Verandert de kleur van de rij zodat het wit blijft 
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{rij++}");
                    Console.Write("\n");
                    pos = 0;
                }

            }
        }
        Console.WriteLine();
    }

    public static List<int> GetChairIDs(int auditoriumID) => _auditoriums[auditoriumID].Chairs;
}