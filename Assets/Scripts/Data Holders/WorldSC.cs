using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;

public class WorldSC : MonoBehaviour
{
    [SerializeField]
    NavMeshSurface Surface;

    public void UpdateNavMesh()
    {
        Surface.BuildNavMesh();
    }

    public List<GameObjects> ItemsInGame;
    public Settings settings;

    private void Awake()
    {
        string jsonImport = File.ReadAllText(Application.dataPath + "/settings.cfg");
        settings = JsonUtility.FromJson<Settings>(jsonImport);
        PlayerController p = GameObject.Find("Player").GetComponent<PlayerController>();
        p.mouseSensetivity = settings.mouseSensetivity;
    }
}

[System.Serializable]
public class GameObjects
{
    [Header("Identification Data")]
    public byte ID;
    public string Name;
    public int StackAmount;
    public Sprite sprite;
    public int amount;

    [Header("Crafting Data")]
    public bool Craftable;
    public List<Recepie> recepie;
    public int amountWhenCrafted;
}

[System.Serializable]
public class Recepie
{
    public byte ID;
    public int amount;
}

[System.Serializable]
public class Settings
{
    [Range(0.1f, 10)]
    public float mouseSensetivity;
}