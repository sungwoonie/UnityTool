using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ContinuousButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent pressAction;

    private bool pressing;
    private Coroutine enhanceCoroutine;

    [SerializeField] private float startDelay = 0.5f;
    [SerializeField] private float minDelay = 0.05f;
    [SerializeField] private float acceleration = 0.1f;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (enhanceCoroutine != null)
            StopCoroutine(enhanceCoroutine);
        pressing = true;

        enhanceCoroutine = StartCoroutine(Enhance());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressing = false;
        if (enhanceCoroutine != null)
        {
            StopCoroutine(enhanceCoroutine);
            enhanceCoroutine = null;
        }
    }

    private IEnumerator Enhance()
    {
        float currentDelay = startDelay;

        pressAction?.Invoke();

        while (pressing)
        {
            yield return new WaitForSeconds(currentDelay);

            if (!pressing) yield break;

            pressAction?.Invoke();
            currentDelay = Mathf.Max(minDelay, currentDelay - acceleration);
        }
    }
}
