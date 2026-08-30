using UnityEngine;

public class LowFuelUI : MonoBehaviour
{

    Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        GameManager.Instance.Platform.OnLowFuel += OnFuelStateChanged;
    }

    private void OnDisable()
    {
        GameManager.Instance.Platform.OnLowFuel += OnFuelStateChanged;

    }

    void OnFuelStateChanged(object sender, bool isLow)
    {
        animator.SetBool("IsLowFuel", isLow);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
