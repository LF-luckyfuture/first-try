using UnityEngine;
using UnityEngine.SceneManagement; 
public class BackToPreviousScene : MonoBehaviour
{
     public void Restart()
    {
       
            SceneManager.LoadScene(1);   
    }
}