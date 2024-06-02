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

    public Character[] Soldiers
    {
        get { return _soldiers.ToArray(); }
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

}
