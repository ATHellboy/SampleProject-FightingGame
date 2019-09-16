using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Infrastructure.Scripts.CQRS;
using Assets.Infrastructure.Validation;
using UniRx;
using UnityEngine;

namespace Assets.Infrastructure.MessageBusImplementations.UniRx
{
    public class UniRxMessageBus : IMessageBus
    {
        public readonly Subject<IMessage> _messages = new Subject<IMessage>();
        public IObservable<IMessage> Messages => _messages;

        public List<MessageRouteRule> Rules = new List<MessageRouteRule>();
        public SubscriptionRegistery RuleSubscriptions = new SubscriptionRegistery();

        private readonly List<RegisteryItem> _subscribedHandlerWithoutRule = new List<RegisteryItem>();

        public void RaiseEvent(IEvent evt)
        {
            if (null == evt)
            {
                return;
            }

            _messages.OnNext(evt);
        }

        public void SendCommand(ICommand cmd)
        {
            if (null == cmd)
            {
                return;
            }

            var validators = GetValidators(cmd);
            var validationContext = new ValidationContext();

            var lastValidatorResult = Validation.ValidationResult.Accepted;
            for (var i = 0; i < validators.Count && lastValidatorResult == ValidationResult.Accepted; ++i)
            {
                var validator = validators[i];
                lastValidatorResult = validator.IsValid(cmd, validationContext);
            }

            if (ValidationResult.Accepted == lastValidatorResult)
            {
                _messages.OnNext(cmd);
            }
        }

        private List<IValidator> GetValidators(ICommand cmd)
        {
            var cmdType = cmd.GetType();

            // TODO: Should find validators by validation or settings

            return new List<IValidator>();
        }

        public void Subscribe<TMessageHandler, TMessage>(TMessageHandler handler, IMessageHandlerActionExecutor methodSelector)
            where TMessage : class, IMessage
            where TMessageHandler : class, IMessageHandler<TMessage>
        {

            Subscribe(handler, typeof(TMessage), methodSelector);
        }

        private void Subscribe(IMessageHandler handler, Type messageType, IMessageHandlerActionExecutor methodSelector)
        {
            var messageHandlerType = handler.GetType();
            var rules = GetRule(messageHandlerType, messageType);

            if (null != rules && rules.Count > 0)
            {
                foreach (var rule in rules)
                {
                    var items = RuleSubscriptions.ItemsByRule(rule);
                    var item = items.FirstOrDefault(it => it.handler == handler);
                    if (null == item)
                    {
                        IObservable<IMessage> observable;

                        if (null != rule.Transformer)
                        {
                            observable =
                                Messages.Where(
                                    message =>
                                        rule.IncludeDrivedMessageTypes
                                            ? rule.Transformer.InputType.IsInstanceOfType(message)
                                            : message.GetType() == rule.Transformer.InputType);

                            if (null != rule.PreCondition)
                            {
                                observable = observable.Where(message => Scripts.CQRS.Validators.ValidationResult.Accepted == rule.PreCondition.Validate(handler, message));
                            }
                            observable = observable.Select(message => Convert.ChangeType(rule.Transformer.Transform(message), rule.Transformer.OutpuType) as IMessage);
                        }
                        else
                        {
                            observable = Messages.Where(message => rule.IncludeDrivedMessageTypes ? messageType.IsInstanceOfType(message) : messageType == message.GetType());
                        }

                        if (null != rule.PostCondition)
                        {
                            observable = observable.Where(message => Scripts.CQRS.Validators.ValidationResult.Accepted == rule.PostCondition.Validate(handler, message));
                        }

                        var subscription = observable.Subscribe(message => methodSelector.Execute(message));

                        RuleSubscriptions.Add(new SubscriptionRegisteryItem()
                        {
                            Route = rule,
                            handler = handler,
                            methodInfo = methodSelector,
                            MessagehandlerType = messageHandlerType,
                            MessageType = messageType,
                            Subscription = subscription
                        });
                    }
                    else
                    {
                        //Debug.LogWarning("Duplicate Subscription.");
                    }
                }
            }
            else
            {
                UnityEngine.Debug.LogWarning($"Message Rule for {messageHandlerType.Name}<{messageType}> Not Found.");

                var registeryItem = new RegisteryItem(messageType, handler, methodSelector);
                _subscribedHandlerWithoutRule.Add(registeryItem);
            }
        }

        public void Unsubscribe<TMessageHandler, TMessage>(TMessageHandler handler)
            where TMessage : class, IMessage
            where TMessageHandler : class, IMessageHandler<TMessage>
        {
            var rule = GetRule<TMessageHandler, TMessage>();

            if (null != rule)
            {
                var subscriptions = RuleSubscriptions.GetRuleSubscriptions(rule)?.ToList();
                if (null == subscriptions || subscriptions.Count <= 0) return;

                var handlerSubscriptions = subscriptions.Where(item => item.handler == handler).ToList();
                var handlerSubscriptionsCount = handlerSubscriptions.Count;
                if (handlerSubscriptionsCount <= 0) return;

                for (var i = handlerSubscriptionsCount - 1; i >= 0; --i)
                {
                    var subscription = subscriptions[i];
                    subscription.Subscription.Dispose();
                    subscriptions.Remove(subscription);
                }
            }
            else
            {
                UnityEngine.Debug.LogWarning($"Message Rule for {typeof(TMessageHandler)}<{typeof(TMessage)}> Not Found.");
            }
        }

        public void AddRule(MessageRouteRule rule)
        {
            Rules.Add(rule);

            var items =
                _subscribedHandlerWithoutRule.Where(item => item.HandlerType == rule.HandlerType && item.MessageType == rule.MessageType)
                    .ToList();

            if (items.Count <= 0)
            {
                return;
            }

            for (var i = items.Count - 1; i >= 0; --i)
            {
                var item = items[i];

                dynamic objHandler;
                var targetAvaliable = item.Handler.TryGetTarget(out objHandler);
                if (!targetAvaliable) continue;

                Subscribe(objHandler, item.MessageType, item.HandlerMethodSelector);

                items.Remove(item);
            }
        }

        private MessageRouteRule GetRule<TMessageHandler, TMessage>() where TMessage : IMessage where TMessageHandler : IMessageHandler<TMessage>
        {
            return Rules.FirstOrDefault(
                roteRule => typeof(TMessageHandler) == roteRule.HandlerType && typeof(TMessage) == roteRule.MessageType);
        }

        private List<MessageRouteRule> GetRule(Type messageHandlerType, Type messageType)
        {
            return Rules.Where(
                roteRule => messageHandlerType == roteRule.HandlerType && messageType == roteRule.MessageType)
                .ToList();
        }
    }

    public class SubscriptionRegistery : List<SubscriptionRegisteryItem>
    {
        public bool ContainsRule(MessageRouteRule routeRule)
        {
            return this.Any(item => item.Route == routeRule);
        }

        public IEnumerable<SubscriptionRegisteryItem> ItemsByRule(MessageRouteRule routeRule)
        {
            return this.Where(item => item.Route == routeRule);
        }

        public IEnumerable<SubscriptionRegisteryItem> GetRuleSubscriptions(MessageRouteRule rule)
        {
            return this.Where(item => item.Route == rule);
        }
    }

    public class SubscriptionRegisteryItem
    {
        public Type MessagehandlerType;
        public Type MessageType;
        public IMessageHandler handler;
        public IActionExecutor methodInfo;
        public MessageRouteRule Route;

        public IDisposable Subscription;

        public SubscriptionRegisteryItem()
        {

        }
    }

    public class RegisteryItem
    {
        public Type HandlerType { get; }
        public Type MessageType { get; }
        public WeakReference<dynamic> Handler { get; }
        public IMessageHandlerActionExecutor HandlerMethodSelector { get; }

        public RegisteryItem(Type messageType, object handler, IMessageHandlerActionExecutor handlerMethodSelector)
        {
            HandlerType = handler.GetType();
            MessageType = messageType;
            Handler = new WeakReference<object>(handler, false);
            HandlerMethodSelector = handlerMethodSelector;
        }

        // public static RegisteryItem Create<TMessageHandler, TMessage>(TMessageHandler handler, MessageHandlerActionExecutor methodSelector)
        //     where TMessage : IMessage
        //     where TMessageHandler : class, IMessageHandler<TMessage>
        // {
        //     return new RegisteryItem(typeof(TMessageHandler), typeof(TMessage), handler, methodSelector);
        // }
    }
}

