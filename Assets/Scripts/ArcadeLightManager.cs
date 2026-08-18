using UnityEngine;
using System.IO.Ports;
using System.Collections;
using System;

public class ArcadeLightManager : MonoBehaviour
{
    public enum LightModes
    {
        fade,
        alert,
        off
    }
    SerialPort serial = new SerialPort("COM6",9600);
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        try 
        { 
            serial.Open();
            serial.ReadTimeout = 100;
        }
        catch { }
        
    }
    void Start()
    {

        //serial.Write("9");
        //SetCabinetLights(LightModes.alert, 0);
        //StartCoroutine(testCycle());
    }
    string message;
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDisable()
    {
        if (serial.IsOpen)
        {
            serial.Write("9");
            serial.Close();
        }
    }

    private void OnApplicationQuit()
    {
        if (serial.IsOpen)
        {
            serial.Write("9");
            serial.Close();
        }
        
    }

    public void SetCabinetLights(LightModes mode, int pallete)
    {
        if (serial.IsOpen)
        {
            switch (mode)
            {
                case LightModes.fade:
                    if (pallete == 0)
                    {
                        serial.Write("1");

                    }
                    else if (pallete == 1)
                    {
                        serial.Write("2");

                    }
                    break;
                case LightModes.alert:
                    if (pallete == 0)
                    {
                        serial.Write("3");

                    }
                    else if (pallete == 1)
                    {
                        serial.Write("4");

                    }
                    break;
                case LightModes.off:
                    serial.Write("9");

                    break;
            }

        }

    }

    IEnumerator testCycle()
    {

        while (true)
        {
            SetCabinetLights(LightModes.fade, 0);
            yield return new WaitForSeconds(5);
            SetCabinetLights(LightModes.off, 0);
            yield return new WaitForSeconds(5);
            SetCabinetLights(LightModes.alert, 0);
            yield return new WaitForSeconds(5);
            SetCabinetLights(LightModes.off, 0);
            yield return new WaitForSeconds(5);
            SetCabinetLights(LightModes.fade, 1);
            yield return new WaitForSeconds(5);
            SetCabinetLights(LightModes.off, 0);
            yield return new WaitForSeconds(5);
            SetCabinetLights(LightModes.alert, 1);
            yield return new WaitForSeconds(5);
            SetCabinetLights(LightModes.off, 1);
            yield return new WaitForSeconds(5);
        }
        
    }

}
