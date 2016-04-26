namespace datNET

module AI =
  open System.Text.RegularExpressions

  type Attr =
  | AssemblyVersion              of string
  | AssemblyFileVersion          of string
  | AssemblyInformationalVersion of string
  | ComVisible                   of bool
    static member GetName (attr: Attr) = attr.GetType().Name

  let getPattern (attrNamePattern: string) =
    sprintf "(\[\<?assembly\: )(%s)(\(\")(.+)(\"\)\>?\])" attrNamePattern
    //       |0________________________________________|
    //       |1_______________||2_||3___||4_||5________|
    //
    // NOTE: The lines above indicate "Groups" in this regex.

  module Parse =
    let getAttrName (line: string) =
      let pattern = getPattern ".*"
      let m = Regex.Match(line, pattern)

      if m.Groups.Count < 6 then None
      elif m.Groups.[2].Captures.Count = 0 then None
      else
        Some (m.Groups.[2].Captures.[0].Value)

    let getAttrValue (line: string) (attrName: string) = Unchecked.defaultof<'a> // TOOD: uhhhhh

  module Resolve =
    let nameToUnionCaseMap =
      [|
        ("AssemblyVersion"              , AssemblyVersion              :> obj) ;
        ("AssemblyFileVersion"          , AssemblyFileVersion          :> obj) ;
        ("AssemblyInformationalVersion" , AssemblyInformationalVersion :> obj) ;
        ("ComVisible"                   , ComVisible                   :> obj) ;
      |]

    let getAttr (attrName: string) (attrVal: 'a) =
      let (_, ctorObj) = Array.find (fun (key, _) -> key = attrName ) nameToUnionCaseMap
      let ctor = ctorObj :?> ('a -> Attr)

      ctor attrVal