using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public static List<Vector3> allPositions = new List<Vector3>();


    private void OnEnable(){
        allPositions.Add(this.transform.position);
    }

    private void OnDisable(){
        allPositions.Remove(this.transform.position);
    }
}
