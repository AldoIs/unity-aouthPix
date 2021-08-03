using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int fuerza;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  public  void getFuerza()
    {
        Debug.Log("La fuerza es " + this.fuerza);
    }
}
