﻿using FileUploadApp.Core;
using FileUploadApp.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Tests
{
    [TestClass]
    public class EventGeneratorTests
    {
        public TestContext TestContext { get; set; }

        private IServiceProvider serviceProvider;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new ContainerBuilder();
            serviceProvider = builder.Create();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (serviceProvider is IDisposable d)
            {
                d.Dispose();
            }
        }

        [TestMethod]
        [TestProperty("file.bmp", "Qk2mFQAAAAAAADYAAAAoAAAAJQAAADEAAAABABgAAAAAAHAVAAAAAAAAAAAAAAAAAAAAAAAA////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AP///////////////////////wAAAP///////////////////////////////////////////////////////////////////wAAAAAAAP///////////////////////////////////////////wD///////////////////////8AAAD///////////////////////////////////////////////////////////////8AAAD///////////////////////////////////////////////////8A////////////////////////AAAA////////////////////////////////////////////////////////////AAAAAAAA////////////////////////////////////////////////////AP///////////////////////wAAAP///////////////////////////////////////////////////////////wAAAP///////////////////////////////////////////////////////wD///////////////////////////8AAAD///////////////////////////////////////////////////////8AAAD///////////////////////////////////////////////////////8A////////////////////////////AAAA////////////////////////////////////////////////AAAAAAAA////////////////////////////////////////////////////////////AP///////////////////////////////wAAAP///////////////////////////////////////////wAAAP///////////////////////////////////////////////////////////////wD///////////////////////////////////8AAAD///////////////////////////////////////8AAAD///////////////////////////////////////////////////////////////8A////////////////////////////////////////AAAA////////////////////////////////AAAAAAAA////////////////////////////////////////////////////////////////AP///////////////////////////////////////////wAAAP///////////////////////////wAAAP///////////////////////////////////////////////////////////////////wD///////////////////////////////////////////8AAAD///////////////////////8AAAD///////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////AAAA////////////////AAAA////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////wAAAP///////////wAAAP///////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////8AAAD///////8AAAD///////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////AAAAAAAAAAAA////////////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////////wAAAP///////////////////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////8AAAAAAAAAAAAAAAD///////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////AAAA////////////AAAA////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////wAAAP///////////////wAAAP///////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////8AAAD///////////////////8AAAD///////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////AAAA////////////////////////AAAAAAAA////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////wAAAP///////////////////////////////wAAAP///////////////////////////////////////////////////////////////wD///////////////////////////////////////8AAAD///////////////////////////////////8AAAD///////////////////////////////////////////////////////////////8A////////////////////////////////////////AAAA////////////////////////////////////////AAAA////////////////////////////////////////////////////////////AP///////////////////////////////////wAAAP///////////////////////////////////////////wAAAAAAAP///////////////////////////////////////////////////////wD///////////////////////////////8AAAAAAAD///////////////////////////////////////////////8AAAD///////////////////////////////////////////////////////8A////////////////////////////AAAA////////////////////////////////////////////////////////AAAAAAAA////////////////////////////////////////////////////AP///////////////////////////wAAAP///////////////////////////////////////////////////////////wAAAP///////////////////////////////////////////////////wD///////////////////////////8AAAD///////////////////////////////////////////////////////////////////////////////////////////////////////////////////8A////////////////////////AAAA////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AP///////////////////////wAAAP///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AA==")]
        [TestProperty("file.7z", "N3q8ryccAAQ81Fq2UwoAAAAAAABiAAAAAAAAAArAMpPgF8MKS10AEeB8kmc1Y6EPQJvWipfi7fCxnZqponTsjapRnj6F2hz1Pqq6R9pIu5vImkLdpXTLrvGXjvbLFd + SfE1zFBazIK + 8xQbiANXXOwh5c9bnRSpUtT22FAqOHyNjObfciK1fhuyeNZBUYP15gRCEeGwJn / 67g2i1dySX + TneOodSCUAA2mw2jpo0dtKZClE4mpqF1KgUvyt2CkAjzhni + rhQ6Oksg4SOHMgOeZj7dBTWgndfNIFUfDwBJiIX + ZL3sgKNIq28oVQN7yNDj / dq5YCsPgHeJavkq6 / LSnHrzMclIRcj9SPziQiq3CfPOqGDNgKN75oeBViUPBsMj / r1Ax7hVz / VM4CHMWS0ycC4DrVjuWHUm4wnG8ZK10GilDzih3uyl6RIVIQqjk8Nxvw5SBBUmoVSdOYt99hm9PbT3XIKw79bZ8NeiR2O6X5wi1VG1T8lEsPA800FpMoMHFCeL + 8S92VMup2GRXjEm6m / kdPIFCPsEbTP8mowgF4g1qUdeIyDr8fUJYsK4jGVb1k66eXwXz8ePcbZYpq5Myu6uOCVfTdCkyF3zdTadqZxmzJXcLRoVqKciahPcj0vfuu7cjMMnzc + lVtFWPR9tZGe2W + lGoFaxv3 + ALy4K4ov3VfVlNnGORTGoBAjw / 7lW0BlfC0T1B48YK4lUI39BRQDKrU4PJBpE6stsQjQ6dDSjpdAgLFye7SHmpzCw9K / xnV + pufr1GO / 6RqxP0zQTSfVY8J4bhFoFPzue6BRQUND0dF5UrwSPqX5K / PQWaNt0NjBe4NnreLv3DbLDix + qqYrgfRbkXIVmWfDYXIS / RXAocqoLYF2XUhIkpybNCApsqf4RjebBfh2xEH59wnnmx +/ oid / 4UVzsJZ0PUWPnX3 + wIXSsvxsN / boCkpWz83ltY8bkFKR35Y / LzA8KyrsQzt / 0WL9fPBMBfUGH2k4mNhVBhfIYuziIGxaGtLjSfbg52iIxVreSzNQk32qUuC1f7mHUIcEqPAlI8HLABkeZF852iP1iYP1VFkBTkFa5vQ3t / BGGwiXgoGHSblLpz2ZO1 / ds9Ze2VcSDcUT1PD79nbh / X5Wzv95Z7qf4vX3 + Ssme0LE / zEzukCdvZLnr4uaYHsamePlIR77pRmdg8t + OF4QZ8INSmbhrHDxPnw5JwXDr0cZeV7wrbnfy8LEdYaKQ / jQOyDju8stLRRJqSBQL4cpdj0EM4wBbTeMXzjGHvxxzWC4pM5EShT5 / g + 4rGVKM + FxXIUpuUKFZfGkwlPg + tdrizWKePeuFgtmlRe0mwJmO6jIeQ8T2lf7gZ00nSqYgLcOVI68 / 4VnX7Ss7ndCrA7Ro5HeBGVPBToqtOUkS5yg4cGMXWkPdsJZI71tQGaJ449LmIe6 + JOh6axTCDoVC2Prmyo + aw//9/vMzTwpNCet8fqwmeb31bQtI7HpdiUlhW0gyJLcI8WUCS64Rh8ixgIvF/Jo0CqKUcnz9f+ZScIk2tBOOGp6pEFT91jfizMBtuXnfi8EYrbU1YLoX+SwJWnl+qaibMuf6FLIPGt5WWDLb7NSdx7GiYLb8G10ybM1i1g0tD6FICNf2AdnT97kuGv8vk+0SDXr6obvSyOBZFNhApj3W1m/ZbYI/8e0QP6AKv9lNkeleHF1YGVYUI7vvnaOot8ARHBg8q/P63yG4fF4kJRhJLYdgG3F4e5gdXhoUpFCqheHCuFf2ZHmgK0nT6/bhk+oIn5YQTsdyQx2XF2k9YsKPmv2PNSjT6x97bd1d0Qu+eOZNaOY8N5ee24kJH2yizaBLE6UwtNmN2SH8vUcM6YBkBL0RhZCbIrdCqQOGkvVZuQQffkv0nfkvniqFbJShV0VAy6TlC9c0tCsqnRLV8Vc/6mtSXwkwRIkMGTiuTVV0I22toMf56ZPJF6eHOgcaIUMrcR9kCeoHK+Jx9rEX4yCR2sSyMlrboVN6ZXUohPUMnqXLBibdyLGmI60AlUH4VAovArUJkHkZUXYOGKeFHp5vh0Rtk1GWWtF39OHPOOjSYtwk0IDPC945lLQPCKLWAZB4VqDPPQY0wq26/1LIh/zSOlfz1XgiQk79nXmTE6l/H0fHhkWFt297jH/N9sCcPTNz17TpbgCiquZxnlwkaEjBvRv7z3pZ15XQ5bkeVoNrfnLZEr+ec98tdfR86UKSTiHNiSclYunB5Enke44RZ/iSc8ajPw7KVm8AONGi2K7zb7K3WL+YAjCjPhTsr7LHXHvsbx5Dw0JkCzHonSOm6sJGFeu2JEetXhYk1T3Xw/PU3uXgpXjsUf5K1+JHQQCmg5jDVcWa1y8VVNgbPwvixrleGuD8l0tkHq/SQKE5zQePDWrDd2B7OZqtj0+VboFPyKXJTJMqn0EglnDPks/EvLrne7tI4YbL6qq8GA/olh/fjNlXmdhGQEgaFbHKCTH7MiDLeH4J8fCTtqaaKN8mvmsbGG2wEE9iTC6rjeQGSPRuDGg09RoDSA+35PRgYDrVcpfbzTh2P7lQ+KB0+mNU/+pT/iPv8CDhr3+072naKlHhbs3/nSk+Wlc0d5edqH0dutst6tBHJgYB6yfNZ1DwXG1zC1/1YLiiGXCO7JQhZLkmhctbPI1XgRg1VmRycqky6hgRSiexmqjvVJlZDKWD3u4z0cIv9P6Kpliw8190Nqeui+OoOsWTOWorLr4T1iG3wiqa9WvO93mXvhA95q+Sek5ttztlLmup1zrpvI/jswKXZtkqve29XqXVJnhrqpo9eqZrexZBJKF+w51hfBUFOkVYP5VH8zI3rMwB4oWs9vysfWV8K4ZGAweZ5YPiX7F3Zyf7hNV/1nVic1+N1TgUrg7XYTsxkaV01ryldC8FRtYbQuf67Di2aBGPxIvqVObeGC1uYxf9zS1xk7puLGO+51EoLM4O21i1zjZBWjXf2lEaPK9+xeZD8aYurlv7Xl5EYhgFk/4aisaf4SuXEtUpL5zU3gJXedrt6ALhuNdrlK6B0Fy/lf8tVAhuGG6gyBs83rqZg/6e0gfckuCmJ0pprYuesycRD+Oo7L+J4mWidXeTDrcYo8qQPXfqfXFbuvXvzM8IR4ji0EhOaUPugFUmE0xkFX/Ruw059O1ppeFJXzALh4NQyIXFjvf6pITxvpfjm3tNqbVwaw2ZVV9Rh2qlgocba01dpjbA97D3YEVB/QoOuTifeCYXhF6hRl1mrFEmF82OifCbJqZ4ZbKwEe3VNRYWAX7Okss59xq5pO2RSvfy1rCkOtx8e/JQF0pT/tccez5wJoWpyMuyU3bbZ6xvutAdKOvvu0NY3Hqi8xv3FmXTjamfnXjVxNqmt7bekkePGMFauHl95RzD4bzoPQHxoVNX1sIARpk4Pb2g6j9HDnjvRYhPgIOS82P+rFxQgIrDO0OipvjAzytHRT7bjp42zSObZF2Bazi4D1smPASTtoM2pQeqt4h52iA8gWNdCcSdRe0NqYmgkzZHvZE+Csdeh6/vC28uQf9gzPRl5qML2Fw3tOO49oJCc2if5MLYssooAABBAYAAQmKUwAHCwEAASEhAQEMl8QACAoBo2zM/AAABQEZCgAAAAAAAAAAAAARFwAuAGcAaQB0AGkAZwBuAG8AcgBlAAAAGQQAAAAAFAoBAAtalPipCNUBFQYBACAAAAAAAA==")]
        public async Task Test_EventGeneratorFillUploadsContentWithFiles()
        {
            var data = TestContext.Properties.
                Where(x => x.Key.StartsWith("file."))
                .Select(x => (x.Key, x.Value.ToString()));

            var list = new List<(string, string)>();
            list.AddRange(data);

            var formFiles = GetTwoFiles(list);
            var httpContext = GetMockHttpContext(GetMockFormCollection(formFiles));

            using (var scope = serviceProvider.CreateScope())
            {
                var eventGen = scope.ServiceProvider.GetRequiredService<EventGenerator>();
                await eventGen.GenerateApprochiateEventAsync(httpContext);

                var uploadContent = scope.ServiceProvider.GetRequiredService<UploadsContext>();

                Assert.IsTrue(uploadContent.Count == 2);
            }
        }

        [TestMethod]
        [TestProperty("json", @"{""files"": [{ ""name"": ""1.bmp"", ""data"": ""data:;base64,Qk2mFQAAAAAAADYAAAAoAAAAJQAAADEAAAABABgAAAAAAHAVAAAAAAAAAAAAAAAAAAAAAAAA////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AP///////////////////////wAAAP///////////////////////////////////////////////////////////////////wAAAAAAAP///////////////////////////////////////////wD///////////////////////8AAAD///////////////////////////////////////////////////////////////8AAAD///////////////////////////////////////////////////8A////////////////////////AAAA////////////////////////////////////////////////////////////AAAAAAAA////////////////////////////////////////////////////AP///////////////////////wAAAP///////////////////////////////////////////////////////////wAAAP///////////////////////////////////////////////////////wD///////////////////////////8AAAD///////////////////////////////////////////////////////8AAAD///////////////////////////////////////////////////////8A////////////////////////////AAAA////////////////////////////////////////////////AAAAAAAA////////////////////////////////////////////////////////////AP///////////////////////////////wAAAP///////////////////////////////////////////wAAAP///////////////////////////////////////////////////////////////wD///////////////////////////////////8AAAD///////////////////////////////////////8AAAD///////////////////////////////////////////////////////////////8A////////////////////////////////////////AAAA////////////////////////////////AAAAAAAA////////////////////////////////////////////////////////////////AP///////////////////////////////////////////wAAAP///////////////////////////wAAAP///////////////////////////////////////////////////////////////////wD///////////////////////////////////////////8AAAD///////////////////////8AAAD///////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////AAAA////////////////AAAA////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////wAAAP///////////wAAAP///////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////8AAAD///////8AAAD///////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////AAAAAAAAAAAA////////////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////////wAAAP///////////////////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////8AAAAAAAAAAAAAAAD///////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////AAAA////////////AAAA////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////wAAAP///////////////wAAAP///////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////8AAAD///////////////////8AAAD///////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////AAAA////////////////////////AAAAAAAA////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////wAAAP///////////////////////////////wAAAP///////////////////////////////////////////////////////////////wD///////////////////////////////////////8AAAD///////////////////////////////////8AAAD///////////////////////////////////////////////////////////////8A////////////////////////////////////////AAAA////////////////////////////////////////AAAA////////////////////////////////////////////////////////////AP///////////////////////////////////wAAAP///////////////////////////////////////////wAAAAAAAP///////////////////////////////////////////////////////wD///////////////////////////////8AAAAAAAD///////////////////////////////////////////////8AAAD///////////////////////////////////////////////////////8A////////////////////////////AAAA////////////////////////////////////////////////////////AAAAAAAA////////////////////////////////////////////////////AP///////////////////////////wAAAP///////////////////////////////////////////////////////////wAAAP///////////////////////////////////////////////////wD///////////////////////////8AAAD///////////////////////////////////////////////////////////////////////////////////////////////////////////////////8A////////////////////////AAAA////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AP///////////////////////wAAAP///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AA==""}]}")]
        public async Task Test_EventGeneratorFillUploadsContentWithFiles_2()
        {
            var json = TestContext.Properties["json"].ToString();
            var httpContext = GetMockHttpContext(System.Text.Encoding.UTF8.GetBytes(json));

            using (var scope = serviceProvider.CreateScope())
            {
                var eventGen = scope.ServiceProvider.GetRequiredService<EventGenerator>();
                await eventGen.GenerateApprochiateEventAsync(httpContext);

                var uploadContent = scope.ServiceProvider.GetRequiredService<UploadsContext>();

                Assert.IsTrue(uploadContent.Count == 1);
            }
        }

        [TestMethod]
        [TestProperty("json", @"{""links"": [ ""https://icdn.lenta.ru/images/2019/05/17/15/20190517152225608/top7_1674364358c174639f4563c3bf8a373f.jpg"" ]}")]
        public async Task Test_EventGeneratorFillUploadsContentWithFiles_3()
        {
            var json = TestContext.Properties["json"].ToString();
            var httpContext = GetMockHttpContext(System.Text.Encoding.UTF8.GetBytes(json));

            using (var scope = serviceProvider.CreateScope())
            {
                var eventGen = scope.ServiceProvider.GetRequiredService<EventGenerator>();
                await eventGen.GenerateApprochiateEventAsync(httpContext);

                var uploadContent = scope.ServiceProvider.GetRequiredService<UploadsContext>();

                Assert.IsTrue(uploadContent.Count == 1);
            }
        }

        #region Mock

        private HttpContext GetMockHttpContext(byte[] body)
        {
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(h => h.Request.Body)
                .Returns(new MemoryStream(body));

            httpContext.Setup(h => h.Request.ContentType).Returns("application/json");
            httpContext.Setup(h => h.Request.HasFormContentType).Returns(false);
            httpContext.Setup(x => x.RequestServices).Returns(serviceProvider);

            return httpContext.Object;
        }

        private static HttpContext GetMockHttpContext(IFormCollection formCollection)
        {
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(h => h.Request.ReadFormAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(formCollection));
            httpContext.Setup(h => h.Request.HasFormContentType).Returns(true);
            return httpContext.Object;
        }

        private static FormFileCollection GetTwoFiles(IEnumerable<(string, string)> fakes)
        {
            var formFiles = new FormFileCollection();
            formFiles.AddRange(fakes.Select(x => GetFakeFile(x.Item1, x.Item2)));

            return formFiles;
        }

        private static IFormCollection GetMockFormCollection(FormFileCollection formFiles)
        {
            var formCollection = new Mock<IFormCollection>();
            formCollection.Setup(f => f.Files).Returns(formFiles);

            return formCollection.Object;
        }

        static readonly Dictionary<string, string> mimes
            = new Dictionary<string, string>
            {
                [".bmp"] = Services.MimeConstants.BitmapMime,
                [".7z"] = Services.MimeConstants.SevenZipMime
            };

        private static IFormFile GetFakeFile(string filename, string base64)
        {
            const string modelName = "file";
            var ms = new MemoryStream(Convert.FromBase64String(base64));

            return new FormFile(ms, 0, ms.Length, modelName, filename)
            {
                Headers = new HeaderDictionary(new Dictionary<string, StringValues>
                {
                    ["Content-Disposition"] = $"form-data; name=\"{modelName}\"; filename=\"{filename}\"",
                    ["Content-Type"] = mimes[Path.GetExtension(filename)]
                })
            };
        }

        #endregion
    }
}
