using System;
using System.Text;
using Auth_303.helpers;
using NUnit.Framework;
using ServiceStack.Messaging;
using ServiceStack.Messaging.Redis;
using ServiceStack.Redis;

namespace Auth_303.tests
{
    [TestFixture]
    public class MessagingTests
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

        [Test]
        public void can_call_an_unauthenticated_method()
        {
            var uniqueCallbackQ = "mq:c1" + ":" + Guid.NewGuid().ToString("N");
            var clientMsg = new Message<GetFactorial>(new GetFactorial {ForNumber = 2})
            {
                ReplyTo = uniqueCallbackQ
            };

            var redisFactory = new PooledRedisClientManager("localhost:6379");
            var mqHost = new RedisMqServer(redisFactory, retryCount: 2);

            var mqClient = mqHost.CreateMessageQueueClient();

            mqClient.Publish(clientMsg);
            var response = mqClient.Get<GetFactorialResponse>(clientMsg.ReplyTo, new TimeSpan(0, 0, 10)); //Blocks thread on client until reply message is received
            Assert.IsNotNull(response);
            Assert.AreEqual(2, response.GetBody().Result);
        }


        [Test]
        public void can_call_an_authenticated_method()
        {
            var uniqueCallbackQ = "mq:c1" + ":" + Guid.NewGuid().ToString("N");
            var clientMsg = new Message<Secure>(new Secure())
            {
                ReplyTo = uniqueCallbackQ,
                Tag = "basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(SystemConstants.AllowedUser + ":" + SystemConstants.AllowedPass))
            };

            var redisFactory = new PooledRedisClientManager("localhost:6379");
            var mqHost = new RedisMqServer(redisFactory, retryCount: 2);

            var mqClient = mqHost.CreateMessageQueueClient();

            mqClient.Publish(clientMsg);
            var response = mqClient.Get<SecureResponse>(clientMsg.ReplyTo, new TimeSpan(0, 0, 10)); //Blocks thread on client until reply message is received
            Assert.IsNotNull(response);
            Assert.AreEqual("Confidential", response.GetBody().Result);
        }

        [Test]
        public void can_authenticated_method_fails_without_creds()
        {
            var uniqueCallbackQ = "mq:c1" + ":" + Guid.NewGuid().ToString("N");
            var clientMsg = new Message<Secure>(new Secure())
            {
                ReplyTo = uniqueCallbackQ
            };

            var redisFactory = new PooledRedisClientManager("localhost:6379");
            var mqHost = new RedisMqServer(redisFactory, retryCount: 2);

            var mqClient = mqHost.CreateMessageQueueClient();

            mqClient.Publish(clientMsg);
            var response = mqClient.Get<SecureResponse>(clientMsg.ReplyTo, new TimeSpan(0, 0, 10)); //Blocks thread on client until reply message is received
            Assert.IsNotNull(response);
            Assert.AreEqual("Confidential", response.GetBody().Result);
        }
    }
}