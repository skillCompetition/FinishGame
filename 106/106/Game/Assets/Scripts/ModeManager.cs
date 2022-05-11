using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeManager : MonoBehaviour
{
    GameManager gameManager => GameManager.Instance;

    [SerializeField] Text modeText;
    AudioSource audio;

    int modeIndex;
    string[] modes = { "Easy", "Normal", "Hard" };

    private void Awake()
    {
        audio = modeText.GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        modeIndex = 1;
    }

    // Update is called once per frame
    void Update()
    {
        ModeController();
    }

    void ModeController()
    {
        switch (modeIndex)
        {
            case 0:
                modeText.color = Color.green;
                break;
            case 1:
                modeText.color = Color.white;
                break;
            case 2:
                modeText.color = Color.red;
                break;
            default:
                break;
        }
        modeText.text = modes[modeIndex];

    }

    public void ModeChangeBtnClick()
    {
        audio.Play();
        if (modeIndex >= 2)
            modeIndex = 0;
        else
            modeIndex++;
    }
}
