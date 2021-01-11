using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    private TextMesh textMesh;

    private void Awake() {
        textMesh = GetComponent<TextMesh>();
    }

    public void ShowScore(int score) {
        textMesh.text = score.ToString();
    }
}
