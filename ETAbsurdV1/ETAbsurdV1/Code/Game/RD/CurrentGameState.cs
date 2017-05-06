using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETAbsurdV1.Code.Game.RD;
using System.Text.RegularExpressions;

namespace ETAbsurdV1.Code.Game.RD
{
    public class CurrentGameState
    {
        public int Id { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public string Instruction { get; set; }
        public string Information { get; set; }     

        public string Error { get; set; }


        public static string NewRoomInformation(string NewCoords)
        {
            List<RoomLibrary> Room = RoomLibrary.Library(NewCoords);
            string RoomInformation = "";
            if (Room.Count != 0)
            {
                foreach (var item in Room)
                {
                    RoomInformation = item.Information;
                }
            }
            else
            {
                RoomInformation = "Failed";
            }       
            return RoomInformation;
        }

        //What the user writes in text box
        public static string CurrentInstruction(string Instruction)
        {
            string[] Instructions = Instruction.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            string InstructionResult = "Null";

            for (var i = 0; i < Instructions.Length; i++)
            {
                if (Regex.IsMatch(@"\b(north|forward)\b", Instructions[i], RegexOptions.IgnoreCase))
                {
                    InstructionResult = "North";
                    break;
                }
                if (Regex.IsMatch(@"\b(south|backward)\b", Instructions[i], RegexOptions.IgnoreCase))
                {
                    InstructionResult = "South";
                    break;
                }
                if (Regex.IsMatch(@"\b(right|east)\b", Instructions[i], RegexOptions.IgnoreCase))
                {
                    InstructionResult = "East";
                    break;
                }
                if (Regex.IsMatch(@"\b(left|west)\b", Instructions[i], RegexOptions.IgnoreCase))
                {
                    InstructionResult = "West";
                    break;
                }
                
            }           
            return InstructionResult;
        }
        
        public static int ChangeCoordinates(int ThisCoord, string Instruction, string Axis)
        {
            if (Instruction == "Null")
            { }
            if (Instruction == "North" & Axis == "Y")
            {
                ThisCoord += 1;                
            }
            if (Instruction == "South" & Axis == "Y")
            {
                ThisCoord -= 1;                
            }
            if (Instruction == "East" & Axis == "X")
            {
                ThisCoord += 1;
            }
            if (Instruction == "West" & Axis == "X")
            {
                ThisCoord -= 1;
            }        
                
            return ThisCoord;
        }        
        
    }
    
}