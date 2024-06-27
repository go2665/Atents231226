using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishRaceScreen : MonoBehaviour
{
    [SerializeField] private Text[] _winnerNickText = new Text[3];
    [SerializeField] private Image[] _winnerImage = new Image[3];

    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void SetWinner(string nick, Color color, int place)
    {
        _winnerNickText[place].text = nick;
        _winnerImage[place].color = color;

        _winnerImage[place].gameObject.SetActive(true);
        _winnerNickText[place].gameObject.SetActive(true);
    }

    public void FadeIn()
    {
        _anim.Play("FadeIn");
    }

    public void FadeOut()
    {
        _anim.Play("FadeOut");
    }
}
