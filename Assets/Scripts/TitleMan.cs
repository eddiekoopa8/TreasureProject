using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TitleManager : MonoBehaviour
{
    bool bShowDialog = false;
    bool bShowMain = false;
    bool bShowCredits = false;
    bool bSentSignal = false;

    public enum SignalType {
        SIGNAL_NONE,              // Loop
        SIGNAL_SELECT_DLG_BTN_A,  // Exit? Yes
        SIGNAL_SELECT_DLG_BTN_B,  // Exit? No
        SIGNAL_SELECT_MAIN_BTN_A, // Play
        SIGNAL_SELECT_MAIN_BTN_B, // Options
        SIGNAL_SELECT_MAIN_BTN_C, // Credits
        SIGNAL_MAX,               // Shouldn't be used
    };
    SignalType signal;


    void Start()
    {
        bShowDialog = false;
        bShowMain = false;
    }

    void Update()
    {
        bool sentSignal = false;
        if (bSentSignal)
        {
            Debug.Log("sent "+(int)signal);
            sentSignal = true;
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            //ScnManager.Next();
            bShowMain = true;
            bShowCredits = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !bShowDialog)
        {
            bShowDialog = true;
        }

        ScnManager.GetUIObject("S_Dialog").SetActive(bShowDialog);
        ScnManager.GetUIObject("MainMenu").SetActive(!bShowDialog && bShowMain && !bShowCredits);
        ScnManager.GetUIObject("T_Start").SetActive(!bShowDialog && !bShowMain);
        ScnManager.GetUIObject("T_Quit").SetActive(!bShowDialog);
        ScnManager.GetUIObject("T_Credits").SetActive(!bShowDialog && bShowCredits);

        // signal handling

        switch (signal)
        {
            case SignalType.SIGNAL_NONE:
                {
                    break;
                }
            case SignalType.SIGNAL_SELECT_DLG_BTN_A:
                {
                    ScnManager.Exit();
                    break;
                }
            case SignalType.SIGNAL_SELECT_DLG_BTN_B:
                {
                    bShowDialog = false;
                    break;
                }
            case SignalType.SIGNAL_SELECT_MAIN_BTN_A:
                {
                    ScnManager.Next();
                    break;
                }
            case SignalType.SIGNAL_SELECT_MAIN_BTN_B:
                {
                    ScnManager.Next();
                    break;
                }
            case SignalType.SIGNAL_SELECT_MAIN_BTN_C:
                {
                    bShowCredits = true;
                    //ScnManager.Next();
                    break;
                }
        }

        if (sentSignal)
        {
            signal = SignalType.SIGNAL_NONE;
            bSentSignal = false;
        }
    }

    public void SendSignal(int newSignal)
    {
        Debug.Assert(signal > SignalType.SIGNAL_NONE || signal < SignalType.SIGNAL_MAX, "Bad Signal Parsed! ("+newSignal+")");
        signal = (SignalType)newSignal;
        bSentSignal = true;
    }
}
