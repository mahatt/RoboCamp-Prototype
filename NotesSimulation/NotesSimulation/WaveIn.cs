using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Win32;
using System.Runtime.InteropServices;
using System.Threading;
using PInvokeLibrary;
using System.IO;

namespace Wave
{
    public class WaveInException : System.Exception
    {
        public WaveInException(string reason) : base(reason) { }
    }

    public class WaveIn : IDisposable
    {
        const uint OFFSET_WAVE_FILE_SIZE = 4;

        const uint OFFSET_WAVE_AUDIO_DATA_SIZE = 40;

        uint numberOfSamples;

        uint samplesPerSec;

        uint numberOfBuffersWritten;

        IntPtr hWaveIn;

        IntPtr waveBufferPtr;

        WAVEHDR waveHdr;

        WAVEFORMATEX waveFormat;

        FileStream fileStream;

        BinaryWriter binaryWriter;

        byte[] waveArray;

        public WaveIn(uint paramNumberOfSamples, uint paramSamplesPerSec)
        {
            // TODO: check params
            numberOfSamples = paramNumberOfSamples;
            samplesPerSec = paramSamplesPerSec;
            numberOfBuffersWritten = 0;

            hWaveIn = IntPtr.Zero;
            waveBufferPtr = IntPtr.Zero;
            waveHdr = new WAVEHDR();
            waveFormat = new WAVEFORMATEX();
            //isRecording = new Mutex();
            //isBufferAvailable = new Mutex();

            waveArray = new byte[numberOfSamples];

            // make dataPtr a pointer a buffer, where the device will put the samples
            waveBufferPtr = Memory.LocalAlloc(Memory.LMEM_FIXED, numberOfSamples);
            if (waveBufferPtr == IntPtr.Zero)
            {
                throw new WaveInException("no memory to allocate buffer.");
            }
        }

        public List<string> getDevices()
        {
            // get the number of waveform-audio input devices present in the system
            int numberOfDevices = WinMM.waveInGetNumDevs();
            // list contains the names of waveform-audio input devices present in the system
            List<string> namesOfDevices = new List<string>();
            // describes the capabilities of a waveform-audio input device
            WAVEINCAPS waveInCaps = new WAVEINCAPS();
            // winmm functions return value
            int result = WinMM.MMSYSERR_NOERROR;

            if (numberOfDevices <= 0)
            {
                throw new WaveInException("No microphone has been detected.\n");
            }
            else
            {
                for (int uDeviceID = 0; uDeviceID < numberOfDevices; uDeviceID++)
                {
                    result = WinMM.waveInGetDevCaps(uDeviceID, ref waveInCaps, Marshal.SizeOf(waveInCaps));
                    if (WinMM.MMSYSERR_NOERROR != result)
                    {
                        throw new WaveInException("Failed to get information about input devices (microphones).\n");
                    }
                    namesOfDevices.Add(waveInCaps.szPname);
                }
            }
            return namesOfDevices;
        }

        public void startDevice(uint uDeviceID, string FileName)
        {
            // winmm functions return value
            numberOfBuffersWritten = 0;
            int result = WinMM.MMSYSERR_NOERROR;

            // parameters validation
            if (numberOfSamples > waveArray.Length)
            {
                string message = "The length of the buffer for recording given is" + waveArray.Length.ToString() +
                    ".\nIt length should be the number of samples, " + numberOfSamples.ToString() + ".\n";
                throw new WaveInException("Failed to open input device!\n");
            }

            // initialize the WAVEFORMATEX structure
            waveFormat.wFormatTag = WinMM.WAVE_FORMAT_PCM;
            waveFormat.nChannels = 1;
            waveFormat.nSamplesPerSec = samplesPerSec;
            waveFormat.wBitsPerSample = 8;
            waveFormat.nBlockAlign = (ushort)((waveFormat.nChannels) * (waveFormat.wBitsPerSample / 8));
            waveFormat.nAvgBytesPerSec = (uint)(waveFormat.nSamplesPerSec * waveFormat.nBlockAlign);
            waveFormat.cbSize = 0;


            // write the WAVE file header
            if ((fileStream = new FileStream(FileName, FileMode.Create)) == null)
            {
                throw new WaveInException("Failed to open output file: " + FileName + " \n");
            }
            else if ((binaryWriter = new BinaryWriter(fileStream)) == null)
            {
                throw new WaveInException("Failed to open binary writer!\n");
            }

            // Write out the header information
            WriteChars(binaryWriter, "RIFF");
            binaryWriter.Write(0);              // size of file - 8 (first 8 bytes) - unknown at this point

            WriteChars(binaryWriter, "WAVEfmt ");
            binaryWriter.Write((int)16);        // size of the WAVEFORMATEX struct that follows

            binaryWriter.Write(waveFormat.wFormatTag);
            binaryWriter.Write(waveFormat.nChannels);
            binaryWriter.Write(waveFormat.nSamplesPerSec);
            binaryWriter.Write(waveFormat.nAvgBytesPerSec);
            binaryWriter.Write(waveFormat.nBlockAlign);
            binaryWriter.Write(waveFormat.wBitsPerSample);

            WriteChars(binaryWriter, "data");
            binaryWriter.Write((int)0);              // size of buffer - unknown at this point

            // opens the given waveform-audio input device for recording
            result = WinMM.waveInOpen(ref hWaveIn, uDeviceID, ref waveFormat, 0, 0, 0);
            if (WinMM.MMSYSERR_NOERROR != result)
            {
                throw new WaveInException("Failed to open input device!\n");
            }

            // start the device. At this point, the sound bytes will not reach your program
            result = WinMM.waveInStart(hWaveIn);
            if (WinMM.MMSYSERR_NOERROR != result)
            {
                throw new WaveInException("Failed to start recording!\n");
            }

        }

        private void WriteChars(BinaryWriter wrtr, string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                char c = (char)text[i];
                wrtr.Write(c);
            }
        }

        public void stopDevice()
        {
            // winmm functions return value
            int result = WinMM.MMSYSERR_NOERROR;

            // close file - update empty fields
            int sizeOfAudioData = (int)(numberOfBuffersWritten * (int)numberOfSamples * waveFormat.nBlockAlign * 2);
            binaryWriter.Seek((int)OFFSET_WAVE_AUDIO_DATA_SIZE, SeekOrigin.Begin);
            binaryWriter.Write((int)sizeOfAudioData);

            int sizeOfFile = sizeOfAudioData + 44 - 8; // -8 for the 8 first bytes
            binaryWriter.Seek((int)OFFSET_WAVE_FILE_SIZE, SeekOrigin.Begin);
            binaryWriter.Write((int)sizeOfFile);

            if (fileStream != null)
                fileStream.Close();

            if (binaryWriter != null)
                binaryWriter.Close();


            if (WinMM.MMSYSERR_NOERROR != result)
            {
                //isRecording.ReleaseMutex();
                throw new WaveInException("Failed to reset input device!\n");
            }

            result = WinMM.waveInClose(hWaveIn);
            if (WinMM.MMSYSERR_NOERROR != result)
            {
                //isRecording.ReleaseMutex();
                throw new WaveInException("Failed to close input device!\n");
            }

            //isRecording.ReleaseMutex();
        }

        public void recordBuffer(byte[] waveArray, int sleepTime)
        {
            // winmm functions return value
            int result = WinMM.MMSYSERR_NOERROR;

            waveHdr.lpData = waveBufferPtr;
            waveHdr.dwBufferLength = (int)numberOfSamples * waveFormat.nBlockAlign; // length in bytes
            waveHdr.dwFlags = 0;

            // prepares a buffer for waveform-audio input. 
            result = WinMM.waveInPrepareHeader(hWaveIn, ref waveHdr, Marshal.SizeOf(waveHdr));
            if (WinMM.MMSYSERR_NOERROR != result)
            {
                throw new WaveInException("Failed to prepare audio block header!\n");
            }

            // sends an input buffer to the given waveform-audio input device. When the buffer is filled, the application is notified. 
            result = WinMM.waveInAddBuffer(hWaveIn, ref waveHdr, Marshal.SizeOf(waveHdr));
            if (WinMM.MMSYSERR_NOERROR != result)
            {
                throw new WaveInException("Failed to add buffer!]n");
            }

            // wait for the API buffer to fill
            //while (((waveHdr.dwFlags & WinMM.WHDR_DONE) != WinMM.WHDR_DONE) && (hWaveIn.Equals(IntPtr.Zero) == false))
            while ((waveHdr.dwFlags & WinMM.WHDR_DONE) != WinMM.WHDR_DONE)
            {
                Thread.Sleep(sleepTime);
            }

            result = WinMM.waveInUnprepareHeader(hWaveIn, ref waveHdr, Marshal.SizeOf(waveHdr));
            if (WinMM.MMSYSERR_NOERROR != result)
            {
                throw new WaveInException("Failed to unprepare audio block header!\n");
            }

            if (waveBufferPtr != IntPtr.Zero)
            {
                //isBufferAvailable.WaitOne();
                Marshal.Copy(waveBufferPtr, waveArray, 0, (int)numberOfSamples);
                //isBufferAvailable.ReleaseMutex();
            }
            else
            {
                throw new WaveInException("Failed to copy data from buffer!\n");
            }

            binaryWriter.Write(waveArray);
            binaryWriter.Write(waveArray);

            numberOfBuffersWritten++;
        }

        public void Dispose()
        {
            if (waveBufferPtr != IntPtr.Zero)
                Memory.LocalFree(waveBufferPtr);
        }
    }
}