using UnityEngine;
using UnityEngine.SceneManagement;

public interface ISceneUpdate 
{
    public void OnSceneChanged(Scene Current, Scene Next);
    
    }
