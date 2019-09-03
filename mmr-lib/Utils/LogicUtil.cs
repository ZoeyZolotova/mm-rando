using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MMRando.Models;

namespace MMRando.Utils
{
    public static class LogicUtil
    {
        public static string[] GetLogic(string path)
        {
            string file;
            using (StreamReader Req = new StreamReader(File.Open(path, FileMode.Open)))
            {
                file = Req.ReadToEnd();
            }
            return file.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }

        public static string[] GetLogic(LogicMode mode)
        {
            string file;
            switch (mode)
            {
                case LogicMode.Casual:
                    file = Resources.GetTextFile("REQ_CASUAL.txt"); break;
                case LogicMode.GlitchedNoSetups:
                    file = Resources.GetTextFile("REQ_GLITCH_NOSETUPS.txt"); break;
                case LogicMode.GlitchedCommonTricks:
                    file = Resources.GetTextFile("REQ_GLITCH_COMMONTRICKS.txt"); break;
                case LogicMode.Glitched:
                    file = Resources.GetTextFile("REQ_GLITCH.txt"); break;
                default:
                    throw new ArgumentException($"{mode} is not a built-in logic setting"); 
            }

            return file.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }
    }
}
