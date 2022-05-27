using System.Collections;
using UnityEngine;

namespace UI.Game
{
    public class DiceAnimationController : MonoBehaviour
    {
        private bool doAnimate;
        public int AnimateDiceOnThrow(float delay, float speed)
        {
            gameObject.SetActive(true);
            doAnimate = true;
            StartCoroutine(WaitForAnimationEnd(delay));
            StartCoroutine(AnimateUntilEnd(speed));
            return Random.Range(1, 6) + Random.Range(1, 6);
        }
        
        IEnumerator WaitForAnimationEnd(float delay)
        {
            yield return new WaitForSeconds(delay);
            doAnimate = false;
            yield return new WaitForSeconds(0.01f);
            gameObject.SetActive(false);
        }

        IEnumerator AnimateUntilEnd(float speed)
        {
            transform.Rotate(Vector3.up * (speed * Time.deltaTime));
            transform.Rotate(Vector3.right * (speed * Time.deltaTime));
            transform.Rotate(Vector3.forward * (speed * Time.deltaTime));
            yield return new WaitForSeconds(0);
            if (doAnimate) StartCoroutine(AnimateUntilEnd(speed));
        }
    }
}
