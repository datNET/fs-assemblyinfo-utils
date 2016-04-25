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
