using UnityEngine;
using UnityEngine.SceneManagement; 
public class BackToPreviousScene : MonoBehaviour
{
     public void GoToPreviousScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int previousSceneIndex = currentSceneIndex - 1;
        if (previousSceneIndex >= 0)
        {
            SceneManager.LoadScene(previousSceneIndex); // ��ת����һ�㳡��
        }
    }
}