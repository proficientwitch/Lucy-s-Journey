using UnityEngine;

public class PrefabSpawner : Spawner
{
    protected static PrefabSpawner instance;
    public static PrefabSpawner Instance => instance;

    public static string DashSmoke = "Player_Dash_Smoke";
    public static string HuntressSpear = "Huntress_Spear";

    protected override void Awake()
    {
        base.Awake();
        if (instance != null) Debug.LogError("PrefabSpawner already exist");
        PrefabSpawner.instance = this;
    }
}