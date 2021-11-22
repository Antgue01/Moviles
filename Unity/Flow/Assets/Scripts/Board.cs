using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    public GameObject gameBoxPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setMap(Map p)
    {
        _map = p;
    }

        

    Map _map;
    //Vector<GameBox> _box;
}
