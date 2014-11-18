using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Notes
{

    /*This class Describes notes and their properties*/
    /* This properties are to be read as xml nodes*/

    public class Note
    {
        public string NoteDescription;
        public short Octave;
        public float Frequency;
        public byte MIDI;
        public byte LineIndex;
        public bool IsSharp;
        public string ABC;
        public int Duration;
        public float Length;

        public Note(string noteDescription,
            short octave,
            float frequency,
            byte midi,
            byte lineIndex,
            bool isSharp = false,
            string abc = "",
            int duration = 0,
            float length = 0.25F)
        {
            NoteDescription = noteDescription;
            Octave = octave;
            Frequency = frequency;
            MIDI = midi;
            LineIndex = lineIndex;
            IsSharp = isSharp;
            ABC = abc;
            Duration = duration;
            Length = length;
        }
    }


    /*Compare to Notes*/
    /**/
    public class NoteComparer : IComparer<Note>
    {
        public int Compare(Note x, Note y)
        {
            return x.Frequency.CompareTo(y.Frequency);
        }
    }



    class NotesDB
    {
        List<Note> NotesTable;
        NoteComparer NotesComparer;

        public NotesDB()
        {
            NotesTable = CreateNotesTable();
            NotesComparer = new NoteComparer();
        }

        public Note GetNote(byte MIDI)
        {
            if (MIDI < 0 || MIDI > 127)
            {
                return null;
            }
            else
            {
                return NotesTable[MIDI];
            }
        }

        public Note GetNoteByABC(string ABC)
        {
            foreach (Note note in NotesTable)
            {
                if (note.ABC == ABC)
                {
                    return note;
                }
            }
            return null;
        }


        private List<Note> CreateNotesTable()
        {
            List<Note> notesTable = new List<Note>();
            string[] notesDescription = { "C", "C#", "D", 
                                          "D#", "E", "F",
                                          "F#", "G", "G#",
                                          "A", "A#", "B" };

            bool[] sharp = { false, true, false,
                             true, false, false,
                             true, false, true,
                             false, true, false };

            // search for Helmholtz pitch notation
            int germanNotataionMarks = -8; // C,,, C,, C, C c c' c'' c''' c'''' c''''' c''''''
            string noteDescription = "";
            int octave = -1;
            double frequency = 0;
            byte midi = 0;
            byte lineIndex = 0;
            bool isSharp = false;
            string abc = "";
            int noteIndex = 0;

            for (midi = 0; midi < 128; midi++)
            {
                frequency = 440.0 * Math.Pow(2, (midi - 69.0) / 12.0);
                noteIndex = midi % notesDescription.Length;
                octave = midi / notesDescription.Length - 1;
                noteDescription = notesDescription[noteIndex];
                isSharp = sharp[noteIndex];

                //ABC
                if (isSharp)
                {
                    abc = "^" + noteDescription[0];
                }
                else
                {
                    abc = noteDescription;
                }

                if (noteIndex == 0)
                {
                    germanNotataionMarks++;
                }
                if (germanNotataionMarks < 0)
                {
                    abc = abc.ToUpper();
                    for (int i = 0; i < Math.Abs(germanNotataionMarks) - 1; i++)
                    {
                        abc += ",";
                    }
                }
                else
                {
                    abc = abc.ToLower();
                    for (int i = 0; i < Math.Abs(germanNotataionMarks); i++)
                    {
                        abc += "'";
                    }
                }

                notesTable.Add(new Note(noteDescription,
                                        (short)octave,
                                        (float)frequency,
                                        (byte)midi,
                                        lineIndex,
                                        isSharp,
                                        abc));
                if (!isSharp)
                {
                    lineIndex++;
                }
            }

            return notesTable;
        }

        public Note FindNote(float frequency)
        {
            Note note = new Note("", 0, frequency, 0, 0);
            int index = NotesTable.BinarySearch(note, NotesComparer);

            if (0 <= index)
            {
                return NotesTable[index];
            }

            index = ~index;

            if ((0 <= index) && (index < NotesTable.Count))
            {
                if (index == 0)
                {
                    return NotesTable[0];
                }
                else if (Math.Abs(NotesTable[index - 1].Frequency - frequency) >= Math.Abs(NotesTable[index].Frequency - frequency))
                {
                    return NotesTable[index];
                }
                else
                {
                    return NotesTable[index - 1];
                }
            }
            return new Note("", 0, 0, 0, 0);
        }

    }
}