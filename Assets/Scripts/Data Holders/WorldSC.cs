using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class WorldSC : MonoBehaviour
{
    public NavMeshSurface Surface;

    public bool inUI;
    public List<GameObjects> ItemsInGame;
    public Settings settings;

    private Vector3 _wind = Vector3.forward;
    public Vector3 wind { get => _wind; set => _wind = value; }

    public void UpdateNavMesh()
    {
        Surface.BuildNavMesh();
    }


    private void Awake()
    {
        string jsonImport = File.ReadAllText(Application.dataPath + "/settings.cfg");
        settings = JsonUtility.FromJson<Settings>(jsonImport);
        PlayerController p = GameObject.Find("Player").GetComponent<PlayerController>();
        p.mouseSensetivity = settings.mouseSensetivity;

        wind = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        // UpdateNavMesh();
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

    [Header("Building Data")]
    public bool isPlaceable;
    public GameObject prefab;
    public byte buildingID;
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