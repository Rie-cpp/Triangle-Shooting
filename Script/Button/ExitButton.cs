using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class ExitButton : MonoBehaviour
{
    public void OnClickStartButton()
    {
       Application.Quit();
    }
}
