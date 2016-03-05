namespace FSharpUtil

module AssemblyInfo =
  open Fake
  open Fake.FileUtils
  open Fake.StringHelper
  open System
  open System.IO
  open System.Text.RegularExpressions

  let private _versionStringPattern = "\(\"(.+)\"\)"

  let private _GetAssemblyAttributeValuePattern attributeName =
    String.Format("^\[\<?assembly\: {0}\(\"(.+)\"\)\>?\]$", [| attributeName |]);

  let private _assemblyVersionPattern =
    _GetAssemblyAttributeValuePattern "AssemblyVersion"

  let private _assemblyFileVersionPattern =
    _GetAssemblyAttributeValuePattern "AssemblyFileVersion"

  let private _assemblyInformationalVersionPattern =
    _GetAssemblyAttributeValuePattern "AssemblyInformationalVersion"

  let private _IsSingleLineComment line =
    "^//" >** line

  let private _IsAssemblyAttribute line =
    "^\[\<?assembly\:.+\(.+\)\>?\]$" >** line

  let private _IsVersionAttribute line =
    (not (_IsSingleLineComment line)) && ("Version\(.+\)\>?\]$" >** line)

  let private _IsAssemblyInformationalVersionAttribute line =
    let pattern = "AssemblyInformationalVersion\(\"(.+)\"\)"
    (not (_IsSingleLineComment line)) && (pattern >** line)

  let private _GetVersionString pattern line =
    let m = Regex.Match(line, pattern);
    // FIXME: This is garbage, but I just need it to work for now. Please come
    // back to this at some point
    m.Groups.[1].Captures.[0].Value

  let ParseInformationalVersionStringFromLines lines =
    lines
    |> Seq.map trim
    |> Seq.filter _IsAssemblyInformationalVersionAttribute
    |> Seq.head
    |> (_GetVersionString _assemblyInformationalVersionPattern)

  let ParseInformationalVersionString (str: string) =
    str.Split [| '\n' |]
    |> ParseInformationalVersionStringFromLines

  let GetAssemblyInformationalVersionString filePath =
    File.ReadAllLines(filePath)
    |> ParseInformationalVersionStringFromLines