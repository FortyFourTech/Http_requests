using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System;
using System.Collections;
using API;

namespace Testing {
    public class APITests {

        [Test]
        public void TestAuthorization() {
            var enumerator = NetworkService.DoAuthorization((result, error) => {
                Assert.IsTrue(result, error);
            });

            _GoThroughCoroutine(enumerator);
        }

        [Test]
        public void TestPlayHintCall() {
            NetworkService.ApiRequestCallback<PlayHintResponse> responseChecker = (result, error) => {
                Assert.IsNull(error, error);
            };

            var hintValues = Enum.GetValues(typeof(HintEnum)) as HintEnum[];
            foreach (var hint in hintValues) {
                var request = NetworkService.PlayHint(hint, responseChecker);
                _GoThroughCoroutine(request);
            }
        }

        [Test]
        public void TestCurrentHintCall() {
            string expectedHintName = null;
            NetworkService.ApiRequestCallback<CurrentHintResponse> responseChecker = (result, error) => {
                Assert.IsNull(error, error);

                var expectedName = expectedHintName == null ? "none" : expectedHintName ;
                Assert.AreEqual(result.hintName, expectedName, "unexpected hint name");
            };

            var requestCurrent = NetworkService.GetCurrentHint(responseChecker);
            _GoThroughCoroutine(requestCurrent);

            var hintValues = Enum.GetValues(typeof(HintEnum)) as HintEnum[];
            foreach (var hint in hintValues) {
                var playRequest = NetworkService.PlayHint(hint);
                _GoThroughCoroutine(playRequest);

                expectedHintName = HintExtension.requestParameterValue(hint);
                requestCurrent = NetworkService.GetCurrentHint(responseChecker);
                _GoThroughCoroutine(requestCurrent);
            }
        }

        private void _GoThroughCoroutine(IEnumerator enumerator) {
            while (enumerator.MoveNext()) {
                if (enumerator.Current is AsyncOperation)
                    while (!(enumerator.Current as AsyncOperation).isDone) { }

                else if (enumerator.Current is IEnumerator)
                    _GoThroughCoroutine(enumerator.Current as IEnumerator);
            }
        }
    }
}
