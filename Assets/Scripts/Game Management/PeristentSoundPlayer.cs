using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PeristentSoundPlayer : MonoBehaviour
{

    
    public AK.Wwise.Event[] SoundEvents;

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
        AK.Wwise.Event foundEvent = SearchForSoundByName(EventName);
        if (foundEvent != null)
        {
            foundEvent.Post(this.gameObject);
        }
    }

    public void PlaySoundOnObject(string eventName, GameObject obj)
    {
        AK.Wwise.Event foundEvent = SearchForSoundByName(eventName);
        if (foundEvent != null)
        {
            foundEvent.Post(obj);
        }
    }

    public void PlaySoundOnTransform(string eventName, Transform trans)
    {
        AK.Wwise.Event foundEvent = SearchForSoundByName(eventName);
        if (foundEvent != null)
        {
            foundEvent.Post(trans.gameObject);
        }
    }


    private AK.Wwise.Event SearchForSoundByName(string EventName)
    {
        foreach(AK.Wwise.Event SoundEvent in SoundEvents)
        {
            if(SoundEvent.Name == EventName)
            {
                return SoundEvent;
            }
        }

        Debug.LogError("No Sound found with the name " + EventName);

        return null;


    }
    


}
