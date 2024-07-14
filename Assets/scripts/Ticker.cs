using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class Ticker : MonoBehaviour
    {
        public static float ticktime = 0.02f; // Time in seconds between each tick.
        private float tickTimer;



        public delegate void TickAction();
        public static event TickAction OnTickAction;


        private void Update()
        {
            tickTimer += Time.deltaTime;
            if (tickTimer >= ticktime)
            {
                tickTimer = 0;
                TickEvent();
            }
        }

        private void TickEvent()
        {
            OnTickAction?.Invoke();
        }

    }

