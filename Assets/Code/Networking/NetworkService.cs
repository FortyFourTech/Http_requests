using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using System;
using fastJSON;
using API.Responses;

namespace API {
    public static class NetworkService {
        public delegate void ApiRequestCallback<T>(T result, string error = null);

        public static string BASE_URL = "https://translate.yandex.net/api/v1.5/tr.json";

        private static IEnumerator MakeRequest<ResponseType>(
            Request request,
            Dictionary<RequestParameter, string> parameters = null,
            Dictionary<string, string> headers = null,
            ApiRequestCallback<ResponseType> callback = null) {

            UnityWebRequest webRequest = null; // = new UnityWebRequest(request.url()) {
                                               //    method = request.method()
                                               //};

            var getRequiredParams = request.getParameters();
            var getParameters = new Dictionary<RequestParameter, string>();
            foreach (var param in getRequiredParams) {
                if (parameters != null && parameters.ContainsKey(param)) {
                    getParameters[param] = parameters[param];
                } else {
                    getParameters[param] = param.defaultValue();
                }
            }
            var paramString = "";

            if (getParameters.Count > 0) {
                paramString = "?";
                foreach (var param in getParameters)
                    paramString += param.Key.ToString() + "=" + param.Value + "&";

                // remove last '&'
                // if (getParameters.Count >= 2)
                    paramString = paramString.Remove(paramString.Length - 1);
            }

            var postRequiredParams = request.postParameters();
            var postParameters = new Dictionary<RequestParameter, string>();
            foreach (var param in postRequiredParams) {
                if (parameters != null && parameters.ContainsKey(param)) {
                    postParameters[param] = parameters[param];
                } else {
                    postParameters[param] = param.defaultValue();
                }
            }

            var requestUrl = request.url() + paramString;
            Debug.Log("requestString: " + requestUrl);

            if (postParameters.Count > 0) {
                var form = new WWWForm();
                foreach (var param in postParameters)
                    form.AddField(param.Key.ToString(), param.Value);

                webRequest = UnityWebRequest.Post(requestUrl, form);
            } else {
                webRequest = UnityWebRequest.Get(requestUrl);
            }

            if (headers != null) {
                foreach (var header in headers)
                    webRequest.SetRequestHeader(header.Key, header.Value);
            }

            yield return webRequest.Send();

            if (webRequest.isNetworkError) {
                Debug.Log(webRequest.error);
                Debug.Log(webRequest.url);

                if (callback != null)
                    callback(default(ResponseType), webRequest.error);
            } else {
                // Show results as text
                var result = webRequest.downloadHandler.text;
                Debug.Log(result);

                var resultObject = new Responses.ResponseBase<ResponseType>(result);

                if (callback != null) {
                    if (resultObject.Error != null)
                        callback(resultObject.Result, resultObject.Error);
                    else
                        callback(resultObject.Result);
                }

                // Or retrieve results as binary data
                //byte[] results = www.downloadHandler.data;
            }
        }

        public static IEnumerator GetLangsList(ApiRequestCallback<LangListResponse> resultHandler = null) {
            return MakeRequest(
                Request.languageList,
                callback: resultHandler
            );
        }
        
        public static IEnumerator DetectLanguage(string text, ApiRequestCallback<DetectLanguageResponse> resultHandler = null) {
            return MakeRequest(
                Request.detectLanguage,
                new Dictionary<RequestParameter, string> {
                    { RequestParameter.text, text },
                },
                callback: resultHandler
            );
        }

        public static IEnumerator Translate(string text, ApiRequestCallback<TranslationResponse> resultHandler = null) {
            return MakeRequest(
                Request.translate,
                new Dictionary<RequestParameter, string> {
                    { RequestParameter.text, text },
                },
                callback: resultHandler
            );
        }
    }
}
