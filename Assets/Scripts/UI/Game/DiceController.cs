using System.Collections;
using DataStorage;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    /// <summary>
    /// Class controls 3D UI Dice on loading
    /// </summary>
    public class DiceController : MonoBehaviour
    {
        [Header("Dice Animation Settings")][Space(5)]
        [Tooltip("Dice Animation Duration")]
        [SerializeField] private float diceAnimationDuration;
        [Tooltip("Dice Animation Speed")]
        [SerializeField] private float diceAnimationSpeed;
        [Tooltip("Dice Animation Speed")]
        [SerializeField] private Text throwLoadingText;
        
        [Header("Dice Outputs")][Space(5)]
        [Tooltip("Left Dice")][SerializeField]
        private Image leftDice;
        [Tooltip("Right Dice")][SerializeField]
        private Image rightDice;
        
        [Header("Dice Sprites")][Space(5)]
        [Tooltip("Left Dice Sprites")][SerializeField]
        private Sprite[] leftDiceSprites;
        [Tooltip("Right Dice Sprites")][SerializeField]
        private Sprite[] rightDiceSprites;
        
        private bool doAnimate;
        private int throwingTextState;
        private int leftDiceValue, rightDiceValue;
        
        /// <summary>
        /// Animates the 3D dice and throws the dice
        /// </summary>
        public void AnimateDiceOnThrow()
        {
            throwLoadingText.text = "";
            leftDice.enabled = false;
            rightDice.enabled = false;
            leftDiceValue = Random.Range(1,6);
            rightDiceValue = Random.Range(1,6);
            throwingTextState = 0;
            gameObject.SetActive(true);
            doAnimate = true;
            StartCoroutine(WaitForAnimationEnd(diceAnimationDuration));
            StartCoroutine(AnimateThrowingText());
            StartCoroutine(AnimateUntilEnd(diceAnimationSpeed));
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
            throwLoadingText.text = "";
            leftDice.enabled = true;
            rightDice.enabled = true;
            leftDice.sprite = leftDiceSprites[leftDiceValue - 1];
            rightDice.sprite = rightDiceSprites[rightDiceValue - 1];
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
                    throwLoadingText.text = "TRWA LOSOWANIE";
                    break;
                case 1:
                case 2:
                    throwLoadingText.text += ".";
                    break;
            }
            throwingTextState = (throwingTextState + 1) % 3;
            yield return new WaitForSeconds(0.2f);
            if (doAnimate) StartCoroutine(AnimateThrowingText());
        }
    }
}
