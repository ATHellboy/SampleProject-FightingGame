using System;
using System.Text;
using Assets.Infrastructure.MessageBusImplementations.UniRx;
using Assets.Infrastructure.Scripts.CQRS;
using Assets.Infrastructure.Scripts.CQRS.Validators;
using Assets.Infrastructure.UnitTests.Editor.MessageBus.UniRx.Handlers;
using Assets.Infrastructure.UnitTests.Editor.MessageBus.UniRx.Messages;
using Assets.Infrastructure.UnitTests.Editor.MessageBus.UniRx.Transformers;
using Assets.Infrastructure.UnitTests.Editor.MessageBus.UniRx.Validators;
using NUnit.Framework;

namespace Assets.Infrastructure.UnitTests.Editor.MessageBus.UniRx
{
    [TestFixture]
    public class UniRxMessageBusTests
    {
        [SetUp]
        public void BindInterfaces()
        {
            
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void When_Subscribe_After_Add_Rule_Should_Handler_Execute()
        {
            var messagebus = new UniRxMessageBus();

            var handler = new SomeMessageHandler(string.Empty);

            messagebus.AddRule(MessageRouteRule.Create<SomeMessage, SomeMessageHandler>(string.Empty, false));

            messagebus.Subscribe<SomeMessageHandler, SomeMessage>(handler,
                new MessageHandlerActionExecutor<SomeMessage>(msg => handler.Handle(msg)));

            var message = new SomeMessage("First message", string.Empty, DateTime.UtcNow);

            messagebus.RaiseEvent(message);

            Assert.AreEqual(1, handler.HandleCallCount);
        }

        [Test]
        public void When_Subscribe_Before_Add_Rule_Should_Handler_Execute()
        {
            var messagebus = new UniRxMessageBus();

            var handler = new SomeMessageHandler(string.Empty);

            messagebus.Subscribe<SomeMessageHandler, SomeMessage>(handler,
                new MessageHandlerActionExecutor<SomeMessage>(msg => handler.Handle(msg)));

            messagebus.AddRule(MessageRouteRule.Create<SomeMessage, SomeMessageHandler>(string.Empty, false));

            var message = new SomeMessage("First message", string.Empty, DateTime.UtcNow);

            messagebus.RaiseEvent(message);

            Assert.AreEqual(1, handler.HandleCallCount);
        }

        [Test]
        public void When_Subscribe_Two_Handler_After_Add_A_Null_Condition_Rule_Should_Both_Handler_Execute()
        {
            var messagebus = new UniRxMessageBus();

            // Define some const
            const string firstHandlerName = "handler First";
            const string secondHandlerName = "handler Second";
            const string firstMessageString = firstHandlerName + "_Message";
            const string secondMessageString = secondHandlerName + "_Message";

            // Create Messages
            var firstMessage = new SomeMessage(firstMessageString, string.Empty, DateTime.UtcNow);
            var secondMessage = new SomeMessage(secondMessageString, string.Empty, DateTime.UtcNow);

            // Create Handlers
            var firstHandler = new SomeMessageHandler(firstHandlerName);
            var secondHandler = new SomeMessageHandler(secondHandlerName);

            // Add Rules
            messagebus.AddRule(MessageRouteRule.Create<SomeMessage, SomeMessageHandler>(firstHandlerName + "_Rule", false));

            // Subscribe handlers
            messagebus.Subscribe<SomeMessageHandler, SomeMessage>(firstHandler,
                new MessageHandlerActionExecutor<SomeMessage>(msg => firstHandler.Handle(msg)));

            messagebus.Subscribe<SomeMessageHandler, SomeMessage>(secondHandler,
                new MessageHandlerActionExecutor<SomeMessage>(msg => secondHandler.Handle(msg)));

            // Raise events
            messagebus.RaiseEvent(firstMessage);
            messagebus.RaiseEvent(secondMessage);

            // Assertions
            Assert.AreEqual(2, firstHandler.HandleCallCount, CreateAssetMessage(firstHandler, 0));
            Assert.AreEqual(2, secondHandler.HandleCallCount, CreateAssetMessage(secondHandler, 1));
        }

        [Test]
        public void When_Subscribe_Two_Handler_After_Add_A_True_Condition_Rule_Should_Both_Handler_Execute()
        {
            var messagebus = new UniRxMessageBus();

            // Define some const
            const string firstHandlerName = "handler First";
            const string secondHandlerName = "handler Second";
            const string firstMessageString = firstHandlerName + "_Message";
            const string secondMessageString = secondHandlerName + "_Message";

            // Create Messages
            var firstMessage = new SomeMessage(firstMessageString, string.Empty, DateTime.UtcNow);
            var secondMessage = new SomeMessage(secondMessageString, string.Empty, DateTime.UtcNow);

            // Create Handlers
            var firstHandler = new SomeMessageHandler(firstHandlerName);
            var secondHandler = new SomeMessageHandler(secondHandlerName);

            // Add Rules
            messagebus.AddRule(MessageRouteRule.Create<SomeMessage, SomeMessageHandler>(firstHandlerName + "_Rule", false, new AlwaysConstResultConditionValidator(ValidationResult.Accepted)));

            // Subscribe handlers
            messagebus.Subscribe<SomeMessageHandler, SomeMessage>(firstHandler,
                new MessageHandlerActionExecutor<SomeMessage>(msg => firstHandler.Handle(msg)));

            messagebus.Subscribe<SomeMessageHandler, SomeMessage>(secondHandler,
                new MessageHandlerActionExecutor<SomeMessage>(msg => secondHandler.Handle(msg)));

            // Raise events
            messagebus.RaiseEvent(firstMessage);
            messagebus.RaiseEvent(secondMessage);

            // Assertions
            Assert.AreEqual(2, firstHandler.HandleCallCount, CreateAssetMessage(firstHandler, 0));
            Assert.AreEqual(2, secondHandler.HandleCallCount, CreateAssetMessage(secondHandler, 1));
        }

        [Test]
        public void When_Subscribe_Two_Handler_After_Add_A_False_Condition_Rule_Should_Both_Handler_Execute()
        {
            var messagebus = new UniRxMessageBus();

            // Define some const
            const string firstHandlerName = "handler First";
            const string secondHandlerName = "handler Second";
            const string firstMessageString = firstHandlerName + "_Message";
            const string secondMessageString = secondHandlerName + "_Message";

            // Create Messages
            var firstMessage = new SomeMessage(firstMessageString, string.Empty, DateTime.UtcNow);
            var secondMessage = new SomeMessage(secondMessageString, string.Empty, DateTime.UtcNow);

            // Create Handlers
            var firstHandler = new SomeMessageHandler(firstHandlerName);
            var secondHandler = new SomeMessageHandler(secondHandlerName);

            // Add Rules
            messagebus.AddRule(MessageRouteRule.Create<SomeMessage, SomeMessageHandler>(firstHandlerName + "_Rule", false, new AlwaysConstResultConditionValidator(ValidationResult.Rejected)));

            // Subscribe handlers
            messagebus.Subscribe<SomeMessageHandler, SomeMessage>(firstHandler,
                new MessageHandlerActionExecutor<SomeMessage>(msg => firstHandler.Handle(msg)));

            messagebus.Subscribe<SomeMessageHandler, SomeMessage>(secondHandler,
                new MessageHandlerActionExecutor<SomeMessage>(msg => secondHandler.Handle(msg)));

            // Raise events
            messagebus.RaiseEvent(firstMessage);
            messagebus.RaiseEvent(secondMessage);

            // Assertions
            Assert.AreEqual(0, firstHandler.HandleCallCount, CreateAssetMessage(firstHandler, 0));
            Assert.AreEqual(0, secondHandler.HandleCallCount, CreateAssetMessage(secondHandler, 1));
        }

        [Test]
        public void When_Subscribe_Two_Handler_After_Add_Two_Different_Rule_With_Different_Condition_Should_Handler_Execute()
        {
            var messagebus = new UniRxMessageBus();

            // Define some const
            const string firstHandlerName = "handler First";
            const string secondHandlerName = "handler Second";
            const string firstMessageString = firstHandlerName +"_Message";
            const string secondMessageString = secondHandlerName + "_Message";
            
            // Create Messages
            var firstMessage = new SomeMessage(firstMessageString, string.Empty, DateTime.UtcNow);
            var secondMessage = new SomeMessage(secondMessageString, string.Empty, DateTime.UtcNow);

            // Create Handlers
            var firstHandler = new SomeMessageHandler(firstHandlerName);
            var secondHandler = new SomeMessageHandler(secondHandlerName);

            // Add Rules
            var firstCondition = new SampleConditionValidator(firstHandlerName, firstMessageString);
            messagebus.AddRule(MessageRouteRule.Create<SomeMessage, SomeMessageHandler>(firstHandlerName + "_Rule", false, firstCondition));

            var secondCondition = new SampleConditionValidator(secondHandlerName, secondMessageString);
            messagebus.AddRule(MessageRouteRule.Create<SomeMessage, SomeMessageHandler>(secondHandlerName + "_Rule", false, secondCondition));
            

            // Subscribe handlers
            messagebus.Subscribe<SomeMessageHandler, SomeMessage>(firstHandler,
                new MessageHandlerActionExecutor<SomeMessage>(msg => firstHandler.Handle(msg)));

            messagebus.Subscribe<SomeMessageHandler, SomeMessage>(secondHandler,
                new MessageHandlerActionExecutor<SomeMessage>(msg => secondHandler.Handle(msg)));

            // Raise events
            messagebus.RaiseEvent(firstMessage);
            messagebus.RaiseEvent(secondMessage);

            // Assertions
            Assert.AreEqual(1, firstHandler.HandleCallCount, CreateAssetMessage(firstHandler, 0));
            Assert.AreEqual(1, secondHandler.HandleCallCount, CreateAssetMessage(secondHandler, 1));
        }

        [Test]
        public void When_Subscribe_Two_Handler_After_Add_A_Rule_Should_Handlers_Execute()
        {
            var messagebus = new UniRxMessageBus();

            // Define some const
            const string firstHandlerName = "handler First";
            const string secondHandlerName = "handler Second";
            const string firstMessageString = firstHandlerName + "_Message";

            // Create Messages
            var firstMessage = new SomeMessage(firstMessageString, string.Empty, DateTime.UtcNow);

            // Create Handlers
            var firstHandler = new SomeMessageHandler(firstHandlerName);
            var secondHandler = new SomeMessageHandler(secondHandlerName);

            // Add Rules
            messagebus.AddRule(MessageRouteRule.Create<SomeMessage, SomeMessageHandler>(firstHandlerName + "_Rule", false, null, null, null));

            // Subscribe handlers
            messagebus.Subscribe<SomeMessageHandler, SomeMessage>(firstHandler,
                new MessageHandlerActionExecutor<SomeMessage>(msg => firstHandler.Handle(msg)));

            messagebus.Subscribe<SomeMessageHandler, SomeMessage>(secondHandler,
                new MessageHandlerActionExecutor<SomeMessage>(msg => secondHandler.Handle(msg)));

            // Raise events
            messagebus.RaiseEvent(firstMessage);

            // Assertions
            Assert.AreEqual(1, firstHandler.HandleCallCount, CreateAssetMessage(firstHandler, 0));
            Assert.AreEqual(1, secondHandler.HandleCallCount, CreateAssetMessage(secondHandler, 1));
        }

        [Test]
        public void When_Transform_Message_On_Rule_Should_Handle_Message_On_Subscriber()
        {
            var messagebus = new UniRxMessageBus();

            var handler = new SomeMessageHandler(string.Empty);

            messagebus.AddRule(MessageRouteRule.Create<SomeOtherMessage, SomeMessage, SomeMessageHandler>(string.Empty, false,
                null, new SampleTransformer(), null));

            messagebus.Subscribe<SomeMessageHandler, SomeMessage>(handler,
                new MessageHandlerActionExecutor<SomeMessage>(msg => handler.Handle(msg)));

            var message = new SomeOtherMessage("First message", string.Empty, DateTime.UtcNow);

            messagebus.RaiseEvent(message);

            Assert.AreEqual(1, handler.HandleCallCount);
        }

        [Test]
        public void When_Subscribe_Message_Type_Rule_And_Raise_Drived_Message_Type_Should_Not_Handler_Execute()
        {
            var messagebus = new UniRxMessageBus();

            var handler = new SomeMessageHandler(string.Empty);

            messagebus.Subscribe<SomeMessageHandler, SomeMessage>(handler,
                new MessageHandlerActionExecutor<SomeMessage>(msg => handler.Handle(msg)));

            messagebus.AddRule(MessageRouteRule.Create<SomeMessage, SomeMessageHandler>(string.Empty, false));

            var message = new SomeDrivedMessage("First message", string.Empty, DateTime.UtcNow);

            messagebus.RaiseEvent(message);

            Assert.AreEqual(0, handler.HandleCallCount);
        }

        [Test]
        public void When_Subscribe_Message_Type_Rule_And_Raise_Drived_Message_Type_Should_Handler_Execute()
        {
            var messagebus = new UniRxMessageBus();

            var handler = new SomeMessageHandler(string.Empty);

            messagebus.Subscribe<SomeMessageHandler, SomeMessage>(handler,
                new MessageHandlerActionExecutor<SomeMessage>(msg => handler.Handle(msg)));

            messagebus.AddRule(MessageRouteRule.Create<SomeMessage, SomeMessageHandler>(string.Empty, true));

            var message = new SomeDrivedMessage("First message", string.Empty, DateTime.UtcNow);

            messagebus.RaiseEvent(message);

            Assert.AreEqual(1, handler.HandleCallCount);
        }

        private string CreateAssetMessage(SomeMessageHandler handler, int indent)
        {
            var strBuilder = new StringBuilder();

            strBuilder.Append($"\n{new string(' ', indent)}Handler Name: {handler.Name}");
            strBuilder.Append($"\n{new string(' ', indent)}Handler Type: {handler.Name}");
            strBuilder.Append($"\n{new string(' ', indent)}Handled Message Count: {handler.HandleCallCount}");
            strBuilder.Append($"\n{new string(' ', indent)}Handled Messages:");
            var messages = handler.HandledMessages;
            foreach (var message in messages)
            {
                strBuilder.Append($"\n{new string(' ', indent + 1)}Message: {message.Message}");
            }

            return strBuilder.ToString();
        }
    }
}