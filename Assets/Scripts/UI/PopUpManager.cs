using UnityEngine;

public class PopUpManager : MonoBehaviour
{

    public GameObject PowerPopUp;
    public GameObject FuelIncreasePopUp;
    
    public void DisplayPowerPopUp(PowerUpManager.PowerUpTypes type, Vector2 point)
    {
        Instantiate(PowerPopUp, point, Quaternion.identity).GetComponent<Animator>().SetInteger("Setting", (int)type);
        
    }

    public void DisplayFuelIncrease(Vector2 point)
    {
        Instantiate(FuelIncreasePopUp, point, Quaternion.identity);
    }
}
