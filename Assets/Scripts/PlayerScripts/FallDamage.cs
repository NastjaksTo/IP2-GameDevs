using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using StarterAssets;
using static UiScreenManager;
using static PlayerAttributes;

namespace Invector.CharacterController
{
    public class FallDamage : MonoBehaviour
    {
        public float startYPos;
        public float endYPos;
        public float damageThreshold;
        public bool damaged = false;
        public bool firstCall = true;
        public float extraDamageMultiplier = 0f;
        
        // Update is called once per frame
        void Update ()
        {
            if (!transform.GetComponent<ThirdPersonController> ().Grounded) 
            {
                if (transform.position.y > startYPos)
                {
                    firstCall = true;
                }
                if (firstCall) 
                {
                    startYPos = transform.position.y;
                    firstCall = false;
                    damaged = true;
                }
            }
            if (transform.GetComponent<ThirdPersonController> ().Grounded) 
            {
                endYPos = transform.position.y;
                if (startYPos - endYPos > damageThreshold) 
                {
                    if (damaged) 
                    {
                        FallDamaged (startYPos - endYPos - damageThreshold);
                        damaged = false;
                        firstCall = true;
                    }
                }
            }
        }



        public void FallDamaged (float damageAmount)
        {
            /***** Add extra damage for more realism from heights*****/
            if (extraDamageMultiplier > 0f) 
            {
                var damage = damageAmount * extraDamageMultiplier;
                playerAttributesScript.currentHealth -= damage;
                playerAttributesScript.currentHealth = Mathf.Round(playerAttributesScript.currentHealth);
                if(playerAttributesScript.currentHealth <= 0) uiScreenManager.OpenDeathUi();
            } else 
            {
                var damage = damageAmount;
                playerAttributesScript.currentHealth -= damage;
                playerAttributesScript.currentHealth = Mathf.Round(playerAttributesScript.currentHealth);
                if(playerAttributesScript.currentHealth <= 0) uiScreenManager.OpenDeathUi();
            }
        }
    }
}