using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.Space)) // press space to restart
        {
            SceneManager.LoadScene("Main");
        }
    }
}
