using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SMS_Billboard
{


    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //-----------------Variables----------------
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont TextObject;
        Vector2 TextPosition;
        string smsFilePath = @"C:\Users\User\Documents\Projects\Java Workspace\SMS Billboard\texts.txt";
        string sms;
        int charsPerLine = 30;
        int screenWidth = 800;
        int screenHeight = 600;


        //---------------------------------------------------
        //--------------------Constructor--------------------
        //---------------------------------------------------
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.PreferMultiSampling = false;
            graphics.IsFullScreen = true;
        }



        //-------------------------------------------------
        //-------------------Initialize--------------------
        //-------------------------------------------------
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }



        //---------------------------------------------------
        //--------------------LoadContent--------------------
        //---------------------------------------------------
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            TextObject = Content.Load<SpriteFont>("TextObject");
            TextPosition = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);
        }



        //-----------------------------------------------------
        //--------------------UnloadContent--------------------
        //-----------------------------------------------------
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }



        //---------------------------------------------
        //--------------------Update--------------------
        //----------------------------------------------
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }



        //--------------------------------------------
        //--------------------Draw--------------------
        //--------------------------------------------
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            sms = GetSMS();
            Vector2 FontOrigin = TextObject.MeasureString(sms) / 2;

            spriteBatch.DrawString(TextObject, sms, TextPosition, Color.White, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);

            spriteBatch.End();
            base.Draw(gameTime);
        }



        //---------------------------------------------
        //-------------------GetSMS--------------------
        //---------------------------------------------
        public string GetSMS()
        {
            string textString = "";
            //int numLines = 0;
            //int i = 0;

            try
            {
                StreamReader sr = new StreamReader(smsFilePath);

                while (!sr.EndOfStream)
                {
                    textString = sr.ReadLine();
                }
                sr.Close();
            }
            catch (IOException e)
            {
                textString = e.Message;
                System.Threading.Thread.Sleep(200);
            }
            sms = WordWrap(textString, charsPerLine);

            return sms;
                
                /*
                numLines = (textString.Length / charsPerLine) + 1;

                if (numLines > 1)
                {
                    for (int j = 0; j < numLines + 1; j++)
                    {
                        if (textString.Length - i < charsPerLine)
                        {
                            for (i = j * charsPerLine; i < textString.Length; i++)
                            {
                                sms = sms + textString.ElementAt(i);
                            }
                        }
                        else
                        {
                            while (i < (charsPerLine * j) + charsPerLine)
                            {
                                sms = sms + textString.ElementAt(i);
                                i++;
                            }
                            sms = sms + "\n";
                        }
                    }
                }
                else
                {
                    sms = textString;
                }
                 */
        }



        //------------------------------------------------
        //--------------------WordWrap--------------------
        //------------------------------------------------
        public static string WordWrap(string text, int width)
        {
            int pos, next;
            StringBuilder sb = new StringBuilder();

            // Lucidity check
            if (width < 1)
                return text;

            // Parse each line of text
            for (pos = 0; pos < text.Length; pos = next)
            {
                // Find end of line
                int eol = text.IndexOf(Environment.NewLine, pos);
                if (eol == -1)
                    next = eol = text.Length;
                else
                    next = eol + Environment.NewLine.Length;

                // Cothpy is line of text, breaking into smaller lines as needed
                if (eol > pos)
                {
                    do
                    {
                        int len = eol - pos;
                        if (len > width)
                            len = BreakLine(text, pos, width);
                        sb.Append(text, pos, len);
                        sb.Append(Environment.NewLine);

                        // Trim whitespace following break
                        pos += len;
                        while (pos < eol && Char.IsWhiteSpace(text[pos]))
                            pos++;
                    } while (eol > pos);
                }
                else sb.Append(Environment.NewLine); // Empty line
            }
            return sb.ToString();
        }



        //-------------------------------------------------
        //--------------------breakLine--------------------
        //-------------------------------------------------
        private static int BreakLine(string text, int pos, int max)
        {
            // Find last whitespace in line
            int i = max;
            while (i >= 0 && !Char.IsWhiteSpace(text[pos + i]))
                i--;

            // If no whitespace found, break at maximum length
            if (i < 0)
                return max;

            // Find start of whitespace
            while (i >= 0 && Char.IsWhiteSpace(text[pos + i]))
                i--;

            // Return length of text before whitespace
            return i + 1;
        }


    }

}


