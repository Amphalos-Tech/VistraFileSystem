using System;

namespace VistraFileSystem
{
    [Serializable]
    public class SaveFile
    {
        public SaveFile()
        {
            Stage = GameStage.Tutorial;
            Upgrades = new byte[]{ 0, 0};
            Settings = new byte[] { 100, 100, 100, 100, 0, 1, 1}; //Max volume all, Max Quality, Medium Text Size, Fast Text Speed
        }
        public enum GameStage //Represents the point the user saved at
        {
            Tutorial = 0,
            NeuvistranMeet = 1,
            NaPaMeet = 2,
            Infiltration = 3,
            NeuvistranGeneral = 4,
            Castle = 5,
            NaPaGeneral = 6,
            Epilogue = 7
        }

        //Property to represent ^
        public GameStage Stage { get; set; } 

        //Level
        public byte[] Upgrades { get; set; }

        //Value (Handled more strictly in load so no need for inbuilt identifiers): Master Voice Music Sfx Quality
        public byte[] Settings { get; set; }
    }
}
