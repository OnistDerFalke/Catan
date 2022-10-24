using System;
using System.Collections;
using Assets.Scripts.Board.States;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Board.States.FieldState;
using static Player.Resources;

namespace Board
{
    public class FieldElement : BoardElement
    {
        //Destiny: List of all neighbour junctions to the field (only for fields elements)
        public List<int> junctionsID;

        //Destiny: Thief figure that shows over the field when there is a thief on it
        [Tooltip("Thief figure")][SerializeField]
        public GameObject thiefFigure;
        
        //Destiny: Resource particle that shows over the field
        [Tooltip("Resource Particle")][SerializeField]
        public GameObject resourceParticle;
        
        [Tooltip("Resource Particles")][SerializeField]
        private GameObject[] resourceParticles;

        //Destiny: The type of the field
        [Header("Type of the field")][Space(5)]
        [Tooltip("Type of the field")][SerializeField]
        public FieldType type;

        public FieldElement()
        {
            State = new FieldState();
        }

        public void SetState(FieldState state)
        {
            ((FieldState)State).id = state.id;
            ((FieldState)State).isThief = state.isThief;
            ((FieldState)State).number = state.number;
            ((FieldState)State).type = state.type;
            type = state.type;
        }

        /// <summary>
        /// Setting number over the field
        /// </summary>
        /// <param name="n">Number over the field to set</param>
        public void SetNumberAndApply(int n)
        {
            ((FieldState)State).number = n;

            if(!((FieldState)State).isThief)
            {
                transform.GetComponent<NumberOverField.NumberOverField>().SetNumberValue(((FieldState)State).number);
            }
            else
            {
                transform.GetComponent<NumberOverField.NumberOverField>().SetNumberValue(0);
            }
        }

        /// <summary>
        /// Sets new value of the variable that represents the presence of a thief
        /// </summary>
        /// <param name="isThief">new value of the presence of a thief</param>
        public void SetThief(bool isThief)
        {
            ((FieldState)State).isThief = isThief;
            SetNumberAndApply(((FieldState)State).number);
        }

        /// <summary>
        /// Runs particle effect over the field for a specified time
        /// </summary>
        public void ParticleAnimation()
        {
            resourceParticle.SetActive(true);
            StartCoroutine(WaitForParticleEnds());
        }

        public void ParticleDesertAnimation(FieldType fieldType)
        {
            foreach (var elem in resourceParticles)
                elem.SetActive(false);
            switch(fieldType)
            {
                case FieldType.Forest:
                    resourceParticles[0].SetActive(true);
                    break;
                case FieldType.Pasture:
                    resourceParticles[1].SetActive(true);
                    break;
                case FieldType.Field:
                    resourceParticles[2].SetActive(true);
                    break;
                case FieldType.Hills:
                    resourceParticles[3].SetActive(true);
                    break;
                case FieldType.Mountains:
                    resourceParticles[4].SetActive(true);
                    break;
                case FieldType.Desert:
                    Debug.LogError("Desert does now own any individual particle!");
                    break;
            }
            StartCoroutine(WaitForParticleEnds());
        }

        /// <summary>
        /// Waits for the end of the particle effect and disables it
        /// </summary>
        private IEnumerator WaitForParticleEnds()
        {
            yield return new WaitForSeconds(4f);
            resourceParticle.SetActive(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldType"></param>
        /// <returns>Type of resource from the given field</returns>
        public ResourceType GetResourceType()
        {
            return ((FieldState)State).type switch
            {
                FieldType.Forest => ResourceType.Wood,
                FieldType.Hills => ResourceType.Clay,
                FieldType.Pasture => ResourceType.Wool,
                FieldType.Mountains => ResourceType.Iron,
                FieldType.Field => ResourceType.Wheat,
                _ => 0
            };
        }

        void Awake()
        {
            elementType = ElementType.Field;
            ((FieldState)State).isThief = false;
        }

        void Update()
        {
            //Destiny: If thief is on the field, activate the figure
            thiefFigure.SetActive(((FieldState)State).isThief);
        }
    }
}