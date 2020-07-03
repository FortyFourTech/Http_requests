using System;
using System.Collections.Generic;
using fastJSON;
using UnityEngine;

namespace API.Responses
{
    public class ApiResponse<T>
    {
        private int _code;
        private string _message;
        private T _result;
        public ApiResponse(string response)
        {
            var dict = JSON.Parse(response) as Dictionary<string, object>;
            var hasError = false;
            object codeObj;
            if (dict.TryGetValue("code", out codeObj))
            {
                int code = Convert.ToInt32(codeObj);
                string errorCode = Enum.GetName(typeof(EErrorCodes), code);
                bool hasErrorCode = errorCode != null;

                if (hasErrorCode)
                {
                    hasError = true;
                    _code = code;
                    _message = (string)dict["message"];
                }
            }

            if (hasError)
            {
                Debug.Log("request returned error:");
                Debug.Log(_message);
            }
            else
            {
                _result = JSON.ToObject<T>(response);
            }
        }

        public string Error { get { return _message; } }

        public T Result { get { return _result; } }
    }

    /// <summary>
    /// List of response codes that should be treated as errors.
    /// </summary>
    enum EErrorCodes
    {
        _401 = 401,
        _402 = 402,
        _404 = 404,
        _413 = 413,
        _422 = 422,
        _501 = 501,
    }
}