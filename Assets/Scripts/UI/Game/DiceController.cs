using DataStorage;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static Board.States.GameState;

namespace UI.Game
{
    /// <summary>
    /// Class controls 3D UI Dice on loading
    /// </summary>
    public class DiceController : MonoBehaviour
    {
        [Header("Dice Animation Settings")][Space(5)]
        [Tooltip("Dice Animation Duration")][SerializeField]
        private float diceAnimationDuration;
        [Tooltip("Dice Animation Speed")][SerializeField] 
        private float diceAnimationSpeed;
        [Tooltip("Dice Throw Start Delay")][SerializeField] 
        private float diceThrowStartDelay;
        [Tooltip("Dice Animation Speed")][SerializeField] 
        private Text throwLoadingText;

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
        
        [Tooltip("Dice Model")][SerializeField]
        private GameObject[] diceModel;
        
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
            
            do
            {
                leftDiceValue = Random.Range(1, 6);
                rightDiceValue = Random.Range(1, 6);
            } while (leftDiceValue + rightDiceValue == 7 && !GameManager.ThiefActive);

            throwingTextState = 0;
            foreach(var dice in diceModel)
                dice.SetActive(true);
            doAnimate = true;

            StartCoroutine(RunAnimation());
        }

        public void HideDicesOutputs()
        {
            leftDice.enabled = false;
            rightDice.enabled = false;
            leftDiceValue = 0;
            rightDiceValue = 0;
        }

        public void ShowDicesWithoutAnimation(int left, int right)
        {
            throwLoadingText.text = "WYLOSOWANO:";
            leftDice.enabled = true;
            rightDice.enabled = true;
            leftDice.sprite = leftDiceSprites[left - 1];
            rightDice.sprite = rightDiceSprites[right - 1];
        }

        IEnumerator RunAnimation()
        {
            yield return new WaitForSeconds(diceThrowStartDelay);
            foreach(var dice in diceModel)
                dice.SetActive(true);
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
            throwLoadingText.text = "WYLOSOWANO:";
            leftDice.enabled = true;
            rightDice.enabled = true;
            leftDice.sprite = leftDiceSprites[leftDiceValue - 1];
            rightDice.sprite = rightDiceSprites[rightDiceValue - 1];
            GameManager.State.CurrentDiceThrownNumber = leftDiceValue + rightDiceValue;
            GameManager.State.LeftDice = leftDiceValue;
            GameManager.State.RightDice = rightDiceValue;
            foreach(var dice in diceModel)
                dice.SetActive(false);

            GameManager.State.MovingUserMode = MovingMode.Normal;
            GameManager.HandleThrowingDices();
        }

        /// <summary>
        /// Rotates (animates) the dice
        /// </summary>
        /// <param name="speed">Speed of the animation rotation</param>
        /// <returns></returns>
        IEnumerator AnimateUntilEnd(float speed)
        {
            for(var i = 0; i < diceModel.Length; i++)
            {
                int sign;
                if (i % 2 == 0) sign = -1;
                else sign = 1;
                diceModel[i].gameObject.transform.Rotate(Vector3.up * (sign * (speed * Time.deltaTime)));
                diceModel[i].gameObject.transform.Rotate(Vector3.right * (sign * (speed * Time.deltaTime)));
                diceModel[i].gameObject.transform.Rotate(Vector3.forward * (sign * (speed * Time.deltaTime)));
            }

            yield return new WaitForSeconds(0);
            if (doAnimate)
            {
                StartCoroutine(AnimateUntilEnd(speed));
            }
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
            if (doAnimate)
            {
                StartCoroutine(AnimateThrowingText());
            }
        }
    }
}
