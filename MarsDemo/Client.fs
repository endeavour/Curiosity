namespace MarsDemo

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI.Next
open WebSharper.UI.Next.Client
open WebSharper.UI.Next.Html

[<JavaScript>]
module Client =

    let Main () =
        
        let solInput = Var.Create "1"        
        let current = Var.Create "1"
        
        let currentSol =
          current
          |> View.FromVar

        let currentPhotos =
          currentSol
          |> View.MapAsync (fun sol ->
            async {
              let! photos = Mars.get sol
              
              let elements =
                photos
                |> Seq.map (fun (desc, xs) ->
                  [
                    yield h2 [text desc]
                    yield! xs |> Seq.map (fun x -> imgAttr [Attr.Create "src" x.PhotoUrl] [])
                  ])
                |> Seq.concat
                |> Seq.cast<_>

              return div elements
            }
            )

        let solDoc = Doc.Input [] solInput
        let btnDoc = Doc.Button "Go" [] (fun () -> current.Value <- solInput.Value)

        div [
            h1 [text "Mars Rover Images"]
            solDoc
            btnDoc
            currentPhotos |> Doc.EmbedView
        ]
