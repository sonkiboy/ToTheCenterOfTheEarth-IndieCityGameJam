using UnityEngine;

public class ConductiveTileMod : MonoBehaviour
{

    private bool _electrified = false;
    public bool IsElectrified
    {
        get
        {
            return _electrified;
        }
        set
        {
            _electrified=value;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
