using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{
    // Start is called before the first frame update
    [TitleDescription()]
    public string title = "���������ƃA�v���P�[�V�������I��";
    public bool isStartQuit = true;

    void Start()
    {
        if(isStartQuit) QuitApp();
    }

    public void QuitApp()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
    Application.Quit();//�Q�[���v���C�I��
#endif
    }

}
