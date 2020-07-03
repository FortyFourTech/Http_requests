using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System;
using System.Collections;
using API;

namespace Testing
{
    public class APITests
    {
        [Test]
        public void TestLangList()
        {
            var enumerator = ApiService.GetLangsListRequest((result, error) =>
            {
                Assert.IsNull(error, error);
            });

            _GoThroughCoroutine(enumerator);
        }

        [Test]
        public void TestDetection()
        {
            var enumerator = ApiService.DetectLanguageRequest("hello", (result, error) =>
            {
                Assert.IsNull(error, error);
                Assert.AreEqual("en", result.lang, "wrong lang");
            });

            _GoThroughCoroutine(enumerator);

            enumerator = ApiService.DetectLanguageRequest("привет", (result, error) =>
            {
                Assert.IsNull(error, error);
                Assert.AreEqual("ru", result.lang, "wrong lang");
            });

            _GoThroughCoroutine(enumerator);
        }

        [Test]
        public void TestTranslate()
        {
            var enumerator = ApiService.TranslateRequest("привет", (result, error) =>
            {
                Assert.IsNull(error, error);
                Assert.AreEqual("hi", result.text[0], "wrong translation");
            });

            _GoThroughCoroutine(enumerator);
        }

        private void _GoThroughCoroutine(IEnumerator enumerator)
        {
            while (enumerator.MoveNext())
            {
                if (enumerator.Current is AsyncOperation)
                    while (!(enumerator.Current as AsyncOperation).isDone) { }

                else if (enumerator.Current is IEnumerator)
                    _GoThroughCoroutine(enumerator.Current as IEnumerator);
            }
        }
    }
}
