using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene2 : MonoBehaviour
{

    bool doLoad = false;
    void Update()
    {
        //Press the space key to start coroutine
        if (Input.GetKey(KeyCode.Space) && !doLoad)
        {
            doLoad = true;
            //Use a coroutine to load the Scene in the background
            StartCoroutine(LoadYourAsyncScene());
        }
    }

    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background at the same time as the current Scene.
        //This is particularly good for creating loading screens. You could also load the Scene by build //number.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scene2", LoadSceneMode.Additive);

        //Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;

            
        }

        Scene loadedScene = SceneManager.GetSceneByName("Scene2");
        Transform root = loadedScene.GetRootGameObjects()[0].transform;

        // This position assignment is run 1 frame after the scene was loaded and showed up at origin.
        root.position = new Vector3(100f, 200f, 300f);
        print("asdf "+ root+" "+ root.position);
    }
}