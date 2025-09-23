using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TitleManager : MonoBehaviour
{
    bool bShowDialog = false;

    public enum SignalType {
        SIGNAL_SELECT_NONE,
        SIGNAL_SELECT_BTN_A,
        SIGNAL_SELECT_BTN_B,
    };
    SignalType signal;

    void Start()
    {
        bShowDialog = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ScnManager.Next();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !bShowDialog)
        {
            bShowDialog = true;
        }

        ScnManager.GetUIObject("S_Dialog").SetActive(bShowDialog);

        // signal handling

        switch (signal)
        {
            case SignalType.SIGNAL_SELECT_NONE:
                {
                    break;
                }
            case SignalType.SIGNAL_SELECT_BTN_A:
                {
                    ScnManager.Exit();
                    signal = SignalType.SIGNAL_SELECT_NONE;
                    break;
                }
            case SignalType.SIGNAL_SELECT_BTN_B:
                {
                    bShowDialog = false;
                    signal = SignalType.SIGNAL_SELECT_NONE;
                    break;
                }
        }
    }

    public void SendSignal(int newSignal)
    {
        signal = (SignalType)newSignal;
    }
}
