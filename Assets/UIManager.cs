using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private float chestAppearTime = 0.25f;
    [SerializeField] private CanvasGroup chestGroup;
    [SerializeField] private RectTransform chestRect;
    [SerializeField] private List<GameObject> chests = new List<GameObject>();

    public void PanelFadeIn()
    {
        chestGroup.alpha = 0f;
        chestRect.transform.localPosition = new Vector3(0f, -1000f, 0f);
        chestRect.DOAnchorPos(new Vector2(0f, 0f), fadeTime, false).SetEase(Ease.OutElastic);
        chestGroup.DOFade(1, fadeTime);
        StartCoroutine("ChestsAnimation");
    }

    public void PanelFadeOut()
    {
        chestGroup.alpha = 1f;
        chestRect.transform.localPosition = new Vector3(0f, 0f, 0f);
        chestRect.DOAnchorPos(new Vector2(0f, -1000f), fadeTime, false).SetEase(Ease.InOutQuint);
        chestGroup.DOFade(0, fadeTime);
    }

    IEnumerator ChestsAnimation()
    {
        foreach(var chest in chests)
        {
            chest.transform.localScale = Vector3.zero;
        }

        foreach(var chest in chests)
        {
            chest.transform.DOScale(1f, fadeTime).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(chestAppearTime);
        }
    }

}
