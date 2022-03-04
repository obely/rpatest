using Polly;

using rpatest.Abstractions;
using rpatest.Infrastructure.WindowsApi;

using System;
using System.Collections.Generic;
using System.Windows.Automation;

using Key = System.Windows.Input.Key;

namespace rpatest.Infrastructure
{
    public class ControlService : IControlService
    {
        private Policy WaitForControlPolicy { get; }

        public ControlService()
        {
            WaitForControlPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(3)
                }, (exception, timeSpan, retryCount, context) =>
                {
                    Console.WriteLine($"{exception.Message}. Retrying in {timeSpan.TotalSeconds}s...");
                });
        }

        public void Click(ControlInfo target)
        {
            var element = FindElement(target);
            var clickablePoint = element.GetClickablePoint();

            Mouse.MoveTo((int)clickablePoint.X, (int)clickablePoint.Y);
            Mouse.Click();
        }

        public void HotKey(int[] keys)
        {
            for (var i = 0; i < keys.Length; i++)
            {
                var key = (Key)keys[i];
                Keyboard.Press(key);
            }

            for (var i = keys.Length - 1; i >= 0; i--)
            {
                var key = (Key)keys[i];
                Keyboard.Release(key);
            }
        }

        public void TypeText(ControlInfo target, string text)
        {
            Click(target);

            Keyboard.Type(text);
        }

        private AutomationElement FindElement(ControlInfo target)
        {
            return WaitForControlPolicy.Execute(() =>
            {
                var parent = AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, target.Parent));

                if (parent == null)
                {
                    throw new Exception($"Unable to found the parent window '{target.Parent}'");
                }

                Condition condition = null;
                var properties = new List<PropertyCondition>();
                if (!string.IsNullOrEmpty(target.Id))
                {
                    properties.Add(new PropertyCondition(AutomationElement.AutomationIdProperty, target.Id));
                }
                if (!string.IsNullOrEmpty(target.Name))
                {
                    properties.Add(new PropertyCondition(AutomationElement.NameProperty, target.Name));
                }
                if (!string.IsNullOrEmpty(target.Type))
                {
                    properties.Add(new PropertyCondition(AutomationElement.LocalizedControlTypeProperty, target.Type));
                }
                if (!string.IsNullOrEmpty(target.Class))
                {
                    properties.Add(new PropertyCondition(AutomationElement.ClassNameProperty, target.Class));
                }

                if (properties.Count == 0)
                {
                    throw new Exception("There is no enough information to find the target control");
                }
                else if (properties.Count == 1)
                {
                    condition = properties[0];
                }
                else
                {
                    condition = new AndCondition(properties.ToArray());
                }

                var element = parent.FindFirst(TreeScope.Descendants, condition);

                if (element == null)
                {
                    throw new Exception($"Unable to find the target control {target}");
                }

                return element;
            });
        }
    }
}
