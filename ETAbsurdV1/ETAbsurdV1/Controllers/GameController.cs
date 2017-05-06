using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ETAbsurdV1.Models;
using ETAbsurdV1.Code.Game.RD;
using System.Web.Script.Serialization;

namespace ETAbsurdV1.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private AbsurdEntities db = new AbsurdEntities();
        // GET: Game
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult RD()
        {
            if (User.Identity.Name == "Oblongamous" | User.Identity.Name == "Gummygirlamous" | User.Identity.Name == "AuthorisedUser")
            {
                return View();
            }
            
            return RedirectToAction("Oops", "Error", new { Message = "Upcoming", e = "Not Ready" });
        }


        public JsonResult GetResult(string GameData)
        {            
            //Serialise the information given by User
            var jsSer = new JavaScriptSerializer();
            CurrentGameState CurrentGameData = jsSer.Deserialize<CurrentGameState>(GameData);

            //used Variables
            int X = CurrentGameData.X;
            int Y = CurrentGameData.Y;
            int Z = CurrentGameData.Z;
            string OriginalInformation = CurrentGameData.Information;

            //Set Error to null, if there is an error then put after these lines
            CurrentGameData.Error = null;

            //String Coords for Game Data retrieval
            string CurrentCoords = CurrentGameData.X.ToString() + "," + CurrentGameData.Y.ToString() + "," + CurrentGameData.Z.ToString();
            //Now look at phrase to see what coords need to change if any
            string Instruction = CurrentGameState.CurrentInstruction(CurrentGameData.Instruction);
            //Change Cordinates if instruction matches
            if (Instruction == "North" | Instruction == "South" | Instruction == "East" | Instruction == "West")
            {
                CurrentGameData.X = CurrentGameState.ChangeCoordinates(CurrentGameData.X, Instruction, "X");
                CurrentGameData.Y = CurrentGameState.ChangeCoordinates(CurrentGameData.Y, Instruction, "Y");
                CurrentGameData.Z = CurrentGameState.ChangeCoordinates(CurrentGameData.Z, Instruction, "Z");
            }
            string NewCoords = CurrentGameData.X.ToString() + "," + CurrentGameData.Y.ToString() + "," + CurrentGameData.Z.ToString();
            CurrentGameData.Information = CurrentGameState.NewRoomInformation(NewCoords);

            if(CurrentGameData.Information == "Failed")
            {
                CurrentGameData.Error = "You hit your face on wall, Achieving nothing but mild head pain.";
                //Set information to room previous;
                CurrentGameData.Information = OriginalInformation;
                //Set Coords to same room
                CurrentGameData.X = X;
                CurrentGameData.Y = Y;
                CurrentGameData.Z = Z;
            }                     

            return Json(CurrentGameData);
        }
    }
}