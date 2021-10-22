﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using WslToolbox.Gui.Handlers;

namespace WslToolbox.Gui.Helpers
{
    public class BindElement
    {
        public readonly BindingMode BindingMode;
        public readonly object BindingSource;
        public readonly string Command;
        public readonly UIElement Element;
        public readonly DependencyProperty Property;

        public BindElement(UIElement element, DependencyProperty property, string command, object bindingSource,
            BindingMode bindingMode = BindingMode.Default)
        {
            Element = element;
            Property = property;
            Command = command;
            BindingMode = bindingMode;
            BindingSource = bindingSource;
        }
    }

    public static class BindHelper
    {
        public static void AddBindings(IEnumerable<BindElement> bindElements)
        {
            foreach (var bindElement in bindElements)
                try
                {
                    BindingOperations.SetBinding(bindElement.Element, bindElement.Property, BindingObject(
                        bindElement.Command, bindElement.BindingSource,
                        bindElement.BindingMode));

                    LogHandler.Log()
                        .Debug("Registered binding element {Element} to {Command}",
                            bindElement.Element.ToString(), bindElement.Command);
                }
                catch (Exception e)
                {
                    LogHandler.Log()
                        .Error("Failed to bind element {Element} to {Command}: {Message}",
                            bindElement.Element.ToString(), bindElement.Command, e.Message);
                }
        }

        public static Binding BindingObject(string path, object source, BindingMode mode = BindingMode.Default)
        {
            return new Binding(path)
            {
                Mode = mode,
                Source = source
            };
        }
    }
}