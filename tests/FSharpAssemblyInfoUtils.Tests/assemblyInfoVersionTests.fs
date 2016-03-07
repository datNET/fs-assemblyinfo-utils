﻿module Tests

open System
open NUnit.Framework
open FsUnit
open FSharpUtil.AssemblyInfo

let assemblyInfoFsContent =
    @"
namespace FSharpAssemblyInfoUtils.AssemblyInfo

open System.Reflection
open System.Runtime.CompilerServices
open System.Runtime.InteropServices

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[<assembly: AssemblyTitle(""ExampleTitle"")>]
[<assembly: AssemblyDescription(""ExampleDescription"")>]
[<assembly: AssemblyCompany(""ExampleCompany"")>]
[<assembly: AssemblyProduct(""ExampleProduct"")>]
[<assembly: AssemblyCopyright(""Copyright ©  2016"")>]


// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[<assembly: ComVisible(false)>]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[<assembly: Guid(""7cff4eba-acd5-45b8-bbcc-a3346c41910c"")>]

// Version information for an assembly consists of the following four values:
//
//       Major Version
//       Minor Version
//       Build Number
//       Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [<assembly: AssemblyVersion(""1.0.*"")>]
[<assembly: AssemblyVersion(""1.0.0.0"")>]
[<assembly: AssemblyFileVersion(""1.0.0.0"")>]
[<assembly: AssemblyInformationalVersion(""1.0.0"")>]

do
    ()
    "

let withoutFsInformationalVersion =
    @"
namespace FSharpAssemblyInfoUtils.AssemblyInfo

open System.Reflection
open System.Runtime.CompilerServices
open System.Runtime.InteropServices

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[<assembly: AssemblyTitle(""ExampleTitle"")>]
[<assembly: AssemblyDescription(""ExampleDescription"")>]
[<assembly: AssemblyCompany(""ExampleCompany"")>]
[<assembly: AssemblyProduct(""ExampleProduct"")>]
[<assembly: AssemblyCopyright(""Copyright ©  2016"")>]


// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[<assembly: ComVisible(false)>]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[<assembly: Guid(""7cff4eba-acd5-45b8-bbcc-a3346c41910c"")>]

// Version information for an assembly consists of the following four values:
//
//       Major Version
//       Minor Version
//       Build Number
//       Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [<assembly: AssemblyVersion(""1.0.*"")>]
[<assembly: AssemblyVersion(""1.0.0.0"")>]
[<assembly: AssemblyFileVersion(""1.0.0.0"")>]

do
    ()
    "

let assemblyInfoCsContent =
    @"
using System.Reflection;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle(""ExampleTitle"")]
[assembly: AssemblyDescription(""ExampleDescription"")]
[assembly: AssemblyCompany(""ExampleCompany"")]
[assembly: AssemblyProduct(""ExampleProduct"")]
[assembly: AssemblyCopyright(""Copyright ©  2014"")]


// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid(""60fcbd4d-0b5a-4daf-bce8-a14c97cac246"")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion(""1.0.*"")]
[assembly: AssemblyVersion(""1.0.0.0"")]
[assembly: AssemblyFileVersion(""1.0.0.0"")]
[assembly: AssemblyInformationalVersion(""1.0.0"")]
    "

let withoutCsInformationalVersion =
    @"
using System.Reflection;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle(""ExampleTitle"")]
[assembly: AssemblyDescription(""ExampleDescription"")]
[assembly: AssemblyCompany(""ExampleCompany"")]
[assembly: AssemblyProduct(""ExampleProduct"")]
[assembly: AssemblyCopyright(""Copyright ©  2014"")]


// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid(""60fcbd4d-0b5a-4daf-bce8-a14c97cac246"")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion(""1.0.*"")]
[assembly: AssemblyVersion(""1.0.0.0"")]
[assembly: AssemblyFileVersion(""1.0.0.0"")]
    "

[<Test>]
let ``parse AssemblyInformationalVersion (C#)`` () =
  let parsed = ParseInformationalVersionString assemblyInfoCsContent

  parsed |> should equal "1.0.0"

[<Test>]
let ``parse AssemblyVersion if AssemblyInformationalVersion missing (C#)`` () =
  let parsed = ParseVersionString withoutCsInformationalVersion

  parsed |> should equal "1.0.0"

[<Test>]
let ``parse AssemblyInformationalVersion (F#)`` () =
  let parsed = ParseInformationalVersionString assemblyInfoFsContent

  parsed |> should equal "1.0.0"

[<Test>]
let ``parse AssemblyVersion if AssemblyInformationalVersion missing (F#)`` () =
  let parsed = ParseVersionString withoutCsInformationalVersion

  parsed |> should equal "1.0.0"
