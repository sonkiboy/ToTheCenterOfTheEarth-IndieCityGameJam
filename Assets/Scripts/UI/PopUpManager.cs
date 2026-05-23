using UnityEngine;

public class PopUpManager : MonoBehaviour
{

    public GameObject PowerPopUp;
    
    public void DisplayPowerPopUp(PowerUpBehavior.PowerUpTypes type, Vector2 point)
    {
        Instantiate(PowerPopUp, point, Quaternion.identity).GetComponent<Animator>().SetInteger("Setting", (int)type);
        
    }
}
