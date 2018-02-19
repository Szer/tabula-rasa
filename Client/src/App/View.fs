module App.View

open App.Types

open Fable.Helpers.React
open Fable.Helpers.React.Props

let menuItem label (page: Option<Page>) currentPage dispatcher =
    div
      [ classList  
         [ "menu-item", true
           "menu-item-selected", page = currentPage ] 
        OnClick (fun _ -> dispatcher (NavigateTo page)) ]
      [ str label ]

let sidebar state dispatcher =
  aside
    [ ClassName "fit-parent child-space"; Style [ TextAlign "center" ] ]
    [ div
        [ Style [ TextAlign "center" ] ]
        [ h3 [ Style [ Color "white" ] ] [ str state.BlogInfo.Value.Name ]
          br []
          img [ ClassName "profile-img"; Src state.BlogInfo.Value.ProfileImageUrl ] ]
      div
        [ ClassName "quote" ]
        [ str state.BlogInfo.Value.About ]
      
      menuItem "Posts" (Some (Posts Posts.Types.Page.AllPosts)) state.CurrentPage dispatcher
      menuItem "About" (Some Page.About) state.CurrentPage dispatcher ]

let main state dispatch = 
    match state.CurrentPage with
    | Some Page.About -> 
        About.View.render()
    | Some (Posts postsPage) -> 
        Posts.View.render postsPage state.Posts (PostsMsg >> dispatch)
    | Some (Admin adminPage) -> 
        Admin.View.render adminPage state.Admin (AdminMsg >> dispatch)
    | None -> 
        div [ ] [ ]

let render state dispatch =
  if state.LoadingBlogInfo 
  then div [ ] [ ]
  elif not state.LoadingBlogInfo && state.BlogInfo.IsNone 
  then h1 [ ] [ str "Error loading initial blog data" ]
  else
  div
    [ ]
    [ div
        [ ClassName "sidebar" ]
        [ sidebar state dispatch ]
      div
        [ ClassName "main-content" ]
        [ main state dispatch ] ]