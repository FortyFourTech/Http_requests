using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using API.Responses;

namespace API
{
    /// <summary>
    /// Service to basic communication with yandex translate service v1.5.
    /// Documentation can be found here: https://tech.yandex.com/translate/
    /// </summary>
    public class ApiService : Dimar.Singleton<ApiService>
    {
        public delegate void ApiRequestCallback<T>(T result, string error = null);

        public static string BASE_URL = "https://translate.yandex.net/api/v1.5/tr.json";

        private static IEnumerator MakeRequest<ResponseType>(
            Request request,
            Dictionary<ERequestParameter, string> parameters = null,
            Dictionary<string, string> headers = null,
            ApiRequestCallback<ResponseType> callback = null)
        {
            if (request.ResponseType != typeof(ResponseType))
            {
                var error = "Expected response type is not matched with request response type";
                Debug.LogError(error);
                callback(default(ResponseType), error);
                yield break;
            }

            UnityWebRequest webRequest = null; // = new UnityWebRequest(request.url()) {
                                               //    method = request.method()
                                               //};

            var requestUrl = request.Url;
            
            var getRequiredParams = request.GetParameters;
            var getParameters = new Dictionary<ERequestParameter, string>();
            if (getRequiredParams != null)
            {
                foreach (var param in getRequiredParams)
                {
                    if (parameters != null && parameters.ContainsKey(param))
                    {
                        getParameters[param] = parameters[param];
                    }
                    else
                    {
                        getParameters[param] = param.defaultValue();
                    }
                }
            }

            if (getParameters.Count > 0)
            {
                var paramString = "?";
                foreach (var param in getParameters)
                    paramString += param.Key.ToString() + "=" + param.Value + "&";

                // remove last '&'
                // if (getParameters.Count >= 2)
                paramString = paramString.Remove(paramString.Length - 1);
                requestUrl += paramString;
            }

            Debug.Log("requestString: " + requestUrl);

            var postRequiredParams = request.PostParameters;
            var postParameters = new Dictionary<ERequestParameter, string>();
            if (postRequiredParams != null)
            {
                foreach (var param in postRequiredParams)
                {
                    if (parameters != null && parameters.ContainsKey(param))
                    {
                        postParameters[param] = parameters[param];
                    }
                    else
                    {
                        postParameters[param] = param.defaultValue();
                    }
                }
            }

            if (postParameters.Count > 0)
            {
                var form = new WWWForm();
                foreach (var param in postParameters)
                    form.AddField(param.Key.ToString(), param.Value);

                webRequest = UnityWebRequest.Post(requestUrl, form);
            }
            else
            {
                webRequest = UnityWebRequest.Get(requestUrl);
            }

            if (headers != null)
            {
                foreach (var header in headers)
                    webRequest.SetRequestHeader(header.Key, header.Value);
            }

            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log(webRequest.error);
                Debug.Log(webRequest.url);

                if (callback != null)
                    callback(default(ResponseType), webRequest.error);
            }
            else
            {
                // Show results as text
                var result = webRequest.downloadHandler.text;
                Debug.Log(result);

                var resultObject = new Responses.ApiResponse<ResponseType>(result);

                if (callback != null)
                {
                    if (resultObject.Error != null)
                        callback(resultObject.Result, resultObject.Error);
                    else
                        callback(resultObject.Result);
                }

                // Or retrieve results as binary data
                // byte[] results = www.downloadHandler.data;
            }
        }

        public void RunRequest(IEnumerator request)
        {
            StartCoroutine(request);
        }

        public static IEnumerator GetLangsListRequest(ApiRequestCallback<LangListResponse> resultHandler = null)
        {
            return MakeRequest(
                Requests.languageList,
                callback: resultHandler
            );
        }
        public static void GetLangsList(ApiRequestCallback<LangListResponse> resultHandler = null)
        {
            if (_instance == null)
            {
                var error = "no service instance to run request";
                Debug.LogError(error);
                resultHandler?.Invoke(default(LangListResponse), error);

                return;
            }

            var request = GetLangsListRequest(resultHandler);

            _instance.RunRequest(request);
        }

        public static IEnumerator DetectLanguageRequest(string text, ApiRequestCallback<DetectLanguageResponse> resultHandler = null)
        {
            return MakeRequest(
                Requests.detectLanguage,
                new Dictionary<ERequestParameter, string> {
                    { ERequestParameter.text, text },
                },
                callback: resultHandler
            );
        }
        public static void DetectLanguage(string text, ApiRequestCallback<DetectLanguageResponse> resultHandler = null)
        {
            if (_instance == null)
            {
                var error = "no service instance to run request";
                Debug.LogError(error);
                resultHandler?.Invoke(default(DetectLanguageResponse), error);

                return;
            }

            var request = DetectLanguageRequest(text, resultHandler);

            _instance.RunRequest(request);
        }

        public static IEnumerator TranslateRequest(string text, ApiRequestCallback<TranslationResponse> resultHandler = null)
        {
            return MakeRequest(
                Requests.translate,
                new Dictionary<ERequestParameter, string> {
                    { ERequestParameter.text, text },
                },
                callback: resultHandler
            );
        }
        public static void Translate(string text, ApiRequestCallback<TranslationResponse> resultHandler = null)
        {
            if (_instance == null)
            {
                var error = "no service instance to run request";
                Debug.LogError(error);
                resultHandler?.Invoke(default(TranslationResponse), error);

                return;
            }

            var request = TranslateRequest(text, resultHandler);

            _instance.RunRequest(request);
        }
    }
}
