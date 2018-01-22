using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interface
{
    public class InterfaceManager : MonoBehaviour
    {
        private void Start() {
            // connection.EventInHome = EventInHome;
        //     connection.EventInHub = EventInHub;
        //     connection.EventLocker = EventLocker;

        //     UnlockDoor = connection.SendHackingDoor;
        //     UnlockIHome = connection.SendHackingIHome;
        //     UnlockLocker = connection.SendHackingLocker;
        }

        /// <summary>
        /// Событие попадание в коридор(открыт лифт)
        /// </summary>
        public void PlayHint(int number) {
            API.NetworkService.PlayHint((HintEnum)number);
        }
        /// </summary>
        // public void PlayHintTwo() {
        //     API.NetworkService.PlayHint(HintEnum.two);
        // }
        // /// </summary>
        // public void PlayHintTwo() {
        //     API.NetworkService.PlayHint(HintEnum.two);
        // }
        // /// </summary>
        // public void PlayHintTwo() {
        //     API.NetworkService.PlayHint(HintEnum.two);
        // }
        // /// </summary>
        // public void PlayHintTwo() {
        //     API.NetworkService.PlayHint(HintEnum.two);
        // }
    }
}
