using UnityEngine;

public class PeristentSoundPlayer : MonoBehaviour
{


    public AK.Wwise.Event[] SoundEvents;

    AK.Wwise.Event PlayerShoot;
    AK.Wwise.Event PlayerJet;

    public AK.Wwise.RTPC MasterVolumeParam;
    public AK.Wwise.RTPC SoundVolumeParam;
    public AK.Wwise.RTPC MusicVolumeParam;

    // Start is called before the first frame update
    void Start()
    {


    }

    private void Update()
    {
    }

    public void PlayNonDiageticSound(string EventName)
    {
        //SKIPPED SOUNDS
        //if (EventName == "MainThemeStart" || EventName == "BossThemeStart")
        //{
        //    return;
        //}


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
        foreach (AK.Wwise.Event SoundEvent in SoundEvents)
        {
            if (SoundEvent.Name == EventName)
            {
                return SoundEvent;
            }
        }

        Debug.LogError("No Sound found with the name " + EventName);

        return null;


    }



}
