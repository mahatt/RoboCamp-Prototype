using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Wave
{
    static class WaveGraphics
    {
        // TODO: add documenatation
        static public void DrawGraph(Graphics graph,
                                        int[] graphSamples,
                                        int maxAmplitude,   // TODO: find a different name for this varible
                                        Color background,
                                        Pen foreground,
                                        int numberOfSamplesToDraw)
        {
            int width = (int)graph.VisibleClipBounds.Width;
            int height = (int)graph.VisibleClipBounds.Height;
            float stretch = 0;

            //TODO: maybe draw polygon?!

            // use double buffering, for better performance
            Image bufferImg = new Bitmap(width, height);
            Graphics bufferImgGraphics = Graphics.FromImage(bufferImg);
            bufferImgGraphics.Clear(background);

            numberOfSamplesToDraw = Math.Min(numberOfSamplesToDraw, graphSamples.Length);
            stretch = (float)width / (float)numberOfSamplesToDraw;

            if (maxAmplitude < graphSamples.Max())
            {
                maxAmplitude = graphSamples.Max();
            }

            int a = 0;
            int b = height - (int)(height * graphSamples[0] / maxAmplitude);
            /*
            for (int i = 0; i < numberOfSamplesToDraw - 1; i++)
            {
                a = b;
                b = height - (int)(height * graphSamples[i + 1] / maxAmplitude);
                bufferImgGraphics.DrawLine(foreground, i * stretch, a - 1, (i + 1) * stretch, b - 1);
            }
            */
            PointF[] points = new PointF[numberOfSamplesToDraw];

            for (int i = 0; i < numberOfSamplesToDraw-1; i++)
            {
                a = b;
                b = height - (int)(height * graphSamples[i + 1] / maxAmplitude);
                //bufferImgGraphics.DrawLine(foreground, i * stretch, a - 1, (i + 1) * stretch, b - 1);
                points[i].X = i * stretch;
                points[i].Y = a - 1;
            }

            bufferImgGraphics.DrawBeziers(foreground, points);
            graph.DrawImage(bufferImg, 0, 0);
        }
    }
}
