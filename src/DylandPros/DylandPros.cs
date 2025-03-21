using System;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class DylandPros : Bot {
    static void Main() => new DylandPros().Start();

    private readonly Random rng = new();
    private bool targetVisible = false;
    private int turnsSinceLastScan = 0;
    private const int MaxLostTurns = 15;
    private int moveCooldown = 0;

    DylandPros() : base(BotInfo.FromFile("DylandPros.json")) { }

    public override void Run(){
        Console.WriteLine("DylandPros lagi berak...");
    }

    public override void OnTick(TickEvent e){
        if (!targetVisible){
            if (moveCooldown <= 0){
                SetForward(rng.Next(100, 300));
                SetTurnRight(rng.Next(-90, 90));
                moveCooldown = rng.Next(20, 40);
            }
            moveCooldown--;
            SetTurnGunRight(45);
        }
        else{
            SetForward(20);
            SetTurnRight(5);
            SetTurnGunRight(20);

            turnsSinceLastScan++;
            if (turnsSinceLastScan > MaxLostTurns){
                targetVisible = false;
                turnsSinceLastScan = 0;
            }
        }
    }

    public override void OnScannedBot(ScannedBotEvent e){
        targetVisible = true;
        turnsSinceLastScan = 0;

        double bearingFromGun = GunBearingTo(e.X, e.Y);
        SetTurnGunLeft(bearingFromGun);

        if (Math.Abs(bearingFromGun) <= 3 && GunHeat == 0){
            double power = Math.Min(3 - Math.Abs(bearingFromGun), Energy - 0.1);
            Fire(power);
        }

        if (Math.Abs(bearingFromGun) < 0.01){
            Rescan();
        }
    }

    public override void OnHitWall(HitWallEvent e){
        Back(50);
        TurnLeft(rng.Next(90, 180));
    }

    public override void OnHitBot(HitBotEvent e){
        Back(40);
        Fire(3);
    }

    public override void OnHitByBullet(HitByBulletEvent e){
        TurnLeft(rng.Next(-90, 90));
        Forward(rng.Next(40, 100));
    }
}
