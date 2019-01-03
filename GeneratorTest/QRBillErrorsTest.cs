﻿//
// Swiss QR Bill Generator for .NET
// Copyright (c) 2018 Manuel Bleichenbacher
// Licensed under MIT License
// https://opensource.org/licenses/MIT
//

using Codecrete.SwissQRBill.Generator;
using Codecrete.SwissQRBill.Generator.Canvas;
using System;
using System.IO;
using Xunit;

namespace Codecrete.SwissQRBill.GeneratorTest
{
    public class QRBillErrorsTest
    {
        [Fact]
        void ThrowsRuntimeException()
        {
            Assert.Throws<QRBillGenerationException>(
                () => GenerateWithFailingCanvas()
            );
        }

        void GenerateWithFailingCanvas()
        {
            Bill bill = SampleData.CreateExample1();
            FailingCanvas canvas = new FailingCanvas();
            bill.Format.OutputSize = OutputSize.QRBillOnly;
            QRBill.Generate(bill, canvas);
        }

        [Fact]
        void ThrowsValidationError()
        {
            Assert.Throws<QRBillValidationError>(
                () => GenerateWithInvalidData()
            );
        }

        void GenerateWithInvalidData()
        {
            Bill bill = SampleData.CreateExample1();
            bill.Creditor.Name = " ";
            bill.Format.OutputSize = OutputSize.QRBillOnly;
            bill.Format.GraphicsFormat = GraphicsFormat.PDF;
            QRBill.Generate(bill);
        }

        class FailingCanvas : AbstractCanvas
        {
            public override void AddRectangle(double x, double y, double width, double height)
            {
                throw new NotImplementedException();
            }

            public override void CloseSubpath()
            {
                throw new NotImplementedException();
            }

            public override void CubicCurveTo(double x1, double y1, double x2, double y2, double x, double y)
            {
                throw new NotImplementedException();
            }

            public override void Dispose()
            {
                // be nice
            }

            public override void FillPath(int color)
            {
                throw new NotImplementedException();
            }

            public override byte[] GetResult()
            {
                throw new NotImplementedException();
            }

            public override void LineTo(double x, double y)
            {
                throw new NotImplementedException();
            }

            public override void MoveTo(double x, double y)
            {
                throw new NotImplementedException();
            }

            public override void PutText(string text, double x, double y, int fontSize, bool isBold)
            {
                throw new NotImplementedException();
            }

            public override void SetTransformation(double translateX, double translateY, double rotate, double scaleX, double scaleY)
            {
                throw new NotImplementedException();
            }

            public override void SetupPage(double width, double height, string fontFamily)
            {
                throw new NotImplementedException();
            }

            public override void StartPath()
            {
                throw new NotImplementedException();
            }

            public override void StrokePath(double strokeWidth, int color)
            {
                throw new NotImplementedException();
            }
        }
    }
}
