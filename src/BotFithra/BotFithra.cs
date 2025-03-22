using System;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class BotFithra : Bot
{
    static void Main() => new BotFithra().Start();

    BotFithra() : base(BotInfo.FromFile("BotFithra.json")) { }

    private int moveDirection = 1;
    private double lastEnemyDirection = 0;

    public override void Run()
    {
        Console.WriteLine("BotFithra ready!");

        while (IsRunning)
        {
            // Gerak zig-zag dengan sudut random untuk menghindari tembakan
            Forward(100 * moveDirection);
            TurnLeft(30 * moveDirection);

            if (TurnNumber % 20 == 0)
                moveDirection *= -1;

            // Radar lock untuk mempertahankan fokus pada musuh
            if (GunHeat == 0) TurnRadarRight(360); // Putar jika belum melihat musuh
            Go();
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        Console.WriteLine($"Musuh terlihat di ({e.X:F2}, {e.Y:F2})");

        // Hitung jarak
        double dx = e.X - X;
        double dy = e.Y - Y;
        double distance = Math.Sqrt(dx * dx + dy * dy);

        // Tentukan power
        double bulletPower = GetFirePower(distance);
        double bulletSpeed = 20 - 3 * bulletPower;
        double timeToHit = distance / bulletSpeed;

        // Prediksi posisi musuh ke depan
        double predictedX = e.X + Math.Cos(ToRadians(e.Direction)) * e.Speed * timeToHit;
        double predictedY = e.Y + Math.Sin(ToRadians(e.Direction)) * e.Speed * timeToHit;

        // Batas arena
        predictedX = Clamp(predictedX, 18, ArenaWidth - 18);
        predictedY = Clamp(predictedY, 18, ArenaHeight - 18);

        // Kunci radar ke arah musuh
        double radarTurn = NormalizeRelativeAngle(AbsoluteBearing(X, Y, e.X, e.Y) - RadarDirection);
        TurnRadarRight(radarTurn);

        // Putar meriam ke arah posisi prediksi
        double angleToTarget = AbsoluteBearing(X, Y, predictedX, predictedY);
        double gunTurn = NormalizeRelativeAngle(angleToTarget - GunDirection);
        TurnGunRight(gunTurn);

        // Tembak jika hampir sejajar
        if (Math.Abs(GunTurnRemaining) < 2 && GunHeat == 0)
        {
            Fire(bulletPower);
        }

        // Simpan arah musuh
        lastEnemyDirection = e.Direction;
    }

    public override void OnHitByBullet(HitByBulletEvent evt)
    {
        Console.WriteLine("Kena peluru, coba manuver!");

        // Ganti arah dan hindari ke arah sebaliknya dari peluru
        TurnRight(90 - evt.Bullet.Direction + Random.Shared.Next(-15, 15));
        Back(100 + Random.Shared.Next(0, 50));
    }

    public override void OnHitWall(HitWallEvent evt)
    {
        Console.WriteLine("Nabrak tembok, berbalik arah!");
        Back(100);
        TurnRight(90);
        moveDirection *= -1;
    }

    public override void OnHitBot(HitBotEvent evt)
    {
        Console.WriteLine("Tabrak musuh, tabrak balik!");
        Back(50);
        Fire(3);
    }

    private double AbsoluteBearing(double x1, double y1, double x2, double y2)
    {
        return Math.Atan2(x2 - x1, y2 - y1) * 180 / Math.PI;
    }

    private double NormalizeRelativeAngle(double angle)
    {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }

    private double ToRadians(double degrees) => degrees * Math.PI / 180.0;

    private double GetFirePower(double distance)
    {
        if (distance > 500) return 1.0;
        if (distance > 200) return 2.0;
        return 3.0;
    }

    private double Clamp(double val, double min, double max)
    {
        return Math.Max(min, Math.Min(max, val));
    }
}