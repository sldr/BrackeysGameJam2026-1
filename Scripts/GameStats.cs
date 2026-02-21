using Godot;
using System;

public partial class GameStats : Node
{
    public bool WonGame;
    public int Kills;
    public double TimeSeconds;
    public int HealthRestores;

    public void ResetStats()
    {
        WonGame = false;
        Kills = 0;
        TimeSeconds = 0;
        HealthRestores = 0;
    }

}
