using Control;
using FlightScripts;
using Parts;
using UnityEditor;
using DragAndDrop = BuildingScripts.DragAndDrop;

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
    
    public bool PlayerWon { get; private set; }
    public int DestroyedAsteroids { get; private set; }
    public int LostShipParts { get; private set; }
    public int DestroyedEnemies { get; private set; }
    public int CollectedResources { get; private set; }
    public long TimeInLevel { get; private set; }
    public long TotalTime { get; private set; }
    public int CompletedLevels { get; private set; }
    public int UsedShipParts { get; private set; }
    public long TotalScore
    {
        get
        {
            var score = this.PlayerWon ? 50L : -25L;
            score += this.DestroyedAsteroids;
            score -= this.LostShipParts;
            score += this.DestroyedEnemies;
            score += this.CollectedResources;
            score += this.TotalTime / 1000 / 10;
            score += this.CompletedLevels * 50;
            score -= this.UsedShipParts;
            return score;
        }
    }

    private StatTracker()
    {
        AsteroidBehaviour.AsteroidDestroyedEvent += (sender, args) => { this.DestroyedAsteroids++; };
        SpaceshipPart.ShipPartLostEvent += (sender, args) => { this.LostShipParts++; };
        SpaceshipPart.ResourceCollectedEvent += (sender, args) => { this.CollectedResources++; };
        GameManager.LevelCompletedEvent += (sender, args) =>
        {
            this.PlayerWon = args.Won;
            this.TimeInLevel = args.TimeForLevel;
            this.TotalTime += args.TimeForLevel;
            this.CompletedLevels += args.Won ? 1 : 0;
        };
        DragAndDrop.ShipPartAddedEvent += (sender, args) => { this.UsedShipParts++; };
        DragAndDrop.ShipPartRemovedEvent += (sender, args) => { this.UsedShipParts--; };
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
