﻿//
// Swiss QR Bill Generator for .NET
// Copyright (c) 2021 Manuel Bleichenbacher
// Licensed under MIT License
// https://opensource.org/licenses/MIT
//

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Codecrete.SwissQRBill.Generator.Canvas
{
    /// <summary>
    /// Creates <c>ICanvas</c> instances using the registered <c>ICanvasFactory</c> instances.
    /// </summary>
    /// <seealso cref="ICanvas"/>
    /// <seealso cref="ICanvasFactory"/>
    public static class CanvasCreator
    {
        static CanvasCreator()
        {
            // Add default factories
            Register(new SVGCanvasFactory());
            Register(new PDFCanvasFactory());
        }

        /// <summary>
        /// Register an additional canvas factory.
        /// <para>
        /// Factories added later take precedence over earlier added factories.</para>
        /// </summary>
        /// <param name="factory">factory to add</param>
        public static void Register(ICanvasFactory factory)
        {
            factories.Insert(0, factory);
        }

        /// <summary>
        /// Creates a new <c>ICanvas</c> instance for the specified bill format.
        /// </summary>
        /// <param name="format">bill format</param>
        /// <param name="width">canvas width, in mm</param>
        /// <param name="height">canvas height, in mm</param>
        /// <returns></returns>
        public static ICanvas Create(BillFormat format, double width, double height)
        {
            foreach (ICanvasFactory factory in factories)
                if (factory.CanCreate(format))
                    return factory.Create(format, width, height);

            return null;
        }

        /// <summary>
        /// Registers the canvas factory for PNG output.
        /// <para>
        /// The method checks if the Codecrete.SwissQRBill.Generator assembly is present.
        /// </para>
        /// </summary>
        public static void RegisterPixelCanvasFactory()
        {
            if (checkedForPixelCanvas)
                return;

            checkedForPixelCanvas = true;

            string factoryClass = null;
            string assemblyName = null;

            // Load the assembly; most likely it has not been loaded yet as there is no direct reference to it
            try
            {
                Assembly.Load("Codecrete.SwissQRBill.Windows, Version=2.4.0.0, Culture=neutral, PublicKeyToken=6aa6bd7a159d47c2");
                factoryClass = "Codecrete.SwissQRBill.Windows.PNGCanvasFactory";
                assemblyName = "Codecrete.SwissQRBill.Windows";
            }
            catch
            {
                // Ignore as the next assembly is tried.
            }

            if (factoryClass == null)
            {
                try
                {
                    Assembly.Load("Codecrete.SwissQRBill.Generator, Version=2.4.0.0, Culture=neutral, PublicKeyToken=6aa6bd7a159d47c2");
                    factoryClass = "Codecrete.SwissQRBill.PixelCanvas.PNGCanvasFactory";
                    assemblyName = "Codecrete.SwissQRBill.Generator";
                }
                catch
                {
                    // Ignore if we are unable to load the assembly.
                    // A more meaningful exception is thrown outside this code.
                }
            }

            // Locate the PixelCanvas assembly and the PNGCanvasFactory class and register it
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetName().Name == assemblyName)
                {
                    Type factoryType = assembly.GetType(factoryClass);
                    if (factoryType != null)
                    {
                        ICanvasFactory factory = (ICanvasFactory)Activator.CreateInstance(factoryType);
                        Register(factory);
                        return;
                    }
                }
            }
        }

        private static readonly List<ICanvasFactory> factories = new List<ICanvasFactory>();

        private static bool checkedForPixelCanvas = false;
    }
}
