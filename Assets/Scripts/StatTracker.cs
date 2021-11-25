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
    public int TimeInLevel { get; private set; }
    public int CompletedLevels { get; private set; }
    public int UsedShipParts { get; private set; }

    public int TotalScore
    {
        get
        {
            var score = 
                    this.DestroyedAsteroids
                    - this.LostShipParts
                    + this.DestroyedEnemies
                    + this.CollectedResources
                    + this.CompletedLevels
                    - this.UsedShipParts;
            return score;
        }
    }

    private StatTracker()
    {
        Projectile.AsteroidDestroyedEvent += (sender, args) => { this.DestroyedAsteroids++; };
    }

    public static void InstantiateTracker()
    {
        _instance ??= new StatTracker();
    }
}
