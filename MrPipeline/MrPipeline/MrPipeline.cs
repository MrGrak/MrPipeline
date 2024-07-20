using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace Game1
{
    public static class MrPipeline
    {
        internal enum ProjectType { DirectX, OpenGL }
        
        public static void Process_Content()
        {
            //first, where is content folder located? read/write to this folder
            string pathToContent = @"C:\Users\Gx000000\Desktop\REPOs\MrPipeline\MrPipeline\MrPipeline\Content\";


            #region Build a mgcb file based on folder contents

            //setup a string builder with a decent size
            StringBuilder SB = new StringBuilder(4096);

            //determine what type of MGCB file we should write
            ProjectType PT = ProjectType.DirectX; //Windows
            bool HiDef = true;
            bool Compress = true;

            //write global properties
            SB.AppendLine(@" ");
            SB.AppendLine(@"#----------------------------- Global Properties ----------------------------#");
            SB.AppendLine(@" ");
            SB.AppendLine(@"/outputDir:bin/$(Platform)");
            SB.AppendLine(@"/intermediateDir:obj/$(Platform)");

            //select directx/windows or assume opengl by default
            if (PT == ProjectType.DirectX) { SB.AppendLine(@"/platform:Windows"); }
            else { SB.AppendLine(@"/platform:DesktopGL"); }

            SB.AppendLine(@"/config:");

            //write hi def property or not
            if (HiDef) { SB.AppendLine(@"/profile:HiDef"); }
            else { SB.AppendLine(@"/profile:Reach"); }

            //write compress property or not
            if (Compress) { SB.AppendLine(@"/compress:True"); }
            else { SB.AppendLine(@"/compress:False"); }

            //write references section
            SB.AppendLine(@" ");
            SB.AppendLine(@"#-------------------------------- References --------------------------------#");
            SB.AppendLine(@" ");

            //write content section
            SB.AppendLine(@" ");
            SB.AppendLine(@"#---------------------------------- Content ---------------------------------#");

            //use directory info to get file.name (so we dont have to trim/split full path filenames)
            DirectoryInfo d = new DirectoryInfo(pathToContent);

            //collect and write all png files
            FileInfo[] PngFiles = d.GetFiles("*.png");
            foreach (FileInfo file in PngFiles)
            {
                SB.AppendLine(@" ");
                SB.AppendLine(@"#begin " + file.Name);
                SB.AppendLine(@"/importer:TextureImporter");
                SB.AppendLine(@"/processor:TextureProcessor");
                SB.AppendLine(@"/processorParam:ColorKeyColor=255,0,255,255");
                SB.AppendLine(@"/processorParam:ColorKeyEnabled=True");
                SB.AppendLine(@"/processorParam:GenerateMipmaps=False");
                SB.AppendLine(@"/processorParam:PremultiplyAlpha=True");
                SB.AppendLine(@"/processorParam:ResizeToPowerOfTwo=False");
                SB.AppendLine(@"/processorParam:MakeSquare=False");
                SB.AppendLine(@"/processorParam:TextureFormat=Color");
                SB.AppendLine(@"/build:" + file.Name);
            }

            //collect and write all wav files
            FileInfo[] WavFiles = d.GetFiles("*.wav");
            foreach (FileInfo file in WavFiles)
            {
                SB.AppendLine(@" ");
                SB.AppendLine(@"#begin " + file.Name);
                SB.AppendLine(@"/importer:WavImporter");
                SB.AppendLine(@"/processor:SoundEffectProcessor");
                SB.AppendLine(@"/processorParam:Quality=Best");
                SB.AppendLine(@"/build:" + file.Name);
            }

            //collect and write all bmp files
            FileInfo[] BmpFiles = d.GetFiles("*.bmp");
            foreach (FileInfo file in BmpFiles)
            {
                SB.AppendLine(@" ");
                SB.AppendLine(@"#begin " + file.Name);
                SB.AppendLine(@"/importer:TextureImporter");
                SB.AppendLine(@"/processor:FontTextureProcessor");
                SB.AppendLine(@"/processorParam:FirstCharacter=");
                SB.AppendLine(@"/processorParam:PremultiplyAlpha=True");
                SB.AppendLine(@"/processorParam:TextureFormat=Color");
                SB.AppendLine(@"/build:" + file.Name);
            }
                        
            //finally, write all text to mgcb file
            string MGCB_filepath = pathToContent + @"Content.mgcb";
            File.WriteAllText(MGCB_filepath, SB.ToString());

            #endregion


            #region Build a Content.cs file based on folder contents

            //reuse string builder from above
            SB.Clear();
            
            //start writing Content.cs c# string
            SB.AppendLine("using Microsoft.Xna.Framework.Audio;");
            SB.AppendLine("using Microsoft.Xna.Framework.Content;");
            SB.AppendLine("using Microsoft.Xna.Framework.Graphics;");
            SB.AppendLine(" ");

            //assume default namespace for game
            SB.AppendLine("namespace Game1");
            SB.AppendLine("{");
            SB.AppendLine("\tpublic static class Assets");
            SB.AppendLine("\t{");

            //write content manager instance
            SB.AppendLine("\t\tpublic static ContentManager CM;");
            SB.AppendLine(" ");

            //write font instances
            foreach (FileInfo file in BmpFiles)
            {
                SB.AppendLine("\t\tpublic static SpriteFont " +
                    Path.GetFileNameWithoutExtension(file.Name) + ";");
            }
            SB.AppendLine(" ");

            //write texture2d instances
            foreach (FileInfo file in PngFiles)
            {
                SB.AppendLine("\t\tpublic static Texture2D " +
                    Path.GetFileNameWithoutExtension(file.Name) + ";");
            }
            SB.AppendLine(" ");

            //write music and soundeffect instances
            foreach (FileInfo file in WavFiles)
            {
                SB.AppendLine("\t\tpublic static SoundEffectInstance " +
                    Path.GetFileNameWithoutExtension(file.Name) + ";");
            }
            SB.AppendLine(" ");
            
            //write load method
            SB.AppendLine("\t\tpublic static void Load()");
            SB.AppendLine("\t\t{");

            //write cautionary comment
            SB.AppendLine("\t\t\t//construct this instance or set its reference here");
            SB.AppendLine("\t\t\t//CM = new ContentManager(Game1.Services, 'Content');");
            SB.AppendLine("\t\t\t//we then load assets into this CM reference below");
            SB.AppendLine(" ");

            //load font instances
            SB.AppendLine("\t\t\t#region Load Fonts");
            SB.AppendLine(" ");
            foreach (FileInfo file in BmpFiles)
            {
                SB.AppendLine("\t\t\t" +
                    Path.GetFileNameWithoutExtension(file.Name) + 
                    " = CM.Load<SpriteFont>(" +
                    '"' + Path.GetFileNameWithoutExtension(file.Name) + '"' + ");");
            }
            SB.AppendLine(" ");
            SB.AppendLine("\t\t\t#endregion");
            SB.AppendLine(" ");

            //load texture2d instances
            SB.AppendLine("\t\t\t#region Load Texture2Ds");
            SB.AppendLine(" ");
            foreach (FileInfo file in PngFiles)
            {
                SB.AppendLine("\t\t\t" +
                    Path.GetFileNameWithoutExtension(file.Name) + 
                    " = CM.Load<Texture2D>(" +
                    '"' + Path.GetFileNameWithoutExtension(file.Name) + '"' + ");");
            }
            SB.AppendLine(" ");
            SB.AppendLine("\t\t\t#endregion");
            SB.AppendLine(" ");

            //load SoundEffectInstances
            SB.AppendLine("\t\t\t#region Load Sound Effects and Music");
            SB.AppendLine(" ");
            SB.AppendLine("\t\t\tSoundEffect src;");
            SB.AppendLine(" ");
            foreach (FileInfo file in WavFiles)
            {
                SB.AppendLine("\t\t\tsrc = CM.Load<SoundEffect>(" +
                    '"' + Path.GetFileNameWithoutExtension(file.Name) + '"' + ");");
                SB.AppendLine("\t\t\t" +
                    Path.GetFileNameWithoutExtension(file.Name) 
                    + " = src.CreateInstance();");
                SB.AppendLine(" ");
            }
            SB.AppendLine(" ");
            SB.AppendLine("\t\t\t#endregion");
            SB.AppendLine(" ");
            
            //close up scopes
            SB.AppendLine("\t\t}");
            SB.AppendLine("\t}");
            SB.AppendLine("}");

            //finally, write all text to mgcb file
            string Assets_filepath = pathToContent + @"Assets.cs";
            File.WriteAllText(Assets_filepath, SB.ToString());

            #endregion


        }
    }
}