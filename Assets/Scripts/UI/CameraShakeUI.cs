using System.Collections;
using UnityEngine;

public class CameraShakeUI : MonoBehaviour
{

    public float ShakeAmount = .0625f;
    public float ShakeRate = .125f;

    bool isShaking = false;

    Vector3 originLocalPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originLocalPos = transform.localPosition;
        GameManager.Instance.OnPlayerDamaged += OnPlayerDamaged;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        GameManager.Instance.OnPlayerDamaged -= OnPlayerDamaged;

    }

    public void OnPlayerDamaged(object sender, int health)
    {
        if(health >  0 && GameManager.Instance.GameOptions.IsScreenShakeOn == true)
        {
            if(isShaking == false)
            {
                StartCoroutine(ShakeCameraFocus());
            }
        }
    }

    IEnumerator ShakeCameraFocus()
    {
        isShaking = true;
        for (int i = 0; i < 3; i++)
        {
            transform.localPosition = originLocalPos + (Vector3.right *ShakeAmount);
            yield return new WaitForSeconds(ShakeRate);
            transform.localPosition = originLocalPos - (Vector3.right * ShakeAmount);
            yield return new WaitForSeconds(ShakeRate);
        }
        isShaking = false;
        transform.localPosition = originLocalPos;

    }


}
