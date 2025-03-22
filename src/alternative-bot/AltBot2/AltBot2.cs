using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using System.Collections.Generic;
using System.Linq;

using Robocode.TankRoyale.BotApi.Events;

public class AltBot2 : Bot
{   
    /* A bot that drives forward and backward, and fires a bullet */
    static void Main(string[] args)
    {
        new AltBot2().Start();
    }
    Random random = new Random();


    AltBot2() : base(BotInfo.FromFile("AltBot2.json")) { }
    private Dictionary<int?, double> enemyDistances = new Dictionary<int?, double>();
    private const int MaxLostTurns = 15;
    public override void Run()
    {
        /* Customize bot colors, read the documentation for more information */
        BodyColor = Color.Gray;
        while (IsRunning)
        {
            if(EnemyCount < 4){
                SetForward(100);
                SetTurnLeft(360);
                SetRescan();
                Go();
            }
            else{
                WaitFor(new TurnCompleteCondition(this));

                double wallMargin = 100; // Minimum safe distance from walls
                double battlefieldWidth = ArenaWidth;
                double battlefieldHeight = ArenaHeight;

                double distanceToLeftWall = X; 
                double distanceToRightWall = battlefieldWidth - X; 
                double distanceToTopWall = battlefieldHeight - Y; 
                double distanceToBottomWall = Y; 

                double turnAngle = 0;

                // Check if bot is near a wall and facing towards it
                if (distanceToLeftWall < wallMargin && CalcBearing(180) < 45) // Facing left wall
                { 
                    turnAngle = 90;
                }
                else if (distanceToRightWall < wallMargin && CalcBearing(0) < 45) // Facing right wall
                {
                    turnAngle = -90;
                }
                else if (distanceToTopWall < wallMargin && CalcBearing(90) < 45) // Facing top wall
                {
                    turnAngle = 90;
                }
                else if (distanceToBottomWall < wallMargin && CalcBearing(270) < 45) // Facing bottom wall
                {
                    turnAngle = -90;
                }
                else
                {
                    // If not near a wall, choose a random turn angle
                    turnAngle = random.Next(-180, 180);
                }

                SetForward(100);
                SetTurnRight(turnAngle);
            }
        }
    }

    // public override void OnTick(TickEvent e){
    //     if (!targetVisible){
    //         if (moveCooldown <= 0){
    //             SetForward(rng.Next(100, 300));
    //             SetTurnRight(rng.Next(-90, 90));
    //             moveCooldown = rng.Next(20, 40);
    //         }
    //         moveCooldown--;
    //         SetTurnGunRight(45);
    //     }
    //     else{
    //         SetForward(20);
    //         SetTurnRight(5);
    //         SetTurnGunRight(20);

    //         turnsSinceLastScan++;
    //         if (turnsSinceLastScan > MaxLostTurns){
    //             targetVisible = false;
    //             turnsSinceLastScan = 0;
    //         }
    //     }
    // }
    public override void OnScannedBot(ScannedBotEvent e)
    {
        if(EnemyCount < 4){
            Interruptible = true;
            double turnDirection  = CalcBearing(e.Direction); 
            Console.WriteLine("Turning: " + e.Direction);
            SetTurnLeft(turnDirection);
            Console.WriteLine("Turn direction: " + turnDirection + " Current Direction: " + Direction);
            SetForward(100);
            Console.WriteLine("Forward");
            SetFire(2);   
            Console.WriteLine("Firing");
        }
        else{
            SetFire(3);
            WaitFor(new TurnCompleteCondition(this));
        }

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


public class TurnCompleteCondition : Condition
{
    private readonly Bot bot;

    public TurnCompleteCondition(Bot bot)
    {
        this.bot = bot;
    }

    public override bool Test()
    {
        // turn is complete when the remainder of the turn is zero
        return bot.TurnRemaining == 0;
    }
}