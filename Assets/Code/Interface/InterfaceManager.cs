using UnityEngine;

namespace Interface
{
    public class InterfaceManager : MonoBehaviour
    {
        public void LanguageList()
        {
            API.ApiService.GetLangsList((result, error) => {
                Debug.Log(result.dirs.ToString());
                Debug.Log(result.langs.ToString());
            });
        }

        public void DetectLang(string text)
        {
            API.ApiService.DetectLanguage(text, (result, error) => {
                Debug.Log(result.lang.ToString());
            });
        }

        public void Translate(string text)
        {
            API.ApiService.Translate(text, (result, error) => {
                Debug.Log(result.text.ToString());
            });
        }
    }
}
