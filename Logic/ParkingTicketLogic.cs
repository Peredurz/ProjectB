using System;
using SkiaSharp;

class ParkingTicketLogic
{
    public static void GenerateBarCode()
    {
        int surfaceWidth = 300;
        // Create new skiasharp surface for the barcode
        SKSurface surface = SKSurface.Create(new SKImageInfo(surfaceWidth, 100));
        // Create new canvas to draw on
        SKCanvas canvas = surface.Canvas;
        // Create new paint to draw with
        SKPaint paint = new SKPaint();
        // for loop to draw the barcode
        int whiteColorCount = 0;
        for (int width = 0; width < surfaceWidth; width++)
        {
            Random random = new Random();
            int randomInt = random.Next(0, 4);
            if (randomInt == 0)
            {
                paint.Color = SKColors.Black;
            }
            else
            {
                paint.Color = SKColors.White;
                whiteColorCount++;
            }
            if (whiteColorCount == 2)
            {
                paint.Color = SKColors.Black;
                whiteColorCount = 0;
            }
            canvas.DrawRect(width, 0, 8, 100, paint);
        }
        using (SKImage image = surface.Snapshot())
        using (SKData data = image.Encode(SKEncodedImageFormat.Png, 100))
        using (System.IO.Stream stream = File.OpenWrite(Path.Combine("DataDocs/ParkeerKaart", "barcode.png")))
        {
            data.SaveTo(stream);
        }
    }
    public static void GenerateParkingTicket()
    {
        GenerateBarCode();
        // Create new skiasharp surface for the ticket
        SKSurface surface = SKSurface.Create(new SKImageInfo(400, 500));
        // Create new canvas to draw on
        SKCanvas canvas = surface.Canvas;
        // Create new paint to draw with
        SKPaint paint = new SKPaint();
        // Draw the background
        paint.Color = SKColors.White;
        canvas.DrawRect(0, 0, 400, 500, paint);
        // Draw the title
        paint.Color = SKColors.Black;
        paint.TextSize = 30;
        canvas.DrawText("Parkeerkaart", 100, 50, paint);
        // Draw the barcode
        SKBitmap barcode = SKBitmap.Decode(Path.Combine("DataDocs/ParkeerKaart", "barcode.png"));
        canvas.DrawBitmap(barcode, 50, 100, paint);
        // Draw the license plate
        paint.Color = SKColors.Black;
        paint.TextSize = 20;
        canvas.DrawText("Kenteken: 1-ABC-23", 100, 250, paint);
        // Draw the start time
        paint.Color = SKColors.Black;
        paint.TextSize = 20;
        canvas.DrawText("Starttijd: 12:00", 100, 300, paint);
        // Draw the end time
        paint.Color = SKColors.Black;
        paint.TextSize = 20;
        canvas.DrawText("Eindtijd: 14:00", 100, 350, paint);
        // Draw the price
        paint.Color = SKColors.Black;
        paint.TextSize = 20;
        canvas.DrawText("Prijs: € 2,00", 100, 400, paint);
        // Draw the footer
        paint.Color = SKColors.Black;
        paint.TextSize = 20;
        canvas.DrawText("Betaald via: Ideal", 100, 450, paint);
        // Save the ticket
        using (SKImage image = surface.Snapshot())
        using (SKData data = image.Encode(SKEncodedImageFormat.Png, 100))
        using (System.IO.Stream stream = File.OpenWrite(Path.Combine("DataDocs/ParkeerKaart", "ticket.png")))
        {
            data.SaveTo(stream);
        }

    }
}