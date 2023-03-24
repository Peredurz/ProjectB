using SkiaSharp;
using System.Drawing;

public class AuditoriumLogic
{
    public static int AuditoriumID;
    private ChairLogic _chairLogic = new ChairLogic();
    private List<AuditoriumModel> _auditoriums = new List<AuditoriumModel>();

    public AuditoriumLogic(int auditoriumID = 0)
    {
        _auditoriums = AuditoriumAccess.LoadAll();
        AuditoriumID = auditoriumID;
    }

    public void InitializeSeats()
    {
        for (int i = 1; i <= 3; i++)
        {
            List<int> chairIds = AuditoriumLogic.ParsePNG(i);
            Console.WriteLine(chairIds.Count());
            AuditoriumModel auditorium = _auditoriums[i - 1];
            auditorium.Chairs = chairIds;
            //AuditoriumAccess.WriteAll(_auditoriums);
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
                //if (colourName == "White")
                //    continue;

                // bepaal de prijs van de stoel op basis van de kleur (dus locatie)
                double chairPrice = 0.0;
                if (colourName == "Blue")
                    chairPrice = 5.0;
                else if (colourName == "Orange")
                    chairPrice = 10.0;
                else if (colourName == "Red")
                    chairPrice = 15.0;

                // maak een nieuwe stoel aan en voeg die aan onze stoelen toe.
                chairModels.Add(new ChairModel(i, j, (int)chairPrice, colourName));
            }
        }
        // de lijst van de ids van de stoelen worden uiteindelijk opgeslagen die later voor andere
        // dingen gebruikt kunnen worden.
        List<int> chairIDs = new List<int>();
        chairModels.ForEach(x => chairIDs.Add(x.ID));

        ChairAccess.WriteAll(chairModels);

        return chairIDs;
    }
}