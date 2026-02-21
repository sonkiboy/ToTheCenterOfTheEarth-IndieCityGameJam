using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PeristentSoundPlayer : MonoBehaviour
{
    AK.Wwise.Event PlayerShoot;
    AK.Wwise.Event PlayerJet;

    // Start is called before the first frame update
    void Start()
    {

        
    }

    private void Update()
    {
    }

    public void PlayNonDiageticSound(string EventName)
    {

    }

    public void PlaySoundOnObject(string eventName, GameObject obj)
    {

    }

    public void PlaySoundOnTransform(string eventName, Transform trans)
    {

    }


    private AK.Wwise.Event SearchForSoundByName(string EventName)
    {
        switch(EventName)
        {
            case "PlayerShoot":

                return PlayerShoot;

            case "PlayerJet":

                return PlayerJet;
            
            default:

                Debug.LogError("No Sound found with that name");

                return null;

            


        }
    }
    


}
