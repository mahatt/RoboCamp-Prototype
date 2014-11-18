
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Notes
{

    public class NoteDrawerException : System.Exception
    {
        public NoteDrawerException(string reason) : base(reason) { }
    }

    class NoteRectangle
    {
        public Note note;
        public RectangleF rectangle;

        public NoteRectangle(Note _note, RectangleF _rectangle)
        {
            note = _note;
            rectangle = _rectangle;
        }
    }

    class NoteDrawer
    {
        const uint numberOfLines = 5;
        const float lengthBetweenLines = 10;
        const float heigthBetweenStaves = 100;
        const float stavesMarginsSize = 20;
        const float spaceBetweenNotes = 15;
        const float spaceAfterClef = 10;
        const float widthOfNote = lengthBetweenLines;
        float width = 0;
        float height = 0;
        float widthOfStave = 0;
        float heightOfStave = (numberOfLines - 1) * lengthBetweenLines;

        /*
        const uint numberOfLines = 5;
        const float lengthBetweenLines = 15;
        const float heigthBetweenStaves = 120;
        const float stavesMarginsSize = 20;
        const float spaceBetweenNotes = 20;
        const float spaceAfterClef = 10;
        const float widthOfNote = lengthBetweenLines;
        float width = 0;
        float height = 0;
        float widthOfStave = 0;
        float heightOfStave = (numberOfLines - 1) * lengthBetweenLines;
        */

        List<PointF> stavesLocation;

        List<PointF> NotesLocation;

        List<NoteRectangle> NotesRect;

        List<Note> DrawenNotes;

        // graphics components
        Image bufferImage;
        Graphics bufferGraphics;
        Image GClef;

        public NoteDrawer()
        {
            lock (this)
            {
                stavesLocation = new List<PointF>();
                NotesLocation = new List<PointF>();
                NotesRect = new List<NoteRectangle>();
                DrawenNotes = new List<Note>();
                bufferImage = new Bitmap(1, 1);
                bufferGraphics = Graphics.FromImage(bufferImage);
                Image GClefSource = NotesSimulation.Properties.Resources.GClefPicture;
                GClef = new Bitmap(GClefSource, (int)(heightOfStave * 0.5), (int)(heightOfStave * 1.3));
            }
        }

        public int Draw(Graphics graphics, Color background, Brush brushSheet, Brush brushNote, Note note = null)
        {
            lock (this)
            {
                UpdateDrawer(graphics, background, brushSheet, brushNote);

                // Draw a new Note
                if (null != note)
                {
                    // Draw a new sheet if current one is full
                    if (DrawenNotes.Count >= NotesLocation.Count)
                    {
                        DrawEmptyMusicSheet(graphics, background, brushSheet);
                        DrawenNotes.Clear();
                        NotesRect.Clear();
                    }

                    int noteIndex = DrawenNotes.Count;
                    DrawNote(bufferGraphics, brushNote, NotesLocation[noteIndex], note);
                    DrawenNotes.Add(note);
                }

                graphics.DrawImage(bufferImage, 0, 0);
            }

            return NotesLocation.Count;
        }

        public int DrawNotes(Graphics graphics, Color background, Brush brushSheet, List<Note> notes, List<Color> notesColors)
        {
            // Redraw music sheet if graphics has changes
            if ((graphics.VisibleClipBounds.Width != this.width) || (graphics.VisibleClipBounds.Height != this.height))
            {
                DrawEmptyMusicSheet(graphics, background, brushSheet);
                graphics.DrawImage(bufferImage, 0, 0);
                return NotesLocation.Count;
            }

            // Draw notes
            if ((null != notes) && (null != notesColors) && (notes.Count <= notesColors.Count))
            {
                for (int noteIndex = 0; noteIndex < notes.Count; noteIndex++)
                {
                    Note note = notes[noteIndex];
                    DrawNote(bufferGraphics,
                        new SolidBrush(notesColors[noteIndex]),
                        NotesLocation[noteIndex],
                        note);
                }
                graphics.DrawImage(bufferImage, 0, 0);
            }
            return NotesLocation.Count;
        }

        public void Clear(Graphics graphics, Color background, Brush brushSheet)
        {
            DrawEmptyMusicSheet(graphics, background, brushSheet);
        }

        public void RedrawNote(Graphics graphics, Brush brushNote, Note note, int noteIndex)
        {
            lock (this)
            {
                if (null == note)
                {
                    return;
                }

                if (NotesLocation.Count <= noteIndex)
                {
                    return; // TODO: throw an exception
                }

                // if want to redraw the last note
                if (0 > noteIndex)
                {
                    noteIndex = DrawenNotes.Count - 1;
                }

                DrawNote(bufferGraphics, brushNote, NotesLocation[noteIndex], note);
                graphics.DrawImage(bufferImage, 0, 0);
            }
        }

        public Note GetNoteOnLocation(Point location)
        {
            lock (this)
            {
                for (int i = 0; i < NotesRect.Count; i++)
                {
                    if (NotesRect[i].rectangle.Contains(location))
                    {
                        return NotesRect[i].note;
                    }
                }
                return null;
            }
        }

        private void UpdateDrawer(Graphics graphics, Color background, Brush brushSheet, Brush brushNote)
        {
            // Redraw music sheet if graphics has changes
            if ((graphics.VisibleClipBounds.Width != width) || (graphics.VisibleClipBounds.Height != height))
            {
                DrawEmptyMusicSheet(graphics, background, brushSheet);

                // If there are too many notes and not enough space for them, remove the oldest notes
                int notesToRemove = (int)DrawenNotes.Count - (int)NotesLocation.Count;
                if (notesToRemove > 0)
                {
                    DrawenNotes.RemoveRange(0, notesToRemove);
                }

                NotesRect.Clear();
                // draw old notes
                for (int noteIndex = 0; noteIndex < DrawenNotes.Count; noteIndex++)
                {
                    Note oldNote = DrawenNotes[noteIndex];
                    DrawNote(bufferGraphics, brushNote, NotesLocation[noteIndex], oldNote);
                }
            }
        }

        private void DrawEmptyMusicSheet(Graphics graphics, Color background, Brush brushSheet)
        {
            width = graphics.VisibleClipBounds.Width;
            height = graphics.VisibleClipBounds.Height;
            widthOfStave = width - 2 * stavesMarginsSize;
            // double buffeing
            bufferImage = new Bitmap((int)width, (int)height);
            bufferGraphics = Graphics.FromImage(bufferImage);
            bufferGraphics.Clear(background);

            stavesLocation.Clear();
            NotesLocation.Clear();

            // Calculate new Staves and Note Locations.
            PointF currentStaveLocation = new PointF(stavesMarginsSize, heigthBetweenStaves);
            while (currentStaveLocation.Y + heightOfStave + heigthBetweenStaves <= height)
            {
                stavesLocation.Add(currentStaveLocation);

                PointF currentNoteLocation = new PointF(currentStaveLocation.X + GClef.Width + spaceAfterClef,
                                                        currentStaveLocation.Y);
                while (currentNoteLocation.X + widthOfNote + spaceBetweenNotes < width - stavesMarginsSize)
                {
                    NotesLocation.Add(currentNoteLocation);
                    currentNoteLocation.X += widthOfNote + spaceBetweenNotes;
                }
                currentStaveLocation.Y += heightOfStave + heigthBetweenStaves;
            }

            // print staves
            foreach (PointF staveLocation in stavesLocation)
            {
                DrawStave(bufferGraphics, brushSheet, staveLocation);
            }
        }

        private void DrawNote(Graphics graph, Brush brush, PointF point, Note note)
        {
            /* The starting position is E' (inner index 51).
             * Based on the current note distance from E", we will set it's location. */
            int distanceFromE = note.LineIndex - 51;
            PointF ellipseBase = new PointF(point.X, point.Y - distanceFromE * lengthBetweenLines / 2F);
            SizeF ellipseSize = new SizeF(widthOfNote, lengthBetweenLines);
            RectangleF ellipse = new RectangleF(ellipseBase, ellipseSize);
            RectangleF noteRect = new RectangleF(ellipseBase, ellipseSize);

            // Figure out note's line location
            PointF LineStart, LineEnd;
            bool noteFlagDown = false;
            if (note.LineIndex > 48)
            {
                noteFlagDown = false;
                LineStart = new PointF(ellipse.X, ellipse.Y + lengthBetweenLines / 2F);
                LineEnd = new PointF(LineStart.X, LineStart.Y + 3.5F * lengthBetweenLines);

                noteRect.Y = ellipse.Y;
                noteRect.Height = LineEnd.Y - ellipse.Y;
            }
            else
            {
                noteFlagDown = true;
                LineStart = new PointF(ellipse.X + widthOfNote, ellipse.Y + lengthBetweenLines / 2F);
                LineEnd = new PointF(LineStart.X, LineStart.Y - 3.5F * lengthBetweenLines);

                noteRect.Y = LineEnd.Y;
                noteRect.Height = ellipse.Bottom - LineEnd.Y;
            }
            PointF[] noteLine = { LineStart, LineEnd };


            // For each note length, print matchig ellipse shape & line 
            graph.DrawEllipse(new Pen(brush, 1F), ellipse);
            graph.FillEllipse(brush, ellipse);
            RectangleF innerEllipse;

            switch ((int)(note.Length * 1000))
            {
                case 125:
                    graph.DrawLines(new Pen(brush, 2F), noteLine);
                    if (noteFlagDown)
                    {
                        DrawFlagDown(graph, brush, LineEnd.X, LineEnd.Y, widthOfNote / 1.5F, lengthBetweenLines * 3);
                    }
                    else
                    {
                        DrawFlagUp(graph, brush, LineEnd.X, LineEnd.Y, widthOfNote / 1.5F, lengthBetweenLines * 3);
                    }
                    break;
                case 250:
                    graph.DrawLines(new Pen(brush, 2F), noteLine);
                    break;
                case 375:
                    graph.FillEllipse(brush, ellipse.X + widthOfNote * 1.2F, ellipse.Y + ellipse.Height / 3F, 4, 4);

                    innerEllipse = ellipse;
                    innerEllipse.Height -= 6;
                    innerEllipse.Y += 3;
                    graph.FillEllipse(new SolidBrush(Color.White), innerEllipse);
                    graph.DrawLines(new Pen(brush, 2F), noteLine);
                    break;
                case 500:
                    innerEllipse = ellipse;
                    innerEllipse.Height -= 6;
                    innerEllipse.Y += 3;
                    graph.FillEllipse(new SolidBrush(Color.White), innerEllipse);
                    graph.DrawLines(new Pen(brush, 2F), noteLine);
                    break;
                case 1000:
                    innerEllipse = ellipse;
                    innerEllipse.Height -= 6;
                    innerEllipse.Y += 3;
                    graph.FillEllipse(new SolidBrush(Color.White), innerEllipse);
                    break;
                default:
                    break;
            }

            NotesRect.Add(new NoteRectangle(note, noteRect));

            if (note.IsSharp)
            {
                string sharp = "\u266F";
                Font font = new Font("Ariel", widthOfNote);
                graph.DrawString(sharp, font, brush, new PointF(ellipseBase.X - widthOfNote, ellipseBase.Y - 5));
            }

            // check if need to add vertical small lines for higher/lower notes
            if (point.Y - ellipseBase.Y >= lengthBetweenLines)
            {
                PointF littleLineStart = new PointF(point.X - 2, point.Y);
                PointF littleLineEnd = new PointF(point.X + widthOfNote + 2, point.Y);
                while (littleLineStart.Y - ellipseBase.Y >= lengthBetweenLines)
                {
                    graph.DrawLine(new Pen(brush, 1F), littleLineStart, littleLineEnd);
                    littleLineStart.Y = littleLineEnd.Y = littleLineEnd.Y - lengthBetweenLines;
                }
            }

            float stave_bottom = point.Y + (numberOfLines - 1) * lengthBetweenLines;
            if (ellipse.Bottom - stave_bottom >= lengthBetweenLines)
            {
                stave_bottom += lengthBetweenLines;
                PointF littleLineStart = new PointF(point.X - 2, stave_bottom);
                PointF littleLineEnd = new PointF(point.X + widthOfNote + 2, stave_bottom);
                while (ellipse.Bottom - littleLineStart.Y > 0)
                {
                    graph.DrawLine(new Pen(brush, 1F), littleLineStart, littleLineEnd);
                    littleLineStart.Y = littleLineEnd.Y = littleLineEnd.Y + lengthBetweenLines;
                }
            }
        }

        private void DrawStave(Graphics graph, Brush brush, PointF point)
        {
            Pen pen = new Pen(brush);

            // draw vertical lines, at the begining and end of the stave
            graph.DrawLine(pen,
                point,
                new PointF(point.X, point.Y + heightOfStave));

            graph.DrawLine(pen,
                new PointF(point.X + widthOfStave, point.Y),
                new PointF(point.X + widthOfStave, point.Y + heightOfStave));

            // draw stave's lines
            for (int i = 0; i < numberOfLines; i++)
            {
                graph.DrawLine(pen,
                    new PointF(point.X, point.Y + i * lengthBetweenLines),
                    new PointF(point.X + widthOfStave, point.Y + i * lengthBetweenLines));
            }

            // draw G-clefs
            graph.DrawImage(GClef, new PointF(point.X, point.Y));
        }

        private void DrawFlagDown(Graphics graph, Brush brush, float x, float y, float width, float height)
        {
            PointF[] points = new PointF[5];
            points[0].X = x; points[0].Y = y;
            points[1].X = x + width; points[1].Y = y + height / 2;
            points[2].X = x + width / 3; points[2].Y = y + height;
            points[3].X = x + width * 2 / 3; points[3].Y = y + height / 2;
            points[4].X = x; points[4].Y = y + height / 5;
            graph.FillClosedCurve(brush, points);
        }

        private void DrawFlagUp(Graphics graph, Brush brush, float x, float y, float width, float height)
        {
            DrawFlagDown(graph, brush, x, y, width, -height);
        }
    }
}
