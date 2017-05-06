using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETAbsurdV1.Code.Game.RD
{
    public class RoomLibrary
    {
        public string RoomCoords { get; set; }
        public string Information { get; set; }  

        public string Character { get; set; }
        public string EnterSpeech { get; set; }

        public string Item { get; set; }  
        public string ItemExamine { get; set; }
        public string ItemPick { get; set; }
        public string ItemDrop { get; set; } 

        public static List<RoomLibrary> Library(string NewCoords)
        {
            List<RoomLibrary> ThisRoom = new List<RoomLibrary>();
            List<RoomLibrary> Rooms = new List<RoomLibrary>();

            // ROOM 1
            RoomLibrary R001 = new RoomLibrary();
            R001.RoomCoords = "10,10,10";
            R001.Information = "waving sarcastically, smiling as the cell door closes... Instantly it re-opens. Reamerging, maybe it was just a scare tactic? No matter "
                               + "northward will lead me back to my quaters.";   

            // ROOM 2
            RoomLibrary R002 = new RoomLibrary();
            R002.RoomCoords = "10,11,10";
            R002.Information = "A Hallway of Sorts, small robotic creatures roaming about. They seem to be wearing John Wayne paraphonalia. To to left a door with 'OLYMPUS ESTILO' posted "
                                + "printed to it.";

            RoomLibrary R0021 = new RoomLibrary();
            R0021.RoomCoords = "9,11,10";
            R0021.Information = "Pitch black... even with the door wide open nothing can be made out an identified.";
            // ROOM 3
            RoomLibrary R003 = new RoomLibrary();
            R003.RoomCoords = "10,12,10";
            R003.Information = "A Hallway of Sorts.";

            RoomLibrary R004 = new RoomLibrary();
            R003.RoomCoords = "10,13,10";
            R003.Information = "A T junction, standing up by the wall is a cannon styled gun. Smeared in... what could be vindaloo sauce on the wall is is 'Transmographawhatnow?'."
                                + " It has to be what you say when this beast hits something.";
            R003.Item = "Bazooka";
            R003.ItemExamine = "A Military grey body and a long neck, capable of holding a bowling ball. It certainly is no tool, this was designed to be a complete"
                                + " douche to whomever it hits before they die.";
            R003.ItemPick = "Got some weight behind it this is a two hander legs bent sorta pick up but managed it, looks like a slot for ammo on the side but its empty";
            R003.ItemDrop = "hmm... Probably should of put it down gently, it could of broke.";

            RoomLibrary R0041 = new RoomLibrary();
            R0041.RoomCoords = "11,13,10";
            R0041.Character = "Toaster";
            R0041.EnterSpeech = "Howdy!, Your the first footsteps I've heard that don't sound like murder";
            R0041.Information = "";



            Rooms.Add(R001);
            Rooms.Add(R002); Rooms.Add(R0021);
            Rooms.Add(R003);
            Rooms.Add(R004); Rooms.Add(R0041);




            foreach (var item in Rooms)
            {
                if(item.RoomCoords == NewCoords)
                {
                    ThisRoom.Add(item);
                    break;
                }                
            }

            return ThisRoom;
        }

    }
}