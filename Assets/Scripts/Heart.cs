using UnityEngine;
using System.Collections;
/// <summary>
/// klasy tworzenia obiektu "Heart" i dodawanie hp dla gracza
/// </summary>
public class Heart : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Character character = collider.GetComponent<Character>();
        
        if (character)
        {
            character.Lives++;
            Destroy(gameObject);
        }
    }
}
