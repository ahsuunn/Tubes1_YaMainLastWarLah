using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class AltBot1 : Bot
{   
    int enemies;
    bool movingForward;
    /* A bot that drives forward and backward, and fires a bullet */
    double firepower;
    double enemyDistance;

    static void Main(string[] args)
    {
        new AltBot1().Start();
    }

    AltBot1() : base(BotInfo.FromFile("AltBot1.json")) { }

    public override void Run()
    {
        enemies = EnemyCount;
        movingForward = true;
        enemyDistance = 0 ;
        Random random = new Random();


        /* Customize bot colors, read the documentation for more information */
        BodyColor = Color.White;
        TurretColor = Color.White;
        RadarColor = Color.White;
        BulletColor = Color.White;
        ScanColor = Color.White;
        TracksColor = Color.White;
        GunColor = Color.White;

        // SetTurnRadarRight(Double.PositiveInfinity);
        while (IsRunning)
        {
                // SetRescan();
                // Go();
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


    public override void OnScannedBot(ScannedBotEvent e)
    {
        double enemyX = e.X;
        double enemyY = e.Y;
        // enemyDistance = DistanceTo(e.X, e.Y);
        // SetFirepower(3);    
        SetFire(3);

        // double radarTurn = RadarBearingTo(e.X, e.Y);
        
        // SetTurnRadarRight(radarTurn);        
        // Go();
        WaitFor(new TurnCompleteCondition(this));
    }

    public override void OnHitBot(HitBotEvent e)
    {
        Console.WriteLine("Ouch! I hit a bot at " + e.X + ", " + e.Y);
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
        Console.WriteLine("Ouch! I hit a wall, must turn back!");
    }

    /* Read the documentation for more events and methods */
    public override void OnBotDeath(BotDeathEvent e) {
        enemies--;  // Decrease enemy count when a bot dies
    }

    public void SetFirepower(double distance) { // Custom firepower setter
        if(distance > 500){
            firepower = 1;
        }
        else if(distance > 400){
            firepower = 1.5;
        }
        else if(distance > 300){
            firepower = 2;
        }
        else if(distance > 200){
            firepower = 2.5;
        }
        else{
            firepower = 3;
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
        // turn is complete when the remainder of the turn is zero
        return bot.TurnRemaining == 0;
    }
}