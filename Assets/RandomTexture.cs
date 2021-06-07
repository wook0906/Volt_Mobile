using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTexture : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Managers.Resource.LoadAsync<Texture2D>(Random.Range(0, 9).ToString(), (result) =>
        {
            GetComponent<UITexture>().mainTexture = result.Result;
            if (Managers.Scene.CurrentScene != null &&
            Managers.Scene.CurrentScene.SceneType == Define.Scene.GameScene)
            {
                GameScene gameScene = Managers.Scene.CurrentScene as GameScene;
                gameScene.OnLoadedMatchingBGTexture();
            }
        });
    }
}
