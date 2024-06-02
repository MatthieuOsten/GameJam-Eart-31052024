using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Soldiers")]
    [SerializeField] private ScriptableArmy _army;
    [SerializeField] private List<Character> _soldiers;
    [SerializeField] private int _actualSoldier = 0;

    public ScriptableArmy Army
    {
        get { return _army; }
    }

    public List<Character> Soldiers
    {
        get { return _soldiers; }
        set { _soldiers = value; }
    }

    public GameObject actualSoldier
    {
        get { 
            if (_actualSoldier >= 0 && _actualSoldier < _soldiers.Count)
            {
                return _soldiers[_actualSoldier].gameObject;
            }

            return null;
         }
    }

    public bool IsLost
    {
        get
        {

            foreach (var soldier in _soldiers)
            {
                if (soldier.Health > 0) { 
                    return false;
                }
            }

            return true;
        }
    }

    private void Start()
    {
        if (_army == null) { Debug.LogWarning(name + " | Player dont correctly setup -> ScriptableArmy is null"); }

    }

    private void Update()
    {
        UpdateHealthSoldiers();
    }

    public void AddSoldier(GameObject soldier)
    {
        Character character = null;

        if (soldier.TryGetComponent<Character>(out character))
        {
            _soldiers.Add(character);
        }
        else
        {
            Debug.LogWarning(name + " dont add " + soldier.name + " dont have Character class");
        }
    }

    private void UpdateHealthSoldiers()
    {
        for (int i = 0; i < _army.Soldiers.Length; i++)
        {
            if (_soldiers.Count > 0 && _soldiers.Count > i)
            {
                if (_army.Soldiers[i] == null || _soldiers[i] == null) { continue; }

                _army.Soldiers[i].Health = _soldiers[i].Health;
            }
        }
    }

    public void SpawnSoldiers(List<Transform> spawnPoints, int index)
    {

        // Detach all point of principal list

        List<Transform> usablePoint = new List<Transform>();

        foreach (Transform point in spawnPoints[index])
        {
            usablePoint.Add(point);
        }

        // Tags

        string tagPlayer = "Player" + index;
        transform.tag = tagPlayer;

        // Spawn all soldier of this player

        int spawnableSoldier = Army.Soldiers.Length;

        while (spawnableSoldier > 0)
        {
            int random = Random.Range(0, usablePoint.Count);

            GameObject instance = Instantiate(Army.Unit.GetPrefab(Army.Soldiers[spawnableSoldier - 1].Skin), transform);

            Character character;
            if (!instance.TryGetComponent<Character>(out character))
            {
                character = instance.AddComponent<Character>();
                Debug.LogWarning("Prefab dont have class CHARACTER");
            }

            Soldiers.Add(instance.GetComponent<Character>());

            instance.transform.tag = tagPlayer;

            usablePoint.RemoveAt(random);
            spawnableSoldier--;
        }

        Debug.Log(name + " | Player " + index + " is totaly spawner " + Army.Soldiers.Length + " soldier, prepare to fight");

    }

}
