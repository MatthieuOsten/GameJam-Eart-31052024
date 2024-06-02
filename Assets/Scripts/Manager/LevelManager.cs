using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HelpURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ")]
public class LevelManager : MonoBehaviour
{
    #region SINGLETON

    /// <summary>
    ///  Force game to have only one LevelManager
    /// </summary>
    [SerializeField] private static LevelManager _instance = null;

    public static LevelManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<LevelManager>();
                // If true, instance is create
                if (_instance == null)
                {
                    var newObjectInstance = new GameObject();
                    newObjectInstance.name = typeof(LevelManager).ToString();
                    _instance = newObjectInstance.AddComponent<LevelManager>();
                }
            }
            return _instance;
        }
    }

    #endregion

    [SerializeField] private uint _actualPlayer = 0;
    [SerializeField] private ScriptableInt _scriptableNbrPlayer;

    [SerializeField] private List<List<Transform>> _spawnPoint = new List<List<Transform>>();

    [SerializeField] private Player[] _players = new Player[4];

    private void Start()
    {
        SetupPlayer();
    }

    private void SetupPlayer()
    {
        byte nbrPlayer = 2;

        if (_scriptableNbrPlayer != null) { nbrPlayer = (byte)_scriptableNbrPlayer.Value; }
        else
        {
            Debug.LogWarning("Scriptable Int is null, by default make two player");
        }

        _players = new Player[nbrPlayer];

        for (int i = 0; i < _players.Length; i++)
        {
            SpawnSoldiers(_players[i], i);
        }

    }

    private void SpawnSoldiers(Player player,int index)
    {

        if (player == null) { return; }
        if (index < 0 || index >= _players.Length) { Debug.LogError("Number of index is not work -> " + index + " nbr player -> " + _players.Length); }

        // Check nbr of player

        if (_scriptableNbrPlayer != null)
        {
            if (_spawnPoint.Count != _scriptableNbrPlayer.Value) { Debug.LogError("Dont have spawn point for all player -> " + _spawnPoint.Count); }
        }
        else
        {
            if (_spawnPoint.Count != 2) { Debug.LogError("(Dont have scriptable nbr player) Dont have spawn point for all player -> " + _spawnPoint.Count); }
        }

        // Check list of points

        if (_spawnPoint[index].Count < 4) { Debug.LogError("Dont have spawn point for all soldiers"); }

        // Detach all point of principal list

        List<Transform> usablePoint = new List<Transform>();

        foreach (var point in _spawnPoint[index])
        {
            usablePoint.Add(point);
        }

        // Spawn all soldier of this player

        int spawnableSoldier = player.Army.Soldiers.Length;

        while (spawnableSoldier > 0)
        {
            int random = Random.Range(0,usablePoint.Count);

            Instantiate(player.Army.Unit.GetPrefab(player.Army.Soldiers[spawnableSoldier - 1].Skin),player.transform);

            usablePoint.RemoveAt(random);
            spawnableSoldier--;
        }

        Debug.Log(name + " | Player " + index + " is totaly spawner " + player.Army.Soldiers.Length + " soldier, prepare to fight");

    }
}
