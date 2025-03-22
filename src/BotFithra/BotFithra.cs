using System;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class BotFithra : Bot
{
    static void Main(string[] args)
    {
        new BotFithra().Start();
    }

    BotFithra() : base(BotInfo.FromFile("BotFithra.json")) { }

    public override void Run()
    {
        Console.WriteLine("Bot Fithra is starting...");

        while (IsRunning)
        {
            Forward(100);
            TurnRight(20); 

            if (Random.Shared.Next(0, 3) == 0)
            {
                TurnLeft(40);
                Back(50);
            }

            TurnGunRight(360);
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        double distance = Math.Sqrt(Math.Pow(e.X - X, 2) + Math.Pow(e.Y - Y, 2));
        Console.WriteLine($"Detected bot at distance: {distance}");

        if (distance < 150 || e.Energy < 20)
        {
            Fire(3); 
        }
        else if (distance < 400)
        {
            Fire(1.5);
        }
        else
        {
            Fire(0.8); 
        }
    }

    public override void OnHitWall(HitWallEvent e)
    {
        Console.WriteLine("Hit a wall, turning back...");
        TurnRight(90);
    }

    public override void OnHitBot(HitBotEvent e)
    {
        Console.WriteLine("Hit another bot!");
        Back(50);
        TurnRight(45);
    }

    public override void OnHitByBullet(HitByBulletEvent evt)
    {
        Console.WriteLine("Got hit by a bullet! Evading...");
        TurnLeft(45);
        Back(50);
    }
}
