using UnityEngine;
using System.Collections;
using TMPro;

public class TextMeshProCounter : MonoBehaviour
{
    private int numMoves = 0;
    private TextMeshProUGUI textMeshPro;
    void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    public void setCounter(int moves)
    {
        numMoves = moves;
        textMeshPro.SetText("" + numMoves);
    }

    public void incCounter()
    {
        ++numMoves;
        textMeshPro.SetText("" + numMoves);
    }
}