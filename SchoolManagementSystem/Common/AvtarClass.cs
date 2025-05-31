﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace SchoolManagementSystem.Common
{
    public class AvtarClass
    {
        public Image GenerateAvtarImage(String text, Font font, Color textColor, Color backColor)
        {
            //first, create a dummy bitmap just to get a graphics object  
            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            //measure the string to see how big the image needs to be  
            SizeF textSize = drawing.MeasureString(text, font);

            //free up the dummy image and old graphics object  
            img.Dispose();
            drawing.Dispose();

            //create a new image of the right size  
            img = new Bitmap(110, 110);

            drawing = Graphics.FromImage(img);

            //paint the background  
            drawing.Clear(backColor);

            //create a brush for the text  
            Brush textBrush = new SolidBrush(textColor);

            //drawing.DrawString(text, font, textBrush, 0, 0);  
            drawing.DrawString(text, font, textBrush, new Rectangle(28, 20, 200, 110));

            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();

            // img.Save(Server.MapPath("~/Images/" + filename + ".gif"));

            return img;

        }
    }
}