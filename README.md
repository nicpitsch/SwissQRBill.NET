# Swiss QR Bill for .NET

Open-source .NET library to generate Swiss QR bills (jointly developed with the [Java version](https://github.com/manuelbl/SwissQRBill)).

Try it yourself and [create a QR bill](https://www.codecrete.net/qrbill). The code for the demo app (Angular UI and RESTful service) can be found on [GitHub](https://github.com/manuelbl/SwissQRBillDemo) as well.

## Introduction

The Swiss QR bill is the new QR code based payment format that started on 30 June, 2020. The old payment slip will no longer be accepted after 30 September 2022.

The new payment slip will be sent electronically in most cases. But it can still be printed at the bottom of an invoice or added to the invoice on a separate sheet. The payer scans the QR code with his/her mobile banking app to initiate the payment. The payment just needs to be confirmed.

If the invoicing party adds structured bill information (VAT rates, payment conditions etc.) to the QR bill, posting in accounts payable can be automated. The invoicing party can also automate the accounts receivable processing as the payment includes all relevant data including a reference number. The Swiss QR bill is convenient for the payer and payee.

![QR Bill](https://raw.githubusercontent.com/wiki/manuelbl/SwissQRBill/images/qr-invoice-e1.svg?sanitize=true)

*More [examples](https://github.com/manuelbl/SwissQRBill/wiki/Swiss-QR-Invoice-Examples) can be found in the [Wiki](https://github.com/manuelbl/SwissQRBill/wiki)*


## Features

The Swiss QR bill library:

- generates QR bills as PDF, SVG,  PNG and EMF files
- generates payment slip (105mm by 210mm), A4 sheets or QR code only
- multilingual: German, French, Italian, English, Romansh
- validates the invoice data and provides detailed validation information
- adds or retrieves structured bill information (according to [Swico S1](https://www.swiss-qr-invoice.org/downloads/qr-bill-s1-syntax-de.pdf))
- parses the invoice data embedded in the QR code
- is easy to use (see example below)
- is small and fast
- is free – even for commercial use (MIT License)
- is built for .NET Standard 2.0, i.e. it runs with .NET Core 2.0 or higher, .NET Framework 4.6.1 or higher, Mono 5.4 or higher, Universal Windows Platform 10.0.16299 or higher, Xamarin etc.
- core library is light-weight and has a single dependency: Net.Codecrete.QrCodeGenerator
- enhanced version uses SkiaSharp for PNG file generation
- Windows version uses `System.Drawing` for generating PNG and EMF files
- available as a NuGet packages (see below)


## Getting started

1. Create a new Visual Studio project for .NET Core 2.x (*File > New > Project...* / *Visual C# > .NET Core > Console App (.NET Core)*)

2. Add the library via NuGet:

   Either via *Project > Manage NuGet Packages...* / *Browse* / search for *qrbill* / *Install*
   
   Or by running a command in the Package Manager Console

```
Install-Package Codecrete.SwissQRBill.Generator -Version 3.0.3
```

3. Add the code:

```c#
using Codecrete.SwissQRBill.Generator;
using System;
using System.IO;

namespace Codecrete.SwissQRBill.Examples.Basic
{
    class Program
    {
        static void Main(string[] args)
        {
            // Setup bill data
            Bill bill = new Bill
            {
                // creditor data
                Account = "CH4431999123000889012",
                Creditor = new Address
                {
                    Name = "Robert Schneider AG",
                    AddressLine1 = "Rue du Lac 1268/2/22",
                    AddressLine2 = "2501 Biel",
                    CountryCode = "CH"
                },

                // payment data
                Amount = 199.95m,
                Currency = "CHF",
                
                // debtor data
                Debtor = new Address
                {
                    Name = "Pia-Maria Rutschmann-Schnyder",
                    AddressLine1 = "Grosse Marktgasse 28",
                    AddressLine2 = "9400 Rorschach",
                    CountryCode = "CH"
                },

                // more payment data
                Reference = "210000000003139471430009017",
                UnstructuredMessage = "Abonnement für 2020",


                // output format
                Format = new BillFormat
                {
                    Language = Language.DE,
                    GraphicsFormat = GraphicsFormat.SVG,
                    OutputSize = OutputSize.QrBillOnly
                }
            };

            // Generate QR bill
            byte[] svg = QRBill.Generate(bill);

            // Save generated SVG file
            const string path = "qrbill.svg";
            File.WriteAllBytes(path, svg);
            Console.WriteLine($"QR bill saved at { Path.GetFullPath(path) }");
        }
    }
}
```

4. Run it

## API documentation

See DocFX [API Documentation](https://codecrete.net/SwissQRBill.NET/api/index.html)


## NuGet packages

Swiss QR Bill generation is available as three different NuGet packages. They all include the basic QR bill generation for PDF and SVG and only differ with regards the PNG and EMF generation.

| NuGet packages | PDF | SVG | PNG | EMF | Platform neutral | Recommendation |
| -- | :--: | :--: | :--: | :--: | :--: | -- |
| [Codecrete.SwissQRBill.Core](https://www.nuget.org/packages/Codecrete.SwissQRBill.Core/) | ✓ | ✓ | – | – | ✓ | Platform-independent core without PNG and EMF generation. |
| [Codecrete.SwissQRBill.Generator](https://www.nuget.org/packages/Codecrete.SwissQRBill.Generator/) | ✓ | ✓ | ✓ | – | ✓ | Core plus platform-independent PNG generation (based on [SkiaSharp](https://github.com/mono/SkiaSharp)). |
| [Codecrete.SwissQRBill.Windows](https://www.nuget.org/packages/Codecrete.SwissQRBill.Windows/) | ✓ | ✓ | ✓ | ✓ | – | Windows specific package including core plus PNG and EMF generation based on [System.Drawing.Common](https://www.nuget.org/packages/System.Drawing.Common) |



## PNG generation

PNG generation requires a raster graphics library. Starting with .NET 6, the *System.Drawing* classes have become a Windows-only technology and standard .NET no longer supports raster graphics out-of-the-box. With this library, you have several options:

- If you do not need PNG generation, you can use the light-weight core library: **Codecrete.SwissQRBill.Core**.
- If you need PNG generation, you can use the enhanced version: **Codecrete.SwissQRBill.Generator**. It uses [SkiaSharp](https://github.com/mono/SkiaSharp) as a platform independent raster graphics library. Note that on Linux, SkiaSharp depends on native libraries that might not be installed on your machine. The easiest solution is to add the NuGet package [SkiaSharp.NativeAssets.Linux.NoDependencies](https://www.nuget.org/packages/SkiaSharp.NativeAssets.Linux.NoDependencies) to your project.
- If you creating a Windows-only software, use the **Codecrete.SwissQRBill.Windows** package. It uses the *System.Drawing* classes for PNG generation and adds EMF generation on top.

**Note on strong-naming / assembly signature**: The latest version of *SkiaSharp* (version 2.80.3) has an invalid signature. Thus, the assembly cannot be loaded in an environment requiring a signature / strong-naming. To work around it, use the *NuGet Package Manager* and selectively downgrade SkiaSharp to version 2.80.2. This only works with *Codecrete.SwissQRBill.Generator* 3.0.2 and later.

## PDF generation

To generate QR bills as PDF files, this library uses its own, minimal PDF generator that requires no further dependencies and comes with the same permissive license as the rest of this library.

If you are already using [iText](https://itextpdf.com/en) or [PDFsharp](http://www.pdfsharp.net/), then have a look at the respective examples:

- iText: https://github.com/manuelbl/SwissQRBill.NET/tree/master/Examples/iText
- PDFsharp: https://github.com/manuelbl/SwissQRBill.NET/tree/master/Examples/PDFsharp

These examples also support adding a QR bill to an existing PDF document.

## Code examples

This library comes with multiple code examples:

- [Basic](Examples/Basic): A basic example showing how to generate a QR bill.

- [WindowsForms](Examples/WindowsForms): An extensive example showing how to use this library in a Windows Forms application (display of QR bills on the screen, printing, working with the clipboard).

- [WindowsPresentationFoundation](Examples/WindowsPresentationFoundation): An example for Windows Presentation Foundation (WPF) applications. It shows how to display and print QR bills.

- [MicrosoftWordAddIn](https://github.com/manuelbl/SwissQRBill.NET/tree/master/Examples/MicrosoftWordAddIn): Implements an add-in for Microsoft Word capable of inserting a QR bill as a resolution independent EMF graphics. As C# add-ins use the *Microsoft Word Interop* interface, this example is also relevant for other software interacting with Microsoft Word.

- [iText](Examples/iText): Example showing how to generate PDFs using the [iText](https://itextpdf.com/en) library, including how to add a QR bill to an existing PDF document.

- [PDFsharp](Examples/PDFsharp): Example showing how to generate PDFs using the [PDFsharp](http://www.pdfsharp.net) library, including how to add a QR bill to an existing PDF document.

## More information

This library is the .NET version of Swiss QR Bill. There is also a [Java version](https://github.com/manuelbl/SwissQRBill) with the same features. More information about both libraries can be found in the [Wiki](https://github.com/manuelbl/SwissQRBill/wiki).

## Other programming languages

If you are looking for a library for yet another programming language or for a library with professional services, you might want to check out [Services & Tools](https://www.moneytoday.ch/iso20022/movers-shakers/software-hersteller/services-tools/) on [MoneyToday.ch](https://www.moneytoday.ch).
