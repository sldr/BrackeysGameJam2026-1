using Godot;
using System;
using static Game;

public partial class GameStats : Node
{
    public enum PickupTypes
    {
        BlackHoles = 0,
        FireOrbs = 1,
        FireRings = 2,
        WaterLeafs = 3,
        Bubble = 4,
        Leafs = 5,
    }

    private int[] PickupCounts = new int[Enum.GetValues(typeof(PickupTypes)).Length];
    private int HealthRestores;
    public bool WonGame;
    public int Kills;
    public double TimeSeconds;

    public void ResetStats()
    {
        WonGame = false;
        Kills = 0;
        TimeSeconds = 0;
        HealthRestores = 0;
        PickupCounts = new int[Enum.GetValues(typeof(PickupTypes)).Length];
    }

    public static PickupTypes intToPickupTypes(int intPickupType)
    {
        return (PickupTypes)intPickupType;
    }

    public int GetHealthRestores()
    {
        return HealthRestores;
    }

    public int[] GetPickupCounts()
    {
        return PickupCounts;
    }

    public void AddHealthRestore(PickupTypes pickupType)
    {
        PickupCounts[(int)pickupType]++;
        HealthRestores++;
    }

}
