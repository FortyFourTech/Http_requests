using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interface {
    public class InterfaceManager : MonoBehaviour {
        public void LanguageList() {
            var request = API.NetworkService.GetLangsList((result, error) => {
                Debug.Log(result.dirs.ToString());
                Debug.Log(result.langs.ToString());
            });

            StartCoroutine(request);
        }

        public void DetectLang(string text) {
            var request = API.NetworkService.DetectLanguage(text, (result, error) => {
                Debug.Log(result.lang.ToString());
            });

            StartCoroutine(request);
        }

        public void Translate(string text) {
            var request = API.NetworkService.Translate(text, (result, error) => {
                Debug.Log(result.text.ToString());
            });
            
            StartCoroutine(request);
        }
    }
}
