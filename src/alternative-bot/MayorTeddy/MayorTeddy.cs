using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class MayorTeddy : Bot
{   
    int enemies;
    bool movingForward;
    double firepower;
    double enemyDistance;

    static void Main(string[] args)
    {
        new MayorTeddy().Start();
    }

    MayorTeddy() : base(BotInfo.FromFile("MayorTeddy.json")) { }

    public override void Run()
    {
        enemies = EnemyCount;
        movingForward = true;
        enemyDistance = 0 ;
        Random random = new Random();


        BodyColor = Color.White;
        TurretColor = Color.White;
        RadarColor = Color.White;
        BulletColor = Color.White;
        ScanColor = Color.White;
        TracksColor = Color.White;
        GunColor = Color.White;

        while (IsRunning)
        {
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

                SetForward(100);
                SetTurnRight(turnAngle);
                

            }
            
            }


    public override void OnScannedBot(ScannedBotEvent e)
    {
        double enemyX = e.X;
        double enemyY = e.Y;
        SetFire(3);
        WaitFor(new TurnCompleteCondition(this));
    }

    public override void OnHitWall(HitWallEvent e)
    {
        if (movingForward)
        {
            SetBack(150);
            movingForward = false;
            WaitFor(new TurnCompleteCondition(this));
        }
        else
        {
            SetForward(150);
            movingForward = true;
            WaitFor(new TurnCompleteCondition(this));
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