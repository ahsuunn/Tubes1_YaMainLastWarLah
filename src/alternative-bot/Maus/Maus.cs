using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using System.Collections.Generic;
using System.Linq;

using Robocode.TankRoyale.BotApi.Events;

public class Maus : Bot
{   
    static void Main(string[] args)
    {
        new Maus().Start();
    }
    Random random = new Random();

    
    Maus() : base(BotInfo.FromFile("Maus.json")) { }
    public override void Run()
    {
        GunColor = Color.Aquamarine;
        TurretColor = Color.Aquamarine;
        ScanColor = Color.Aquamarine;
        BulletColor = Color.Aquamarine;
        TracksColor = Color.Aquamarine;
        BodyColor = Color.White;
        RadarColor = Color.White;

        while (IsRunning)
        {
            if(EnemyCount < 4){
                SetForward(100);
                SetTurnLeft(10);
                SetTurnGunLeft(10);
                Go();
            }
            else{
                WaitFor(new TurnCompleteCondition(this));

                double wallMargin = 100; 
                double battlefieldWidth = ArenaWidth;
                double battlefieldHeight = ArenaHeight;

                double distanceToLeftWall = X; 
                double distanceToRightWall = battlefieldWidth - X; 
                double distanceToTopWall = battlefieldHeight - Y; 
                double distanceToBottomWall = Y; 

                double turnAngle = 0;

                if (distanceToLeftWall < wallMargin && CalcBearing(180) < 45)
                { 
                    turnAngle = 90;
                }
                else if (distanceToRightWall < wallMargin && CalcBearing(0) < 45)
                {
                    turnAngle = -90;
                }
                else if (distanceToTopWall < wallMargin && CalcBearing(90) < 45) 
                {
                    turnAngle = 90;
                }
                else if (distanceToBottomWall < wallMargin && CalcBearing(270) < 45)
                {
                    turnAngle = -90;
                }
                else
                { 
                    turnAngle = random.Next(-180, 180);
                }

                SetForward(60);
                SetTurnRight(turnAngle);
            }
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        if(EnemyCount < 4){
            Interruptible = true;
            double turnDirection  = BearingTo(e.X,e.Y); 
            var bearingFromGun = GunBearingTo(e.X, e.Y);
            Console.WriteLine("Turning: " + e.Direction);
            SetTurnGunLeft(bearingFromGun/2);
            SetTurnLeft(turnDirection);
            
            Console.WriteLine("Turn direction: " + turnDirection + " Current Direction: " + Direction);
            SetForward(100);
            Console.WriteLine("Forward");
            
            SetFire(FiringPower(DistanceTo(e.X, e.Y)));
            if (bearingFromGun == 0){
                Rescan();
            }
        }
        else{
            SetFire(FiringPower(DistanceTo(e.X, e.Y)));
            WaitFor(new TurnCompleteCondition(this));
        }

    }   

    public override void OnHitBot(HitBotEvent e)
    {
        if (e.IsRammed){
            Fire(3);
        }
        else{
            Back(50);
            TurnLeft(90 + random.Next(45));
        }
    }

    public override void OnHitByBullet(HitByBulletEvent bulletHitBotEvent)
    {
        TurnRight(60);
    }

    public override void OnHitWall(HitWallEvent e)
    {
        Back(50 + random.Next(30));
        TurnLeft(90 + random.Next(45));
    }
    public double FiringPower(double distance){
        if(distance > 200){
            return 1;
        }
        else if (distance > 100){
            return 2;
        }
        else{
            return 3;
        }
    } 

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
        return bot.TurnRemaining == 0;
    }
}