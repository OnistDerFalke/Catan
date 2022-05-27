using System.Collections;
using DataStorage;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    /// <summary>
    /// Class controls 3D UI Dice on loading
    /// </summary>
    public class DiceAnimationController : MonoBehaviour
    {
        [Header("Dice Settings")][Space(5)]
        [Tooltip("Thrown Number Text")][SerializeField]
        private Text thrownNumberText;
        [Tooltip("Dice Animation Duration")]
        [SerializeField] private float diceAnimationDuration;
        [Tooltip("Dice Animation Speed")]
        [SerializeField] private float diceAnimationSpeed;
        
        private bool doAnimate;
        private int throwingTextState;
        
        /// <summary>
        /// Animates the 3D dice and throws the dice
        /// </summary>
        public void AnimateDiceOnThrow()
        {
            throwingTextState = 0;
            gameObject.SetActive(true);
            doAnimate = true;
            StartCoroutine(WaitForAnimationEnd(diceAnimationDuration));
            StartCoroutine(AnimateThrowingText());
            StartCoroutine(AnimateUntilEnd(diceAnimationSpeed));
            GameManager.CurrentDiceThrownNumber = Random.Range(1, 6) + Random.Range(1, 6);
        }
        
        /// <summary>
        /// Removes the dice after using it and updates thrown number text;
        /// </summary>
        /// <param name="delay">Dice animation duration</param>
        /// <returns></returns>
        IEnumerator WaitForAnimationEnd(float delay)
        {
            yield return new WaitForSeconds(delay);
            doAnimate = false;
            yield return new WaitForSeconds(0.01f);
            thrownNumberText.text = GameManager.CurrentDiceThrownNumber.ToString();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Rotates (animates) the dice
        /// </summary>
        /// <param name="speed">Speed of the animation rotation</param>
        /// <returns></returns>
        IEnumerator AnimateUntilEnd(float speed)
        {
            transform.Rotate(Vector3.up * (speed * Time.deltaTime));
            transform.Rotate(Vector3.right * (speed * Time.deltaTime));
            transform.Rotate(Vector3.forward * (speed * Time.deltaTime));
            yield return new WaitForSeconds(0);
            if (doAnimate) StartCoroutine(AnimateUntilEnd(speed));
        }

        /// <summary>
        /// Animates the text of throwing the dice
        /// </summary>
        /// <returns></returns>
        IEnumerator AnimateThrowingText()
        {
            switch (throwingTextState)
            {
                case 0:
                    thrownNumberText.text = "TRWA LOSOWANIE";
                    break;
                case 1:
                case 2:
                    thrownNumberText.text += ".";
                    break;
            }
            throwingTextState = (throwingTextState + 1) % 3;
            yield return new WaitForSeconds(0.2f);
            if (doAnimate) StartCoroutine(AnimateThrowingText());
        }
    }
}
