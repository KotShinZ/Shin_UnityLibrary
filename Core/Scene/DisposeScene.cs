using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using System;

namespace Shin_UnityLibrary
{
    public class DisposeScene : MonoBehaviour
    {
        string sceneName;
        IDisposable disposable;

        public DisposeScene(IDisposable d, string name)
        {
            disposable = d;
            sceneName = name;
            SceneManager.sceneUnloaded += this.DisposeCharged;
        }
        void DisposeCharged(Scene scene)
        {
            if (scene.name == sceneName)
            {
                disposable.Dispose();
                SceneManager.sceneUnloaded -= this.DisposeCharged;
            }
            Destroy(this);
        }

        public static void Dispose(IDisposable d, string name)
        {
            var a = new DisposeScene(d, name);
        }
    }
}

