using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class AEZAKMI : Bot
{
    private static readonly Random random = new Random();
    private const int SafeMargin = 100; // Batas area aman
    private double battlefieldWidth, battlefieldHeight;
    private double lastScannedX, lastScannedY;

    static void Main(string[] args) => new AEZAKMI().Start();

    AEZAKMI() : base(BotInfo.FromFile("AEZAKMI.json")) { }

    public override void Run()
    {
        BodyColor = Color.DarkBlue;
        battlefieldWidth = ArenaWidth;
        battlefieldHeight = ArenaHeight;
        
        while (IsRunning)
        {
            MoveGreedy();
        }
    }

    private void MoveGreedy()
    {
        double bestAngle = 0;
        double maxDistance = 0;
        
        for (int i = 0; i < 360; i += 45)
        {
            double newX = X + Math.Cos(DegreesToRadians(i)) * 100;
            double newY = Y + Math.Sin(DegreesToRadians(i)) * 100;
            if (IsSafePosition(newX, newY))
            {
                double distance = DistanceTo(lastScannedX, lastScannedY);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    bestAngle = i;
                }
            }
        }
        
        SetTurnRight(bestAngle);
        Forward(100);
    }

    private bool IsSafePosition(double x, double y)
    {
        return x > SafeMargin && x < battlefieldWidth - SafeMargin &&
               y > SafeMargin && y < battlefieldHeight - SafeMargin;
    }

    private double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        lastScannedX = e.X;
        lastScannedY = e.Y;
        LockRadar(e);
        PredictiveFire(e);
    }

    private void LockRadar(ScannedBotEvent e)
    {
        double angleToEnemy = BearingTo(e.X, e.Y);
        double radarTurn = NormalizeBearing(angleToEnemy - GunDirection);
        SetTurnGunRight(radarTurn);
    }

    private void PredictiveFire(ScannedBotEvent e)
    {
        double bulletSpeed = 20 - (3 * 2); // Power 2 bullet
        double distance = DistanceTo(e.X, e.Y);
        double time = distance / bulletSpeed;
        double futureX = e.X + Math.Cos(DegreesToRadians(e.Direction)) * e.Speed * time;
        double futureY = e.Y + Math.Sin(DegreesToRadians(e.Direction)) * e.Speed * time;
        double fireAngle = BearingTo(futureX, futureY);
        SetTurnGunRight(NormalizeBearing(fireAngle - GunDirection));
        Fire(2);
    }

    private double DistanceTo(double x, double y)
    {
        return Math.Sqrt(Math.Pow(x - X, 2) + Math.Pow(y - Y, 2));
    }

    public override double BearingTo(double x, double y)
    {
        return Math.Atan2(y - Y, x - X) * (180 / Math.PI);
    }

    private double NormalizeBearing(double angle)
    {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }

    public override void OnHitWall(HitWallEvent e)
    {
        Back(50);
        TurnRight(random.Next(90, 180));
    }
}