﻿using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Host;
using ServiceStack.Testing;

namespace Auth_303.helpers
{
    public class DirectServiceClient : IServiceClient, IRestClient
    {
        ServiceController ServiceController { get; set; }

        readonly MockHttpRequest _httpReq = new MockHttpRequest();
        readonly MockHttpResponse _httpRes = new MockHttpResponse();

        public DirectServiceClient(ServiceController serviceController)
        {
            ServiceController = serviceController;
        }

        public void SendOneWay(object requestDto)
        {
            ServiceController.Execute(requestDto);
        }

        public void SendOneWay(string relativeOrAbsoluteUri, object requestDto)
        {
            ServiceController.Execute(requestDto);
        }

        private bool ApplyRequestFilters<TResponse>(object request)
        {
            if (HostContext.ApplyRequestFilters(_httpReq, _httpRes, request))
            {
                ThrowIfError<TResponse>(_httpRes);
                return true;
            }
            return false;
        }

        private void ThrowIfError<TResponse>(MockHttpResponse httpRes)
        {
            if (httpRes.StatusCode >= 400)
            {
                var webEx = new WebServiceException("WebServiceException, StatusCode: " + httpRes.StatusCode)
                {
                    StatusCode = httpRes.StatusCode,
                    StatusDescription = httpRes.StatusDescription,
                };

                try
                {
                    var deserializer = HostContext.ContentTypes.GetStreamDeserializer(_httpReq.ResponseContentType);
                    webEx.ResponseDto = deserializer(typeof(TResponse), new MemoryStream(httpRes.ReadAsBytes()));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                throw webEx;
            }
        }

        private bool ApplyResponseFilters<TResponse>(object response)
        {
            if (HostContext.ApplyResponseFilters(_httpReq, _httpRes, response))
            {
                ThrowIfError<TResponse>(_httpRes);
                return true;
            }
            return false;
        }

        public TResponse Send<TResponse>(object request)
        {
            _httpReq.HttpMethod = HttpMethods.Post;

            if (ApplyRequestFilters<TResponse>(request)) return default(TResponse);

            _httpReq.HttpMethod = HttpMethods.Post;
            var response = ServiceController.Execute(request, _httpReq);

            if (ApplyResponseFilters<TResponse>(response)) return (TResponse)response;

            return (TResponse)response;
        }

        public TResponse Send<TResponse>(IReturn<TResponse> request)
        {
            throw new NotImplementedException();
        }

        public void Send(IReturnVoid request)
        {
            throw new NotImplementedException();
        }

        public TResponse Patch<TResponse>(string relativeOrAbsoluteUrl, object requestDto)
        {
            throw new NotImplementedException();
        }

        public HttpWebResponse CustomMethod(string httpVerb, IReturnVoid requestDto)
        {
            throw new NotImplementedException();
        }

        public HttpWebResponse CustomMethod(string httpVerb, object requestDto)
        {
            throw new NotImplementedException();
        }

        public TResponse CustomMethod<TResponse>(string httpVerb, IReturn<TResponse> requestDto)
        {
            throw new NotImplementedException();
        }

        public TResponse CustomMethod<TResponse>(string httpVerb, object requestDto)
        {
            throw new NotImplementedException();
        }

        public HttpWebResponse Head(IReturn requestDto)
        {
            throw new NotImplementedException();
        }

        public HttpWebResponse Head(object requestDto)
        {
            throw new NotImplementedException();
        }

        public HttpWebResponse Head(string relativeOrAbsoluteUrl)
        {
            throw new NotImplementedException();
        }

        TResponse PostFile<TResponse>(string relativeOrAbsoluteUrl, FileInfo fileToUpload, string mimeType)
        {
            throw new NotImplementedException();
        }

        public TResponse PostFile<TResponse>(string relativeOrAbsoluteUrl, Stream fileToUpload, string fileName, string mimeType)
        {
            throw new NotImplementedException();
        }

        public TResponse PostFileWithRequest<TResponse>(
            Stream fileToUpload, string fileName, object request, string fieldName = "upload")
        {
            throw new NotImplementedException();
        }

        public HttpWebResponse Get(object request)
        {
            throw new NotImplementedException();
        }

        public TResponse Get<TResponse>(IReturn<TResponse> requestDto)
        {
            throw new NotImplementedException();
        }

        public TResponse Get<TResponse>(object requestDto)
        {
            throw new NotImplementedException();
        }

        public HttpWebResponse Get(IReturnVoid request)
        {
            throw new NotImplementedException();
        }

        public TResponse Get<TResponse>(string relativeOrAbsoluteUrl)
        {
            _httpReq.HttpMethod = HttpMethods.Get;

            var requestTypeName = typeof(TResponse).Namespace + "." + relativeOrAbsoluteUrl;
            var requestType = typeof(TResponse).Assembly.GetType(requestTypeName);
            if (requestType == null)
                throw new ArgumentException("Type not found: " + requestTypeName);

            var request = requestType.CreateInstance();

            if (ApplyRequestFilters<TResponse>(request)) return default(TResponse);

            _httpReq.HttpMethod = HttpMethods.Get;
            var response = ServiceController.Execute(request, _httpReq);

            if (ApplyResponseFilters<TResponse>(response)) return (TResponse)response;

            return (TResponse)response;
        }

        public HttpWebResponse Delete(IReturnVoid requestDto)
        {
            throw new NotImplementedException();
        }

        public HttpWebResponse Delete(object requestDto)
        {
            throw new NotImplementedException();
        }

        public TResponse Delete<TResponse>(IReturn<TResponse> request)
        {
            throw new NotImplementedException();
        }

        public TResponse Delete<TResponse>(object request)
        {
            throw new NotImplementedException();
        }

        public TResponse Delete<TResponse>(string relativeOrAbsoluteUrl)
        {
            throw new NotImplementedException();
        }

        public HttpWebResponse Post(IReturnVoid requestDto)
        {
            throw new NotImplementedException();
        }

        public HttpWebResponse Post(object requestDto)
        {
            throw new NotImplementedException();
        }

        public TResponse Post<TResponse>(IReturn<TResponse> requestDto)
        {
            throw new NotImplementedException();
        }

        public TResponse Post<TResponse>(object requestDto)
        {
            throw new NotImplementedException();
        }

        public TResponse Post<TResponse>(string relativeOrAbsoluteUrl, object request)
        {
            throw new NotImplementedException();
        }

        public HttpWebResponse Put(IReturnVoid requestDto)
        {
            throw new NotImplementedException();
        }

        public HttpWebResponse Put(object requestDto)
        {
            throw new NotImplementedException();
        }

        public TResponse Put<TResponse>(IReturn<TResponse> requestDto)
        {
            throw new NotImplementedException();
        }

        public TResponse Put<TResponse>(object requestDto)
        {
            throw new NotImplementedException();
        }

        public TResponse Put<TResponse>(string relativeOrAbsoluteUrl, object requestDto)
        {
            throw new NotImplementedException();
        }

        public HttpWebResponse Patch(IReturnVoid requestDto)
        {
            throw new NotImplementedException();
        }

        public HttpWebResponse Patch(object requestDto)
        {
            throw new NotImplementedException();
        }

        public TResponse Patch<TResponse>(IReturn<TResponse> requestDto)
        {
            throw new NotImplementedException();
        }

        public TResponse Patch<TResponse>(object requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> SendAsync<TResponse>(object requestDto)
        {
            var tcs = new TaskCompletionSource<TResponse>();
            try
            {
                try
                {
                    if (ApplyRequestFilters<TResponse>(requestDto))
                    {
                        tcs.SetResult(default(TResponse));
                        return tcs.Task;
                    }
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                    return tcs.Task;
                }

                var response = Send<TResponse>(requestDto);

                try
                {
                    if (ApplyResponseFilters<TResponse>(requestDto))
                    {
                        tcs.SetResult(response);
                        return tcs.Task;
                    }
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                    return tcs.Task;
                }

                tcs.SetResult(response);
                return tcs.Task;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);

                tcs.SetException(ex);
                return tcs.Task;
            }
        }

        public void SetCredentials(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> GetAsync<TResponse>(IReturn<TResponse> requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> GetAsync<TResponse>(object requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> GetAsync<TResponse>(string relativeOrAbsoluteUrl)
        {
            throw new NotImplementedException();
        }

        public Task<HttpWebResponse> GetAsync(IReturnVoid requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> DeleteAsync<TResponse>(IReturn<TResponse> requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> DeleteAsync<TResponse>(object requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> DeleteAsync<TResponse>(string relativeOrAbsoluteUrl)
        {
            throw new NotImplementedException();
        }

        public Task<HttpWebResponse> DeleteAsync(IReturnVoid requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> PostAsync<TResponse>(IReturn<TResponse> requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> PostAsync<TResponse>(object requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> PostAsync<TResponse>(string relativeOrAbsoluteUrl, object request)
        {
            throw new NotImplementedException();
        }

        public Task<HttpWebResponse> PostAsync(IReturnVoid requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> PutAsync<TResponse>(IReturn<TResponse> requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> PutAsync<TResponse>(object requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> PutAsync<TResponse>(string relativeOrAbsoluteUrl, object request)
        {
            throw new NotImplementedException();
        }

        public Task<HttpWebResponse> PutAsync(IReturnVoid requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> CustomMethodAsync<TResponse>(string httpVerb, IReturn<TResponse> requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> CustomMethodAsync<TResponse>(string httpVerb, object requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<HttpWebResponse> CustomMethodAsync(string httpVerb, IReturnVoid requestDto)
        {
            throw new NotImplementedException();
        }

        public void CancelAsync()
        {
            throw new NotImplementedException();
        }

        public void SendAsync<TResponse>(object requestDto, Action<TResponse> onSuccess, Action<TResponse, Exception> onError)
        {
            throw new NotImplementedException();
        }

        public void Dispose() { }
        public TResponse PostFileWithRequest<TResponse>(string relativeOrAbsoluteUrl, FileInfo fileToUpload, object request, string fieldName = "upload")
        {
            throw new NotImplementedException();
        }

        public TResponse PostFileWithRequest<TResponse>(string relativeOrAbsoluteUrl, Stream fileToUpload, string fileName, object request, string fieldName = "upload")
        {
            throw new NotImplementedException();
        }
    }
}