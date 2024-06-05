using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSurvey : MonoBehaviour
{
    public void OpenSurveyLink()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSdURiFaqUHVn6uTUwK5INcS1-5zTlQS4vODYj8D4cVmavGxBA/viewform?usp=sf_link");
    }
}
