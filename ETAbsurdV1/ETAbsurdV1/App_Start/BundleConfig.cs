using System.Web;
using System.Web.Optimization;

namespace ETAbsurdV1
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(                        
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.js"));

            bundles.Add(new ScriptBundle("~/bundles/JqueryUI").Include(
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.js",
                        "~/Scripts/Jquery-ui.js",
                        "~/Scripts/Jquery-ui.min.js"));

            bundles.Add(new StyleBundle("~/bundles/JqueryUI/css").Include(
                        "~/Content/Jquery-UI/jquery-ui.css",
                        "~/Content/Jquery-UI/jquery-ui.min.css",
                        "~/Content/Jquery-UI/jquery=ui.structure.css",
                        "~/Content/Jquery-UI/jquery-ui"));

           

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/SiteMobile.css",
                     "~/Content/Home/Animation.css",
                     "~/Content/Navigation.css"));
            //Plug in Scrollbar
            bundles.Add(new ScriptBundle("~/ScrollBar/js").Include(
                        "~/Scripts/Plugins/ScrollBar/perfect-scrollbar.jquery.js",
                        "~/Scripts/Plugins/ScrollBar/perfect-scrollbar.jquery.min.js",
                        "~/Scripts/Plugins/ScrollBar/perfect-scrollbar.js",
                        "~/Scripts/Plugins/ScrollBar/perfect-scrollbar.min.js"));
            bundles.Add(new StyleBundle("~/ScrollBar/css").Include(
                        "~/Content/ScrollBar/perfect-scrollbar.css",
                        "~/Content/ScrollBar/perfect-scrollbar.css"));

            //NewPlugin ScrollBar
            bundles.Add(new ScriptBundle("~/MScrollBar/js").Include(
                        "~/Scripts/Plugins/MScrollBar/jquery.mCustomScrollbar.concat.min.js",
                        "~/Scripts/Plugins/MScrollBar/jquery.mCustomScrollbar.js"));
            bundles.Add(new StyleBundle("~/MScrollBar/css").Include(
                        "~/Scripts/Plugins/MScrollBar/jquery.mCustomScrollbar.min.css",
                        "~/Scripts/Plugins/MScrollBar/jquery.mCustomScrollbar.css"));

            //Plugin DragDealer
            bundles.Add(new ScriptBundle("~/DragDealer/js").Include(
                        "~/Scripts/Plugins/DragDealer/dragdealer.js"));
            bundles.Add(new StyleBundle("~/DragDealer/css").Include(
                        "~/Content/DragDealer/dragdealer.css"));


            //Layout
            bundles.Add(new ScriptBundle("~/Layout/js").Include(
                        "~/Scripts/Common/Common.js",
                        "~/Scripts/Common/Navigation.js"));


            // HomePage
            bundles.Add(new StyleBundle("~/Content/Homepage/css").Include(
                       "~/Content/Home/Index.css"));
            bundles.Add(new ScriptBundle("~/Scripts/Homepage/js").Include(
                      "~/Scripts/Home/Index.js"));
            //SecondHomePage
            bundles.Add(new StyleBundle("~/Content/Homepage2/css").Include(
                    "~/Content/Home/Index2.css"));
            bundles.Add(new ScriptBundle("~/Scripts/Homepage2/js").Include(
                      "~/Scripts/Home/Index2.js"));

            //Login & Register
            bundles.Add(new StyleBundle("~/Content/Login/css").Include(
                       "~/Content/Login/Login.css"));           

            //Account
            bundles.Add(new StyleBundle("~/Content/Account/css").Include(
                       "~/Content/Account/ControlPanel.css"));
            bundles.Add(new ScriptBundle("~/Scripts/Account/js").Include(
                       "~/Scripts/Account/ControlPanel.js"));

            //Forum
            bundles.Add(new StyleBundle("~/Content/Forum/css").Include(
                        "~/Content/Forum/Index.css",
                        "~/Content/Forum/Start.css",
                        "~/Content/Forum/Category.css"));
            bundles.Add(new StyleBundle("~/Content/Forum/Thread/css").Include(
                       "~/Content/Forum/Thread.css"));
            bundles.Add(new ScriptBundle("~/Scripts/Forum/js").Include(
                        "~/Scripts/Forum/Index.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Forum/Start/js").Include(
                        "~/Scripts/Forum/Start.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Forum/Category/js").Include(
                        "~/Scripts/Forum/Category.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Forum/Thread/js").Include(
                        "~/Scripts/Forum/Thread.js"));

            //Forum Edot
            bundles.Add(new ScriptBundle("~/Scripts/Forum/Edit/js").Include(
                        "~/Scripts/Forum/Edit.js"));

            //Blog
            bundles.Add(new StyleBundle("~/Content/Blog/css").Include(
                        "~/Content/Blog/Index.css"));
                        
            bundles.Add(new ScriptBundle("~/Scripts/Blog/js").Include(
                        "~/Scripts/Blog/Index.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Blog/NewBlog/js").Include(
                        "~/Scripts/Blog/NewBlog.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Blog/Blog/js").Include(
                        "~/Scripts/Blog/Blog.js"));
            bundles.Add(new StyleBundle("~/Content/Blog/Blog/css").Include(
                        "~/Content/Blog/Blog.css"));
            bundles.Add(new ScriptBundle("~/Scripts/Blog/Edit.js").Include(
                        "~/Scripts/Blog/EditBlog.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Blog/Bloggers/js").Include(
                        "~/Scripts/Blog/Bloggers.js"));

            //Blog Mobile
            bundles.Add(new StyleBundle("~/Content/Blog/Mobile/css").Include(
                        "~/Content/Blog/BlogMobile.css"));

            //Error
            bundles.Add(new StyleBundle("~/Content/Error/css").Include(
                       "~/Content/Error/Oops.css"));


            //Game Content
            bundles.Add(new StyleBundle("~/Content/Game/Index/css").Include(
                        "~/Content/Game/Index.css"));







            //Game Scripts
            bundles.Add(new ScriptBundle("~/Scripts/Game/Index/js").Include(
                        "~/Scripts/Game/Index.js"));

            //RD Game using Plugin Typed
            bundles.Add(new StyleBundle("~/Content/Game/RDGame/css").Include(
                        "~/Content/Game/RDGame.css",
                        "~/Content/Game/Animations.css"));
            bundles.Add(new ScriptBundle("~/Scripts/Game/RDGame/js").Include(
                        "~/Scripts/Game/Common.js",
                        "~/Scripts/Game/RDGame.js",
                        "~/Scripts/Plugins/typed.js"));



            //Faction Wars Index/Common
            bundles.Add(new ScriptBundle("~/Scripts/Faction/Index/js").Include(
                        "~/Scripts/Faction/Index.js"));

            bundles.Add(new StyleBundle("~/Content/Faction/Index/css").Include(
                        "~/Content/Faction/Index.css"));

            bundles.Add(new StyleBundle("~/Content/Faction/ControlPanel/css").Include(
                        "~/Content/Faction/ControlPanel.css"));
            bundles.Add(new ScriptBundle("~/Scripts/Faction/ControlPanel/js").Include(
                        "~/Scripts/Faction/ControlPanel.js"));




            //puzzles index
            bundles.Add(new StyleBundle("~/Content/Puzzle/Index").Include(
                        "~/Content/Puzzle/Index.css"));
            //Puzzles and Creation
            bundles.Add(new ScriptBundle("~/Scripts/Puzzle/js").Include(
                        "~/Scripts/Puzzle/Puzzle.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Create/js").Include(
                       "~/Scripts/Puzzle//Create.js"));

            bundles.Add(new StyleBundle("~/Content/Puzzle/css").Include(
                        "~/Content/Puzzle/Puzzle.css"));
            bundles.Add(new StyleBundle("~/Content/Create/css").Include(
                        "~/Content/Puzzle/Create.css"));

            bundles.Add(new ScriptBundle("~/Scripts/Creation/js").Include(
                        "~/Scripts/Puzzle/Creation.js"));
            //CrossWord
            bundles.Add(new ScriptBundle("~/Scripts/Crossword/js").Include(
                        "~/Scripts/Puzzle/CrossWord.js",
                        "~/Scripts/Puzzle/CrossWordAlter.js",
                        "~/Scripts/Puzzle/CrossWordSubmit.js"));
            bundles.Add(new StyleBundle("~/Content/CrossWord/css").Include(
                        "~/Content/Puzzle/CrossWord.css"));
            bundles.Add(new StyleBundle("~/Content/CrossWord/solve/css").Include(
                        "~/Content/Puzzle/SolveCrossWord.css"));

            bundles.Add(new ScriptBundle("~/Scripts/Puzzle/Solve/js").Include(
                        "~/Scripts/Puzzle/SolveCrossWord.js"));

            //Contact Section
            bundles.Add(new ScriptBundle("~/Scripts/Contact/js").Include(
                        "~/Scripts/Contact/Contact.js"));
            bundles.Add(new StyleBundle("~/Content/Contact/css").Include(
                        "~/Content/Contact/Contact.css"));




            //APP
            bundles.Add(new ScriptBundle("~/Scripts/App/Gif").Include(
                        "~/Scripts/App/CreateGif.js"));
            bundles.Add(new StyleBundle("~/Content/App/Gif").Include(
                        "~/Content/App/CreateGif.css"));
        }
    }
}
