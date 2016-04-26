module Tests

open System
open NUnit.Framework
open FsUnit
open datNET.AssemblyInfo
open System.IO

let _readDataFile fileName =
  let dataDir = Path.Combine [| Directory.GetCurrentDirectory() ; ".." ; ".." ; "data" |]
  let filePath = Path.Combine [| dataDir ; fileName |]
  
  File.ReadAllText filePath

let assemblyInfoFsContent = _readDataFile "AssemblyInfo.fs"
let withoutFsInformationalVersion = _readDataFile "AssemblyInfoNoInformationalVersion.fs"

let assemblyInfoCsContent = _readDataFile "AssemblyInfo.cs"
let withoutCsInformationalVersion = _readDataFile "AssemblyInfoNoInformationalVersion.cs"

[<Test>]
let ``parse AssemblyInformationalVersion (C#)`` () =
  ParseInformationalVersionString assemblyInfoCsContent
  |> should equal "1.0.0"

[<Test>]
let ``throw an exception if AssemblyInformationalVersion missing (C#)`` () =
  (fun() -> ParseInformationalVersionString withoutCsInformationalVersion |> ignore)
  |> should throw typeof<System.Exception>

[<Test>]
let ``parse AssemblyInformationalVersion (F#)`` () =
  ParseInformationalVersionString assemblyInfoFsContent
  |> should equal "1.0.0"

[<Test>]
let ``throw an exception if AssemblyInformationalVersion missing (F#)`` () =
  (fun() -> ParseInformationalVersionString withoutFsInformationalVersion |> ignore)
  |> should throw typeof<System.Exception>

//let version = datNET.AInfo.AssemblyVersion("1.0.0.0")
//let fileVersion = datNET.AInfo.AssemblyFileVersion("1.0.0.0")
//let informationalVersion = datNET.AInfo.AssemblyInformationalVersion("1.0.0")
//
//let printItOut attribute =
//  let result =
//    match attribute with
//    | datNET.AInfo.AssemblyVersion verStr -> "av: " + verStr
//    | datNET.AInfo.AssemblyFileVersion verStr -> "afv: " + verStr
//    | datNET.AInfo.AssemblyInformationalVersion verStr -> "aiv: " + verStr
//
//  printfn "%s" (datNET.AInfo.getAttributePattern attribute)
//
//printItOut version
//printItOut fileVersion
//printItOut informationalVersion
//
//let result =
//  datNET.AInfo.modifyAssemblyInfoFile "F:\\420\\blaze\\it"
//    [|
//      datNET.AInfo.AssemblyVersion, fun attrVal -> attrVal + " [UPDATED]" ;
//      datNET.AInfo.AssemblyFileVersion, fun attrVal -> attrVal + " [UPDATED]" ;
//      datNET.AInfo.AssemblyInformationalVersion, fun attrVal -> attrVal + " [UPDATED]" ;
//
//      // TODO: Figure out how to do this without blowing up (it's mad about string vs bool)
//      // datNET.AInfo.ComVisible, fun v -> false ;
//    |]
//
//printfn "%s" result