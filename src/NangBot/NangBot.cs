// using System;
// using System.Drawing;
// using Robocode.TankRoyale.BotApi;
// using Robocode.TankRoyale.BotApi.Events;

// public class NangBot : Bot
// {
//     static void Main(string[] args) => new NangBot().Start();

//     NangBot() : base(BotInfo.FromFile("NangBot.json")) { }

//     private bool enemySpotted = false;
//     private const double WALL_MARGIN = 50;
//     private double lastEnemyX, lastEnemyY;
//     private int scanSpeed = 30;

//     public override void Run()
//     {
//         BodyColor = Color.Blue;
//         TurretColor = Color.Black;
//         RadarColor = Color.Red;
//         BulletColor = Color.Yellow;
//         ScanColor = Color.White;

//         while (IsRunning)
//         {
//             if (!enemySpotted)
//                 SetTurnRadarRight(scanSpeed); // Scan lebih cepat saat tidak ada musuh
//             else
//                 SetTurnRadarRight(10); // Scan lebih presisi saat ada musuh

//             AvoidWalls();
//             SetForward(80);
//             Go();
//         }
//     }

//     public override void OnScannedBot(ScannedBotEvent evt)
//     {
//         enemySpotted = true;
//         double bearing = evt.Direction - Direction;
//         double distance = DistanceTo(evt.X, evt.Y);
//         double absoluteBearing = Direction + bearing;

//         lastEnemyX = X + Math.Sin(DegreesToRadians(absoluteBearing)) * distance;
//         lastEnemyY = Y + Math.Cos(DegreesToRadians(absoluteBearing)) * distance;

//         // Radar mengikuti musuh
//         SetTurnRadarRight(GunBearingTo(lastEnemyX, lastEnemyY));

//         // Hitung posisi prediksi musuh
//         double bulletPower = Math.Min(3, Energy / 4);
//         double bulletSpeed = 20 - (3 * bulletPower);
//         double time = distance / bulletSpeed;
//         double predictedX = lastEnemyX + Math.Sin(DegreesToRadians(evt.Direction)) * evt.Speed * time;
//         double predictedY = lastEnemyY + Math.Cos(DegreesToRadians(evt.Direction)) * evt.Speed * time;

//         // Arahkan turret ke posisi prediksi
//         SetTurnGunRight(GunBearingTo(predictedX, predictedY));

//         // Tetap bergerak saat menembak
//         SetForward(50);
//         SetTurnRight(10);
//         Fire(bulletPower);
//     }

//     public override void OnTick(TickEvent e)
//     {
//         if (!enemySpotted)
//         {
//             scanSpeed = 30; // Scan lebih cepat jika tidak melihat musuh
//             SetTurnRadarRight(scanSpeed);
//         }
//     }

//     private void AvoidWalls()
//     {
//         if (X < WALL_MARGIN || X > ArenaWidth - WALL_MARGIN || Y < WALL_MARGIN || Y > ArenaHeight - WALL_MARGIN)
//         {
//             SetBack(50);
//             SetTurnRight(45);
//         }
//     }

//     private double DegreesToRadians(double degrees) => degrees * Math.PI / 180.0;
// }



using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class NangBot : Bot
{
    static void Main(string[] args)
    {
        new NangBot().Start();
    }

    NangBot() : base(BotInfo.FromFile("NangBot.json")) { }

    private double enemyX, enemyY, enemyHeading, enemyVelocity;
    private bool movingRight = true;
    private const double WALL_MARGIN = 50; // Jarak aman dari dinding

    private double scanSpeed = 100;
    private bool lastSawEnemy = false;
    public override void Run()
    {
        // Warna Bot (menggabungkan gaya AdvancedBot & TrackFire)
        BodyColor = Color.Blue;
        TurretColor = Color.Black;
        RadarColor = Color.Red;
        BulletColor = Color.Yellow;
        ScanColor = Color.White;

        while (IsRunning)
        {
            AvoidWalls();
            SmoothMovement();
            TurnGunLeft(scanSpeed); // Terus scanning musuh

            if (!lastSawEnemy){
                scanSpeed = 100;
                lastSawEnemy = false;
            }
        }
    }

    private void SmoothMovement()
    {
        if (movingRight)
        {
            SetTurnRight(40);
        }
        else
        {
            SetTurnLeft(40);
        }
        SetForward(80);
        movingRight = !movingRight;
    }

    private void AvoidWalls()
    {
        if (X < WALL_MARGIN || X > ArenaWidth - WALL_MARGIN || Y < WALL_MARGIN || Y > ArenaHeight - WALL_MARGIN)
        {
            SetBack(50); // Mundur jika terlalu dekat dengan dinding
            movingRight = !movingRight; // Ubah arah
        }
    }

    public override void OnScannedBot(ScannedBotEvent evt)
    {
        // Hitung posisi musuh
        double bearing = evt.Direction - Direction;
        double distance = DistanceTo(evt.X, evt.Y);
        double absoluteBearing = Direction + bearing;
        enemyX = X + Math.Sin(DegreesToRadians(absoluteBearing)) * distance;
        enemyY = Y + Math.Cos(DegreesToRadians(absoluteBearing)) * distance;
        enemyHeading = evt.Direction;
        enemyVelocity = evt.Speed;
        scanSpeed = 10;
        lastSawEnemy = true;
        
        // Hitung arah tembakan
        double bearingFromGun = GunBearingTo(evt.X, evt.Y);
        TurnGunLeft(bearingFromGun);

        if (Math.Abs(bearingFromGun) <= 3 && GunHeat == 0)
        {
            double firePower = Math.Min(3 - Math.Abs(bearingFromGun), Energy - 0.1);
            Fire(firePower);
        }

        // Rescan jika perlu
        if (bearingFromGun == 0)
        {
            Rescan();
        }

    }

    public override void OnWonRound(WonRoundEvent e)
    {
        // Tarian kemenangan
        TurnLeft(36_000);
    }

    private double DegreesToRadians(double degrees) => degrees * Math.PI / 180.0;
    private double RadiansToDegrees(double radians) => radians * 180.0 / Math.PI;
}

