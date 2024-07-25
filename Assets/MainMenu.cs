using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Transform rotatey;
    [SerializeField] Vector3 rotation = Vector3.up;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotatey.Rotate(rotation * Time.deltaTime);
    }

    public void StartGame()
    {
        //SceneManager.LoadScene("Loading Screen");
        GameManager.instance.LoadGame();
    }
}
