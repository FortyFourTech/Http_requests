using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using System;

namespace API {
    public static class NetworkService {
        private delegate void NetRequestCallback(string result, string error = null);

        public static string BASE_URL = "http://192.168.1.32/";

        private static string kHeaderNameCookieSet = "Set-Cookie";
        private static string kHeaderNameCookie = "Cookie";
        private static string kHeaderNameLocation = "Location";
        private static string kCookieNameSession = "session";

        private static string sessionCookie;

        private static IEnumerator MakeRequest(
            Request request,
            Dictionary<string, string> parameters = null,
            Dictionary<string, string> headers = null,
            NetRequestCallback callback = null) {

            UnityWebRequest webRequest = null; // = new UnityWebRequest(request.url()) {
                                               //    method = request.method()
                                               //};

            if (parameters != null) {
                if (request.method() == UnityWebRequest.kHttpVerbPOST) {
                    var form = new WWWForm();
                    foreach (var param in parameters)
                        form.AddField(param.Key, param.Value);

                    webRequest = UnityWebRequest.Post(request.url(), form);
                } else if (request.method() == UnityWebRequest.kHttpVerbGET) {
                    var paramString = "?";
                    foreach (var param in parameters)
                        paramString += param.Key + "=" + param.Value + "&";

                    // remove last '&'
                    if (parameters.Count >= 2)
                        paramString = paramString.Remove(paramString.Length - 2);

                    webRequest = UnityWebRequest.Get(request.url() + paramString);
                }
            } else {
                webRequest = UnityWebRequest.Get(request.url());
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
                    callback(null, webRequest.error);
            } else {
                // Show results as text
                var result = webRequest.downloadHandler.text;
                Debug.Log(result);

                if (callback != null)
                    callback(result);

                // Or retrieve results as binary data
                //byte[] results = www.downloadHandler.data;
            }
        }

        public delegate void AuthorizationCallback(bool success, string error = null);
        // very special case
        public static IEnumerator DoAuthorization(AuthorizationCallback cb = null) {
            var loginParams = new Dictionary<string, string> {
                {"user", "device" },
                {"pass", "qwerty" },
            };

            var loginForm = new WWWForm();
            foreach (var param in loginParams)
                loginForm.AddField(param.Key, param.Value);

            var webRequest = UnityWebRequest.Post(Request.authorize.url(), loginForm);

            webRequest.redirectLimit = 0;

            yield return webRequest.Send();

            bool success = false;
            string error = null;
            // here must be error because of the redirect limit
            // this is desired path
            if (webRequest.isNetworkError) {
                var responseHeaders = webRequest.GetResponseHeaders();

                var successCondition = responseHeaders.Any(x => x.Key == kHeaderNameLocation && x.Value.StartsWith("/index.htm")) &&
                    responseHeaders.Any(x => x.Key == kHeaderNameCookieSet);

                var failCondition = responseHeaders.Any(x => x.Key == kHeaderNameLocation && x.Value.StartsWith("/auth.htm"));

                if (successCondition) {
                    var cookiesString = responseHeaders.First(x => x.Key == kHeaderNameCookieSet).Value;
                    var cookiesArray = cookiesString.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries);
                    var cookiesDict = cookiesArray.ToDictionary(
                        item => item.Split('=')[0],
                        item => item.Split('=')[1]
                    );

                    var sessionId = cookiesDict[kCookieNameSession];
                    sessionCookie = sessionId;

                   success = true;
                } else if (failCondition) {
                    // unauthorized
                    error = "authorization failed";
                } else {
                    // unknown error
                    error = "unknown authorization error";
                }
            } else {
                // unknown error
                error = "unknown authorization error";

                // Show results as text
                var result = webRequest.downloadHandler.text;
                Debug.Log(result);

                //var resultObject = JsonUtility.FromJson<>(result);
            }

            if (cb != null) cb(success, error);
        }

        public delegate void ApiRequestCallback<T>(T result, string error);
        public static IEnumerator GetCurrentHint(ApiRequestCallback<CurrentHintResponse> resultHandler = null) {
            return MakeRequest(
                Request.getCurrent,
                headers: new Dictionary<string, string> {
                    { kHeaderNameCookie, kCookieNameSession + "=" + sessionCookie },
                },
                callback: (response, error) => {
                    if (resultHandler != null) {
                        var resultObject = JsonUtility.FromJson<CurrentHintResponse>(response);
                        resultHandler(resultObject, error);
                    }
                }
            );
        }
        
        public static IEnumerator PlayHint(HintEnum hint, ApiRequestCallback<PlayHintResponse> resultHandler = null) {
            return MakeRequest(
                Request.playSound,
                new Dictionary<string, string> {
                    { "hint", hint.requestParameterValue() },
                },
                new Dictionary<string, string> {
                    { kHeaderNameCookie, kCookieNameSession + "=" + sessionCookie },
                },
                callback: (response, error) => {
                    if (resultHandler != null) {
                        var resultObject = JsonUtility.FromJson<PlayHintResponse>(response);
                        resultHandler(resultObject, error);
                    }
                }
            );
        }
    }
}
