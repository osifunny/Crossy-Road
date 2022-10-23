using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Vector3 offset;
    Vector3 lastAnimalPos;

    private void Start(){
        offset = this.transform.position - player.transform.position;
    }

    private void Update(){
        if(lastAnimalPos != player.transform.position && !player.IsDie){
            var targetAnimalPos = new Vector3(
                player.transform.position.x,
                0,
                player.transform.position.z
            );
            transform.position = targetAnimalPos + offset;
            lastAnimalPos = player.transform.position;
        }
    }
}
