using System;
using API.Responses;

namespace API
{
    public struct Request
    {
        private string _url;
        public ERequestParameter[] GetParameters;
        public ERequestParameter[] PostParameters;
        public Type ResponseType;

        public string Url {
            get => _url;
            set => _url = ApiService.BASE_URL + value;
        }

        public Request(string url)
        {
            _url = url;
            GetParameters = null;
            PostParameters = null;
            ResponseType = typeof(DefaultResponse);
        }
    }

    public static class Requests
    {
        /// <summary>
        /// Список поддерживаемых языков.
        /// </summary>
        public static readonly Request languageList = new Request()
        {
            Url = "/getLangs",
            GetParameters = new ERequestParameter[] {
                ERequestParameter.key,
                ERequestParameter.ui,
            },
            ResponseType = typeof(LangListResponse)
        };

        /// <summary>
        /// Определить язык текста.
        /// </summary>
        public static readonly Request detectLanguage = new Request()
        {
            Url = "/detect",
            GetParameters = new ERequestParameter[] {
                ERequestParameter.key,
                ERequestParameter.text,
                ERequestParameter.hint,
            },
            ResponseType = typeof(DetectLanguageResponse)
        };

        /// <summary>
        /// Перевести текст на указанный язык.
        /// </summary>
        /// <returns></returns>
        public static readonly Request translate = new Request()
        {
            Url = "/translate",
            GetParameters = new ERequestParameter[] {
                ERequestParameter.key,
                ERequestParameter.text,
                ERequestParameter.lang,
            },
            ResponseType = typeof(TranslationResponse)
        };
    }
}
