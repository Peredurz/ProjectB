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

    /// <summary>
    /// Dit is de functie om alle stoelen van een bepaalde zaal en film te printen in de console.
    /// Hierbij heb je de verschillende kleuren stoelen die horen bij een bepaalde prijs.
    /// <c>∩</c> = Beschikbare stoel
    /// <c>X</c> = Gereserveerde stoel
    /// </summary>
    /// <remarks>
    /// Dit gebeurt door de statische <see cref="Movie.MovieID"/> variabele die iederekeer opnieuw wordt aangepast.
    /// Als iemand een andere film kiest in het keuzemenu voor de <see cref="Movie"/> objecten.
    /// </remarks>
    public void ChairPrint(int x, int y)
    {
        // Hoeveelheid stoelen in de lijst met stoelen checken
        int chairsAmount = this._chairLogic.Chairs.Count;
        // De lijst van integers in de Auditorium.json file met de key chairs meegeven waarbij is gesorteerd op het AuditoriumID
        // Dus alleen de lijst met stoelID's van een bepaald Auditorium wordt meegegeven
        List<int> chairs = _auditoriums[Movie.AuditoriumID - 1].Chairs; 
        // Lengte van het auditorium
        int length = _auditoriums[Movie.AuditoriumID - 1].TotalCols;
        int width = _auditoriums[Movie.AuditoriumID - 1].TotalRows;
        int[,] chairs2d = {};
        chairs2d = new int[width, length];
        int idx = 0;
        for (int r = 0; r < width; r++)
        {
            for (int c = 0; c < length; c++)
            {
                chairs2d[r, c] = chairs[idx];
                idx++;
            }
        }

        List<MovieModel> movies = MovieAccess.LoadAll();

        MovieModel movie = movies.Find(x => x.ID == Movie.MovieID);

        List<ChairReservationModel> chairReservations = ChairReservationAccess.LoadAll();

        DateTime time = movie.Time;
        // Alle stoelreserveringen voor een bepaalde film en tijd in een lijst zetten
        List<ChairReservationModel> chairReservationsForMovie = chairReservations.FindAll(x => x.Time == time);
        // Positie van de rij zet ik op 0 omdat je begint met kolom 1 maar de eerste keer dat je een stoel print is de positie 0 (nog geen stoel geprint)
        int pos = 0;
        // String dat uiteindelijk wordt gereturned
        //string chairPrint = "";

        // Rij bijhouden van de stoelen
        int rij = 1;
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

        idx = _chairLogic.Chairs.IndexOf(_chairLogic.Chairs.Where(x => x.ID == chairs.First()).ToList().First());
        for (int r = 0; r < chairs2d.GetLength(0); r++)
        {
            for (int c = 0; c < chairs2d.GetLength(1); c++)
            {
                // Stoel pakken uit de lijst van stoelen van de json file chairs.json op positie i.
                ChairModel chair = _chairLogic.Chairs[idx];
                // Als de chairID overeenkomt met een van de chairID's in de chairs lijst
                // Want je wilt geen stoelen uit een andere zaal printen
                if (chairs.Contains(chair.ID))
                {
                    // Als de stoel in de lijst van gereserveerde stoelen zit wordt er een X geprint
                    if (chairReservationsForMovie.Contains(chairReservations.Find(x => x.ChairID == chair.ID)))
                    {
                        // zoek het daadwerkelijke reserveringsmodel om aan te kunnen geven of het een ? moet zijn of een X.
                        ChairReservationModel foundChairReservation = chairReservations.Find(x => x.ChairID == chair.ID);
                        // Als de stoel in de lijst van gereserveerde stoelen zit wordt er een X geprint
                        if (r == (x - 1) && c == (y - 1))
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write(foundChairReservation.IsCompleted == true ? "X " : "? ");
                            Console.BackgroundColor = ConsoleColor.Black;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write(foundChairReservation.IsCompleted == true ? "X " : "? ");
                        }
                        pos++;
                        // Als de lengte van de rij is behaald wordt er een nieuwe regel gestart en wordt de positie weer op 0 gezet en word de rij nummer geprint
                        if (pos == length)
                        {
                            Console.Write($"{rij}");
                            Console.Write("\n");
                            pos = 0;
                            rij++;
                        }
                        idx++;
                        continue;
                    }
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
                        "white" => ConsoleColor.Black,
                        _ => ConsoleColor.Black,
                    };

                    // x en y zijn de cursor, als deze gelijk zijn aan de row en column kan je het een andere achtergrond
                    // geven om aan te geven dat het gekozen is.
                    if (r == (x - 1) && c == (y - 1))
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = color;
                        Console.Write(result);
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.ForegroundColor = color;
                        Console.Write(result);
                    }

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
                idx++;
            }
        }

        Console.WriteLine();
    }
    
    public AuditoriumModel GetAuditoriumModel(int auditoriumID)
    {
        foreach (AuditoriumModel auditorium in _auditoriums)
        {
            if (auditoriumID == auditorium.ID)
            {
                return auditorium;
            }
        }
        return null;
    }

    public static List<int> GetChairIDs(int auditoriumID) => _auditoriums[auditoriumID].Chairs;
}