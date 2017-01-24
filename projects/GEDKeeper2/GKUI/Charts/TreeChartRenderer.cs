﻿/*
 *  "GEDKeeper", the personal genealogical database editor.
 *  Copyright (C) 2009-2016 by Serg V. Zhdanovskih (aka Alchemist, aka Norseman).
 *
 *  This file is part of "GEDKeeper".
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System.Drawing;
using GKCommon;

namespace GKUI.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class TreeChartRenderer
    {
        protected bool fAutoScale;

        protected TreeChartRenderer(bool autoScale)
        {
            fAutoScale = autoScale;
        }

        public abstract void SetTarget(object target);

        public void DrawImage(Image image, float x, float y)
        {
            DrawImage(image, x, y, image.Width, image.Height);
        }

        public abstract void DrawImage(Image image, float x, float y,
                                       float width, float height);

        public abstract int GetTextHeight(string text, Font font);
        public abstract int GetTextWidth(string text, Font font);

        public abstract void DrawString(string text, Font font, Brush brush, float x, float y);

        public abstract void DrawLine(Pen pen, float x1, float y1, float x2, float y2);

        public abstract void DrawRectangle(Pen pen, Color fillColor, float x, float y,
                                           float width, float height);
        public abstract void DrawRoundedRectangle(Pen pen, Color fillColor, float x, float y,
                                                  float width, float height, float radius);
    }
}
