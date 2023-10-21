using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{
    // Start is called before the first frame update
    [TitleDescription()]
    public string title = "生成されるとアプリケーションを終了";
    public bool isStartQuit = true;

    void Start()
    {
        if(isStartQuit) QuitApp();
    }

    public void QuitApp()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
    }

}
