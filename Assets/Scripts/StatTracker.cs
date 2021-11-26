using FlightScripts;
using Parts;

public class StatTracker
{
    private static StatTracker _instance;

    public static StatTracker Instance
    {
        get
        {
            _instance ??= new StatTracker();
            return _instance;
        }
    }
    
    public bool PlayerWon { get; set; }
    public int DestroyedAsteroids { get; private set; }
    public int LostShipParts { get; private set; }
    public int DestroyedEnemies { get; private set; }
    public int CollectedResources { get; private set; }

    private long _timeInLevel;
    public long TimeInLevel
    {
        get => this._timeInLevel;
        set
        {
            this._timeInLevel = value;
            this.TotalTime += value;
        }
    }

    public long TotalTime { get; private set; }
    public int CompletedLevels { get; private set; }
    public int UsedShipParts { get; private set; }

    public long TotalScore
    {
        get
        {
            var score = this.PlayerWon ? 50L : -10L;
            score += this.DestroyedAsteroids;
            score -= this.LostShipParts;
            score += this.DestroyedEnemies;
            score += this.CollectedResources;
            score += this.TotalTime / 1000 / 10;
            score += this.CompletedLevels;
            score -= this.UsedShipParts;
            return score;
        }
    }

    private StatTracker()
    {
        Projectile.AsteroidDestroyedEvent += (sender, args) => { this.DestroyedAsteroids++; };
        SpaceshipPart.ShipPartLostEvent += (sender, args) => { this.LostShipParts++; };
    }

    public static void InstantiateTracker()
    {
        _instance ??= new StatTracker();
    }

    public static void ResetTracker()
    {
        _instance = new StatTracker();
    }
}
