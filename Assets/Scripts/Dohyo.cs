using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dohyo : MonoBehaviour
{
    public List<Transform> playersOnDohyo;
    public Sprite[] dhoyos;

    private void OnTriggerExit2D(Collider2D other)
    {
        if (playersOnDohyo.Contains(other.transform))
        {
            playersOnDohyo.Remove(other.transform);
            Instantiate(other.gameObject.GetComponent<CharacterController>().deathParticles.gameObject, other.gameObject.transform.position, Quaternion.identity);
            other.gameObject.SetActive(false);
            
            if(other.gameObject.GetComponent<DesktopControls>() == null)
            {
                //it is not the player 
                if (playersOnDohyo.Contains(other.gameObject.transform))
                {
                    FindObjectOfType<CharacterController>().hahaSFX.Play();
                }
            }
        }
    }
}