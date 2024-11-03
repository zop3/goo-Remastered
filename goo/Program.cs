// Program.cs
using System.Numerics;
using Silk.NET.GLFW;
using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Input;

class Program : Simulation
{
    Car car = new Car();
    public static Bread bread = new Bread();
    public static Points points = new Points();

    public bool start, fs;

    public ISound musica;
    public SoundPlayback pb;
    public ITexture truck;
    public ITexture breadLoaf;
    public static void Main()
    {
        Start<Program>();
        
    }

    public override void OnInitialize()
    {
        Simulation.SetFixedResolution(1440, 1080, Color.Black);
        musica = Audio.LoadSound("8bit bossfight (lower qual wav).wav");
        truck = Graphics.LoadTexture("Food Truck-c.png");
        breadLoaf = Graphics.LoadTexture("Bread.png");
        car.xPos = 720;
        car.yPos = 540;

        pb = musica.Loop();

        bread.setPos();
    }

    public override void OnRender(ICanvas canvas)
    {
        if (Keyboard.IsKeyDown(Key.Space))
        {
            start = true;
        }

        if (Keyboard.IsKeyPressed(Key.F11))
        {
            fs = !fs;
            if (fs)
            {
                Window.EnterFullscreen();
            }
            else
            {
                Window.ExitFullscreen();
            }
        }

        if (Keyboard.IsKeyDown(Key.Esc))
        {
            Environment.Exit(0);
        }

        if (start)
        { 
            car.Calculate();


            // don't forget to clear the screen each frame!
            canvas.Clear(Color.White);


            canvas.Translate(car.xPos, car.yPos);
            canvas.Rotate(car.dir);
            canvas.DrawTexture(truck, new Vector2(0, 0), new Vector2(75, 75), Alignment.Center);
            canvas.ResetState();


            canvas.DrawTexture(breadLoaf, new Vector2(bread.bx, bread.by), new Vector2(60, 20), Alignment.Center);

            canvas.Fill(Color.Black);
            canvas.FontSize(100);
            canvas.DrawText(Convert.ToString(points.points), new Vector2(50, 1030), Alignment.Center, TextBounds.BestFit);
        }
        else
        {
            canvas.Clear(Color.Black);

            canvas.Fill(Color.White);
            canvas.FontSize(70);
            canvas.DrawText("Press space to play", new Vector2(720, 465), Alignment.Center, TextBounds.BestFit);
            canvas.Fill(Color.White);
            canvas.FontSize(70);
            canvas.DrawText("Press F11 to enter/exit fullscreen", new Vector2(720, 540), Alignment.Center, TextBounds.BestFit);
            canvas.FontSize(70);
            canvas.DrawText("Press esc to quit", new Vector2(720, 615), Alignment.Center, TextBounds.BestFit);
        }
       
    }
}

class Car
{
    
    public float dir, xPos, yPos;
    public float vx = 1f, vy = 1f;

    public void Calculate()
    {


        dir = MathF.Atan2(Mouse.Position.Y - yPos, Mouse.Position.X - xPos);
        vx += ((MathF.Abs(vx)) / vx) * -0.1f * Time.DeltaTime * 30;
        vy += ((MathF.Abs(vy)) / vy) * -0.1f * Time.DeltaTime * 30;
        vx += MathF.Cos(dir) * 0.35f * Time.DeltaTime * 30;
        vy += MathF.Sin(dir) * 0.35f * Time.DeltaTime * 30;

        xPos += vx * Time.DeltaTime * 30;
        yPos += vy * Time.DeltaTime * 30;

        

        if (rectrect(xPos, yPos, 75, 75, Program.bread.bx, Program.bread.by, 60, 20, Alignment.Center, Alignment.Center))
        {
            Program.bread.setPos();
            Program.points.score();

        }

        if (xPos > 1440 | xPos < 0 | yPos > 1080 | yPos < 0)
        {
            Environment.Exit(0);
        }


    }
    /// <summary>checks if 2 rectangles are colliding</summary>
    public static bool rectrect(float r1x, float r1y, float r1w, float r1h, float r2x, float r2y, float r2w, float r2h, Alignment r1a = Alignment.TopLeft, Alignment r2a = Alignment.TopLeft)
    {
        float r1xl = 0, //x left
              r2xl = 0,
              r1yt = 0, //y top
              r2yt = 0,
              r1xr = 0, //x right
              r2xr = 0,
              r1yb = 0, //y bot
              r2yb = 0;

        switch (r1a)
        {
            default:
                r1xl = r1x; r1xr = r1x + r1w; r1yt = r1y; r1yb = r1y + r1h; break;
            case Alignment.BottomLeft:
                r1xl = r1x; r1xr = r1x + r1w; r1yt = r1y + r1h; r1yb = r1y; break;
            case Alignment.TopRight:
                r1xl = r1x - r1w; r1xr = r1x; r1yt = r1y; r1yb = r1y + r1h; break;
            case Alignment.BottomRight:
                r1xl = r1x - r1w; r1xr = r1x; r1yt = r1y + r1h; r1yb = r1y; break;
            case Alignment.TopCenter:
                r1xl = r1x - r1w / 2; r1xr = r1x + r1w / 2; r1yt = r1y; r1yb = r1y + r1h; break;
            case Alignment.BottomCenter:
                r1xl = r1x - r1w / 2; r1xr = r1x + r1w / 2; r1yt = r1y - r1h; r1yb = r1y; break;
            case Alignment.CenterLeft:
                r1xl = r1x; r1xr = r1x + r1w; r1yt = r1y - r1h / 2; r1yb = r1y + r1h / 2; break;
            case Alignment.CenterRight:
                r1xl = r1x - r1w; r1xr = r1x; r1yt = r1y - r1h / 2; r1yb = r1y + r1h / 2; break;
            case Alignment.Center:
                r1xl = r1x - r1w / 2; r1xr = r1x + r1w / 2; r1yt = r1y - r1h / 2; r1yb = r1y + r1h / 2; break;
        }

        switch (r2a)
        {
            default:
                r2xl = r2x; r2xr = r2x + r2w; r2yt = r2y; r2yb = r2y + r2h; break;
            case Alignment.BottomLeft:
                r2xl = r2x; r2xr = r2x + r2w; r2yt = r2y + r2h; r2yb = r2y; break;
            case Alignment.TopRight:
                r2xl = r2x - r2w; r2xr = r2x; r2yt = r2y; r2yb = r2y + r2h; break;
            case Alignment.BottomRight:
                r2xl = r2x - r2w; r2xr = r2x; r2yt = r2y + r2h; r2yb = r2y; break;
            case Alignment.TopCenter:
                r2xl = r2x - r2w / 2; r2xr = r2x + r2w / 2; r2yt = r2y; r2yb = r2y + r2h; break;
            case Alignment.BottomCenter:
                r2xl = r2x - r2w / 2; r2xr = r2x + r2w / 2; r2yt = r2y - r2h; r2yb = r2y; break;
            case Alignment.CenterLeft:
                r2xl = r2x; r2xr = r2x + r2w; r2yt = r2y - r2h / 2; r2yb = r2y + r2h / 2; break;
            case Alignment.CenterRight:
                r2xl = r2x - r2w; r2xr = r2x; r2yt = r2y - r2h / 2; r2yb = r2y + r2h / 2; break;
            case Alignment.Center:
                r2xl = r2x - r2w / 2; r2xr = r2x + r2w / 2; r2yt = r2y - r2h / 2; r2yb = r2y + r2h / 2; break;
        }

        return r1xr >= r2xl && r1xl <= r2xr && r1yb >= r2yt && r1yt <= r2yb;
    }
}

class Bread
{
    public float bx, by;

    public void setPos()
    {
        Random ranx = new Random();
        Random rany = new Random();

        bx = ranx.Next(0,1440);
        by = rany.Next(0,1080);
    }
}

class Points
{
    public int points;

    public void score()
    {
        points++;
    }
}