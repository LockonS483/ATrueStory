using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCard : MonoBehaviour
{
    void OnMouseDown(){
        SceneManager.LoadScene("WinScene", LoadSceneMode.Single);
    }
}
