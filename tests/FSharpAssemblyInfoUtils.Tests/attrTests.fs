module attrTests

open FsUnit
open NUnit.Framework
open System
open System.IO
open datNET.AI
open datNET.AssemblyInfo

[<Test>]
let ``AssemblyVersion key gets you the UnionCase`` () =
  let attrName  = "AssemblyVersion"
  let attrValue = "1.2.3.0"
  let attr      = Resolve.getAttr attrName attrValue

  should be True <|
    match attr with
    | AssemblyVersion _ -> true
    | _ -> false

[<Test>]
let ``AssemblyFileVersion key gets you the UnionCase`` () =
  let attrName  = "AssemblyFileVersion"
  let attrValue = "1.2.3.0"
  let attr      = Resolve.getAttr attrName attrValue

  should be True <|
    match attr with
    | AssemblyFileVersion _ -> true
    | _ -> false

[<Test>]
let ``AssemblyInformationalVersion key gets you the UnionCase`` () =
  let attrName  = "AssemblyInformationalVersion"
  let attrValue = "1.2.3"
  let attr      = Resolve.getAttr attrName attrValue

  should be True <|
    match attr with
    | AssemblyInformationalVersion _ -> true
    | _ -> false

[<Test>]
let ``ComVisible key gets you the UnionCase`` () =
  let attrName = "ComVisible"
  let attrValue = true
  let attr = Resolve.getAttr attrName attrValue

  should be True <|
    match attr with
    | ComVisible _ -> true
    | _ -> false


[<Test>]
let ``getAttrName works (AssemblyVersion)`` () =
  let line = @"[<assembly: AssemblyVersion(""0.2.0.0"")>]"

  datNET.AI.Parse.getAttrName line
  |> should equal (Some "AssemblyVersion")

[<Test>]
let ``getAttrName works (AssemblyFileVersion)`` () =
  Parse.getAttrName @"[<assembly: AssemblyFileVersion(""0.2.0.0"")>]"
  |> should equal (Some "AssemblyFileVersion")

[<Test>]
let ``getAttribute works (AssemblyInformationalVersion)`` () =
  Parse.getAttrName @"[<assembly: AssemblyInformationalVersion(""0.2.0"")>]"
  |> should equal (Some "AssemblyInformationalVersion")

[<Test>]
let ``getAttribute works (N/A)`` () =
  Parse.getAttrName @"// General Information about an assembly is controlled through the following"
  |> should equal None