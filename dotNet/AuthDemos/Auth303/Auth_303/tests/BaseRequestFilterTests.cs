using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Auth_303.helpers;
using Funq;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Host;
using ServiceStack.Text;

namespace Auth_303.tests
{
    
    [TestFixture]
    public abstract class RequestFiltersTests
    {
        protected RequestFiltersAppHostHttpListener AppHost;

        [TestFixtureSetUp]
        public void OnTestFixtureSetUp()
        {
            AppHost = new RequestFiltersAppHostHttpListener();
            AppHost.Init();
            AppHost.Start(SystemConstants.ListeningOn);
        }

        [TestFixtureTearDown]
        public void OnTestFixtureTearDown()
        {
            AppHost.Dispose();
        }

        protected abstract IServiceClient CreateNewServiceClient();
        protected abstract IRestClientAsync CreateNewRestClientAsync();

        protected virtual string GetFormat()
        {
            return null;
        }

        private static void Assert401(IServiceClient client, WebServiceException ex)
        {
            if (client is Soap11ServiceClient || client is Soap12ServiceClient)
            {
                if (ex.StatusCode != 401)
                {
                    Console.WriteLine("WARNING: SOAP clients returning 500 instead of 401");
                }
                return;
            }

            Console.WriteLine(ex);
            Assert.That(ex.StatusCode, Is.EqualTo(401));
        }

        private static void FailOnAsyncError<T>(T response, Exception ex)
        {
            Assert.Fail(ex.Message);
        }

        private static bool Assert401(Exception ex)
        {
            var webEx = (WebServiceException)ex;
            Assert.That(webEx.StatusCode, Is.EqualTo(401));
            return true;
        }

        [Test]
        public void Can_login_with_Basic_auth_to_access_Secure_service()
        {
            var format = GetFormat();
            if (format == null) return;

            var req = (HttpWebRequest)WebRequest.Create(
                string.Format("{0}{1}/reply/Secure", SystemConstants.ListeningOn, format));

            req.Headers[HttpHeaders.Authorization]
                = "basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(SystemConstants.AllowedUser + ":" + SystemConstants.AllowedPass));


            var dtoString = new StreamReader(req.GetResponse().GetResponseStream()).ReadToEnd();
            Assert.That(dtoString.Contains("Confidential"));
            Console.WriteLine(dtoString);
        }

        [Test]
        public void Can_login_with_Basic_auth_to_access_Secure_service_using_ServiceClient()
        {
            var format = GetFormat();
            if (format == null) return;

            var client = CreateNewServiceClient();
            client.SetCredentials(SystemConstants.AllowedUser, SystemConstants.AllowedPass);

            var response = client.Get<SecureResponse>(new Secure());

            Assert.That(response.Result, Is.EqualTo("Confidential"));
        }

        [Test]
        public async Task Can_login_with_Basic_auth_to_access_Secure_service_using_RestClientAsync()
        {
            var format = GetFormat();
            if (format == null) return;

            var client = CreateNewRestClientAsync();
            client.SetCredentials(SystemConstants.AllowedUser, SystemConstants.AllowedPass);

            var response = await client.GetAsync<SecureResponse>(SystemConstants.ServiceClientBaseUri + "secure");

            Assert.That(response.Result, Is.EqualTo("Confidential"));
        }

        [Test]
        public void Can_login_without_authorization_to_access_Insecure_service()
        {
            var format = GetFormat();
            if (format == null) return;

            var req = (HttpWebRequest)WebRequest.Create(
                string.Format("{0}{1}/reply/Insecure", SystemConstants.ServiceClientBaseUri, format));

            req.Headers[HttpHeaders.Authorization]
                = "basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(SystemConstants.AllowedUser + ":" + SystemConstants.AllowedPass));

            var dtoString = new StreamReader(req.GetResponse().GetResponseStream()).ReadToEnd();
            Assert.That(dtoString.Contains("Public"));
            Console.WriteLine(dtoString);
        }

        [Test]
        public void Can_login_without_authorization_to_access_Insecure_service_using_ServiceClient()
        {
            var format = GetFormat();
            if (format == null) return;

            var client = CreateNewServiceClient();

            var response = client.Send<InsecureResponse>(new Insecure());

            Assert.That(response.Result, Is.EqualTo("Public"));
        }

        [Test]
        public async Task Can_login_without_authorization_to_access_Insecure_service_using_RestClientAsync()
        {
            var format = GetFormat();
            if (format == null) return;

            var client = CreateNewRestClientAsync();

            var response = await client.GetAsync<InsecureResponse>(SystemConstants.ServiceClientBaseUri + "insecure");

            Assert.That(response.Result, Is.EqualTo("Public"));
        }

        [Test]
        public void Can_login_with_session_cookie_to_access_Secure_service()
        {
            var format = GetFormat();
            if (format == null) return;

            var req = (HttpWebRequest)WebRequest.Create(
                string.Format("{0}{1}/reply/Secure", SystemConstants.ListeningOn, format));

            req.Headers[HttpHeaders.Authorization]
                = "basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(SystemConstants.AllowedUser + ":" + SystemConstants.AllowedPass));

            var res = (HttpWebResponse)req.GetResponse();
            var cookie = res.Cookies["ss-session"];
            if (cookie != null)
            {
                req = (HttpWebRequest)WebRequest.Create(
                    string.Format("{0}{1}/reply/Secure", SystemConstants.ListeningOn, format));
                req.CookieContainer.Add(new Cookie("ss-session", cookie.Value));

                var dtoString = new StreamReader(req.GetResponse().GetResponseStream()).ReadToEnd();
                Assert.That(dtoString.Contains("Confidential"));
                Console.WriteLine(dtoString);
            }
        }

        [Test]
        public void Get_401_When_accessing_Secure_using_fake_sessionid_cookie()
        {
            var format = GetFormat();
            if (format == null) return;

            var req = (HttpWebRequest)WebRequest.Create(
                string.Format("{0}{1}/reply/Secure", SystemConstants.ListeningOn, format));

            req.CookieContainer = new CookieContainer();
            req.CookieContainer.Add(new Cookie("ss-session", SystemConstants.AllowedUser + "/" + Guid.NewGuid().ToString("N"), "/", "localhost"));

            try
            {
                var res = req.GetResponse();
            }
            catch (WebException x)
            {
                Assert.That(((HttpWebResponse)x.Response).StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            }
        }

        [Test]
        public void Get_401_When_accessing_Secure_using_ServiceClient_without_Authorization()
        {
            var client = CreateNewServiceClient();

            try
            {
                var response = client.Send<SecureResponse>(new Secure());
                Console.WriteLine(response.Dump());
            }
            catch (WebServiceException ex)
            {
                Assert401(client, ex);
                return;
            }
            Assert.Fail("Should throw WebServiceException.StatusCode == 401");
        }

        [Test]
        public async Task Get_401_When_accessing_Secure_using_RestClient_GET_without_Authorization()
        {
            var client = CreateNewRestClientAsync();
            if (client == null) return;

            try
            {
                await client.GetAsync<SecureResponse>(SystemConstants.ServiceClientBaseUri + "secure");
                Assert.Fail("Should throw WebServiceException.StatusCode == 401");
            }
            catch (WebServiceException webEx)
            {
                Assert401(webEx);
                Assert.That(webEx.ResponseDto, Is.Null);
            }
        }

        [Test]
        public async Task Get_401_When_accessing_Secure_using_RestClient_DELETE_without_Authorization()
        {
            var client = CreateNewRestClientAsync();
            if (client == null) return;

            try
            {
                await client.DeleteAsync<SecureResponse>(SystemConstants.ServiceClientBaseUri + "secure");
                Assert.Fail("Should throw WebServiceException.StatusCode == 401");
            }
            catch (WebServiceException webEx)
            {
                Assert401(webEx);
                Assert.That(webEx.ResponseDto, Is.Null);
            }
        }

        [Test]
        public async Task Get_401_When_accessing_Secure_using_RestClient_POST_without_Authorization()
        {
            var client = CreateNewRestClientAsync();
            if (client == null) return;

            try
            {
                await client.PostAsync<SecureResponse>(SystemConstants.ServiceClientBaseUri + "secure", new Secure());
                Assert.Fail("Should throw WebServiceException.StatusCode == 401");
            }
            catch (WebServiceException webEx)
            {
                Assert401(webEx);
                Assert.That(webEx.ResponseDto, Is.Null);
            }
        }

        [Test]
        public async Task Get_401_When_accessing_Secure_using_RestClient_PUT_without_Authorization()
        {
            var client = CreateNewRestClientAsync();
            if (client == null) return;

            try
            {
                await client.PutAsync<SecureResponse>(SystemConstants.ServiceClientBaseUri + "secure", new Secure());
                Assert.Fail("Should throw WebServiceException.StatusCode == 401");
            }
            catch (WebServiceException webEx)
            {
                Assert401(webEx);
                Assert.That(webEx.ResponseDto, Is.Null);
            }
        }
        
    }
}