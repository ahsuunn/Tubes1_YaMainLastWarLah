using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class AltBot3 : Bot
{   
    /* A bot that drives forward and backward, and fires a bullet */
    static void Main(string[] args)
    {
        new AltBot3().Start();
    }

    AltBot3() : base(BotInfo.FromFile("AltBot3.json")) { }
    
    public override void Run()
    {
        /* Customize bot colors, read the documentation for more information */
        BodyColor = Color.Gray;
        // AdjustGunForBodyTurn = true;
        while (IsRunning)
        {
            SetForward(100);
            SetTurnLeft(10);
            SetTurnGunLeft(10);
            Go();

        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        Interruptible = true;
        double turnDirection  = CalcBearing(e.Direction); 
        var bearingFromGun = GunBearingTo(e.X, e.Y);
        Console.WriteLine("Turning: " + e.Direction);
        SetTurnGunLeft(bearingFromGun);
        SetTurnLeft(turnDirection);
        
        Console.WriteLine("Turn direction: " + turnDirection + " Current Direction: " + Direction);
        SetForward(100);
        Console.WriteLine("Forward");
        
        SetFire(2);
        // Generates another scan event if we see a bot.
        // We only need to call this if the gun (and therefore radar)
        // are not turning. Otherwise, scan is called automatically.
        if (bearingFromGun == 0)
            Rescan();

        Console.WriteLine("Firing");

    }

    public override void OnHitBot(HitBotEvent e)
    {
        Console.WriteLine("Ouch! I hit a bot at " + e.X + ", " + e.Y);
    }

    public override void OnHitWall(HitWallEvent e)
    {
        Console.WriteLine("Ouch! I hit a wall, must turn back!");
    }

    /* Read the documentation for more events and methods */
}
