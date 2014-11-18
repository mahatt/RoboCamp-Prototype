using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Notes;

namespace NotesIO
{
    static public class AbcParser
    {
        static public RecordingData AbcToNotes(string abcFilePath)
        {
            RecordingData recordingData = new RecordingData();
            NotesDB notesDB = new NotesDB();

            recordingData.Notes = new List<Note>();
            string[] lines = File.ReadAllLines(abcFilePath);

            foreach (string line in lines)
            {
                ParseLine(recordingData, line, notesDB);
            }
            
            return recordingData;
        }



        static private void ParseLine(RecordingData recordingData, string line, NotesDB notesDB)
        {
            if (!ParseHeaderLine(recordingData, line))
            {
                ParseNotesLine(recordingData, line, notesDB);
            }
        }

        static private void ParseNotesLine(RecordingData recordingData, string line, NotesDB notesDB)
        {
            string chord = "";
            string notesLetters = "ABCDEFGabcdefg";
            string accidentals = "_=^";
            string fractions = "0123456789/";

            bool isSharp = false;
            bool isFlat = false;

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '"') // it's a chord
                {
                    chord = "";
                    i++;
                    while ((i < line.Length) && (line[i] != '"'))
                    {
                        chord += line[i];
                        i++;
                    }
                    continue;
                }
                else if (notesLetters.Contains(line[i])) // it's a note
                {
                    int noteIndex = i;
                    string noteLength = "";
                    string abcOctavesSymbols = ",'";
                    string noteABC = "";
                    float length = 0;

                    // see if the note has , or  before / after
                    noteABC += line[i];
                    if ((i + 1 < line.Length) && (abcOctavesSymbols.Contains(line[i + 1])))
                    {
                        i++;
                        while ((i < line.Length) && (abcOctavesSymbols.Contains(line[i])))
                        {
                            noteABC += line[i];
                            i++;
                        }
                        i--;
                    }

                    // see if the note has a fraction for timing
                    if ((i + 1 < line.Length) && (fractions.Contains(line[i + 1])))
                    {
                        i++;
                        while ((i < line.Length) && (fractions.Contains(line[i])))
                        {
                            noteLength += line[i];
                            i++;
                        }
                        i--;
                    }

                    // Calculate note Length
                    if (noteLength.Length > 0)
                    {
                        length = ParseNoteLength(noteLength);
                    }
                    else
                    {
                        length = 1;
                    }

                    // get note by ABC
                    Note note = notesDB.GetNoteByABC(noteABC);
                    if (null != note)
                    {
                        // note DUration?
                        note.Length = length * recordingData.DefaultNoteLength;
                        note.IsSharp = isSharp;
                        note.Duration = (int)(note.Length * 1000F);
                        recordingData.Notes.Add(note);
                    }

                }
                else if (accidentals.Contains(line[i]))
                {
                    if (line[i] == '^')
                    {
                        isSharp = true;
                    }
                    if (line[i] == '_')
                    {
                        isFlat = true;
                    }
                }
                else
                {
                    isSharp = false;
                    isFlat = false;
                }
            }
        }

        static private float ParseNoteLength(string noteLength)
        {
            char x, y;
            float a, b;
            string[] fraction = noteLength.Trim().Split('/');

            if (fraction[0].Length == 0)
            {
                a = 1;
            }
            else
            {
                x = fraction[0][0];
                a = x - 48;
            }

            if ((fraction.Length == 1) || (fraction[1].Length == 0))
            {
                b = 1;
            }
            else
            {
                y = fraction[1][0];
                b = y - 48;
            }

            return a / b;
        }

        static private bool ParseHeaderLine(RecordingData recordingData, string line)
        {
            // if it's a comment. ignore
            if (0 < line.Length && line[0] == '%')
            {
                return true;
            }

            if (line.Length < 2)
            {
                return false;
            }

            if (line[1] == ':')
            {
                char header = line[0];
                string content = line.Remove(0, 2);

                switch (header)
                {
                    case 'A': // Author of Lyrics, ignore
                        break;
                    case 'B': // Book, ignore
                        break;
                    case 'C': // Composer
                        recordingData.Composer = content;
                        break;
                    case 'D': // Discography, ignore
                        break;
                    case 'F': // File URL, ignore
                        break;
                    case 'H': // History, ignore
                        break;
                    case 'I': // Instruction, ignore
                        break;
                    case 'K': // Key [clef] [middle-x] - TODO
                        break;
                    case 'L': // Note length unit
                        recordingData.DefaultNoteLength = ParseNoteLength(content);
                        break;
                    case 'M': // line is the meter. M:2/4 is two/four time;
                        //M:6/8 is jig time, etc. M:C and M:C| have the obvious meanings - TODO
                        break;
                    case 'm': // macro, ignore
                        break;
                    case 'N': // Notes, ignore? - TODO
                        break;
                    case 'O': // Origin, ignore
                        break;
                    case 'P': // Parts, ignore
                        break;
                    case 'Q': // tempo, ignore
                        break;
                    case 'R': // Rhythm, ignore
                        break;
                    case 'r': // remark, ignore
                        break;
                    case 'S': // Source, ignore
                        break;
                    case 's': // Symbol line, ignore
                        break;
                    case 'T': // Title
                        recordingData.Title = content;
                        break;
                    case 'U': // User defined, ignore
                        break;
                    case 'V': // Voice, ignore
                        break;
                    case 'W': // Words, ignore
                        break;
                    case 'X': // Reference number, ignore
                        break;
                    case 'Z': // Transcriber, ignore
                        break;
                    case '|': // repeat mark, ignore
                        return false;
                    default:
                        // thorw exception
                        break;
                }
                return true;
            }

            return false;
        }
    }
}
