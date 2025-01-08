using System.Collections;
using TMPro;
using UnityEngine;

namespace Util
{
    public class TextFeedback : MonoBehaviour
    {
        public static IEnumerator FadeTimer(float feedbackDuration, TextMeshProUGUI scoreFeedbackText)
        {
            var num = feedbackDuration / 0.01;
            var pos = scoreFeedbackText.transform.localPosition;
            for (var i = 0; i < num; i++)
            {
                scoreFeedbackText.transform.localPosition += (Vector3.up * 0.01f);
                scoreFeedbackText.SetText("+1");
                scoreFeedbackText.alpha = 1.0f - i * 0.01f;
                yield return new WaitForSeconds(0.01f);
            }

            scoreFeedbackText.transform.localPosition = pos;
            scoreFeedbackText.alpha = 0.0f;
        }
    }
}