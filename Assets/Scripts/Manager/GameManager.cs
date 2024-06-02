using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class SceneInfos
    {
        [SerializeField] private string _name;
        [SerializeField] private string[] _tags;
        [SerializeField] private bool _isAdditive;

        [SerializeField] private SceneInfos[] _indexAdditiveScenes;

        public string Name
        {
            get { return _name; }
        }

        public bool IsAdditive
        {
            get { return _isAdditive; }
        }

        public SceneInfos[] IndexAdditiveScenes
        {
            get { return _indexAdditiveScenes; }
        }

        public bool CompareTag(string compare)
        {
            bool result = false;

            foreach (var tag in _tags)
            {
                if (string.Equals(tag, compare)) { result = true; }
            }

            return result;
        }
    }

    [SerializeField] private SceneInfos[] _scenes;

    public void SceneLoader(uint index)
    {
        SceneLoader(_scenes[index]);
    }

    public void SceneLoader(string name)
    {
        foreach (var info in _scenes)
        {
            if (string.Equals(info.Name,name))
            {
                SceneLoader(info);
                return;
            }
        }
    }

    public void SceneLoader(SceneInfos info)
    {

        if (info != null)
        {
            if (info.Name != string.Empty && info.Name != "")
            {
                if (info.IsAdditive)
                {
                    SceneManager.LoadScene(info.Name,LoadSceneMode.Additive);
                }
                else
                {
                    SceneManager.LoadScene(info.Name, LoadSceneMode.Single);
                }

                if (info.IndexAdditiveScenes.Length > 0)
                {
                    foreach (var additive in info.IndexAdditiveScenes)
                    {
                        if (additive != info)
                        {
                            SceneManager.LoadScene(additive.Name, LoadSceneMode.Additive);
                        }
                    }
                }
            }
        }

    }

    public void QuitGame()
    {
        Debug.Log("Quitting. . .");

        Application.Quit();
    }

}
