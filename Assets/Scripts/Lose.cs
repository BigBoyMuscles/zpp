using UnityEngine;
using UnityEngine.SceneManagement;

public class Lose : MonoBehaviour
{
    public float loseHeight = -5f;

	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKey(KeyCode.R))
	    {
	        SceneManager.LoadScene("Game");
	    }
	}
}
