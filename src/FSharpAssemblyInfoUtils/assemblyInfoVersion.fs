namespace datNET

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


  let private _IsSingleLineComment line = "^//" >** line

  let private _IsAssemblyAttribute line =
    "^\[\<?assembly\:.+\(.+\)\>?\]$" >** line

  let private _IsVersionAttribute line =
    (not (_IsSingleLineComment line)) && ("Version\(.+\)\>?\]$" >** line)

  let private _IsAssemblyVersionAttribute line =
    let pattern = "AssemblyVersion\(\"(.+)\"\)"
    (not (_IsSingleLineComment line)) && (pattern >** line)

  let private _IsAssemblyFileVersionAttribute line =
    let pattern = "AssemblyFileVersion\(\"(.+)\"\)"
    (not (_IsSingleLineComment line)) && (pattern >** line)

  let private _IsAssemblyInformationalVersionAttribute line =
    let pattern = "AssemblyInformationalVersion\(\"(.+)\"\)"
    (not (_IsSingleLineComment line)) && (pattern >** line)

  let private _GetVersionString pattern line =
    let m = Regex.Match(line, pattern)

    // FIXME: This is garbage, but I just need it to work for now. Please come
    // back to this at some point
    if m.Groups.Count < 2
      then String.Empty
    elif m.Groups.[1].Captures.Count = 0
      then String.Empty
    else
      m.Groups.[1].Captures.[0].Value

  let _ValidateAnyInformationalVersionAttributesFound lines =
    if Seq.isEmpty lines then raise (new Exception("Missing required AssemblyInformationalVersion attribute"))
    else lines

  let ParseInformationalVersionStringFromLines lines =
    lines
    |> Seq.map trim
    |> Seq.filter _IsAssemblyInformationalVersionAttribute
    |> _ValidateAnyInformationalVersionAttributesFound
    |> Seq.head
    |> (_GetVersionString _assemblyInformationalVersionPattern)

  let ParseInformationalVersionString (str: string) =
    str.Split ( [| Environment.NewLine |], StringSplitOptions.None )
    |> ParseInformationalVersionStringFromLines

  let GetAssemblyInformationalVersionString filePath =
    File.ReadAllLines(filePath)
    |> ParseInformationalVersionStringFromLines

  // TODO: Clean this up ======================================================

  let _SetAttributeParametersValue attributeName line (attributeValue: string) =
    let pattern = String.Format(@"({0}\("").+(""\))", [| attributeName |])
    System.Text.RegularExpressions.Regex.Replace(line, pattern, "${1}" + attributeValue + "${2}")

  // Note: the type annotations on these are temporary
  let private _SetAssemblyVersionValue line value =
    _SetAttributeParametersValue "AssemblyVersion" line value

  let private _SetAssemblyFileVersionValue line value =
    _SetAttributeParametersValue "AssemblyFileVersion" line value

  let private _SetAssemblyInformationalVersionValue line value =
    _SetAttributeParametersValue "AssemblyInformationalVersion" line value

  let _SetVersionValue matchLine mutateLine (fileContents: string) versionString =
    fileContents.Split ( [| Environment.NewLine |], StringSplitOptions.None )
    |> Seq.map trim
    |> Seq.map (fun line ->
          if matchLine line then mutateLine line versionString
          else line
      )

  let SetAssemblyVersion = _SetVersionValue _IsAssemblyVersionAttribute _SetAssemblyVersionValue
  let SetAssemblyFileVersion = _SetVersionValue _IsAssemblyFileVersionAttribute _SetAssemblyFileVersionValue
  let SetAssemblyInformationalVersion = _SetVersionValue _IsAssemblyInformationalVersionAttribute _SetAssemblyInformationalVersionValue