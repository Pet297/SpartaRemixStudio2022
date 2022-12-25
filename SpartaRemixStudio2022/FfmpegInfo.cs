using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace SpartaRemixStudio2022
{
    public static class FfmpegInfo
    {
        public static bool Found { get; private set; } = false;
        public static bool UsePath { get; private set; } = false;
        public static string FfmpegPath { get; private set; } = null;
        public static string FfprobePath { get; private set; } = null;
        public static string CommandProbePath { get; private set; } = null;
        public static string CommandFFMPEGPath { get; private set; } = null;

        public static void LookForFFMPEG()
        {
            Found = true;
            //TODO: Warn about FFMPEG
            if (File.Exists("ffmpeg.txt"))
            {
                string text = File.ReadAllText("ffmpeg.txt");
                if (text == "path")
                {
                    UsePath = true;
                }
                else if (Directory.Exists(text))
                {
                    FfmpegPath = text;
                    if (!text.EndsWith("\\")) FfmpegPath += "\\";
                    FfprobePath = FfmpegPath + "ffprobe";
                    FfmpegPath = FfmpegPath + "ffmpeg";
                    if (!File.Exists(FfmpegPath + ".exe") || !File.Exists(FfprobePath + ".exe"))
                    {
                        Found = false;
                    }
                }
                else Found = false;
            }
            else Found = false;

            if (Found && UsePath)
            {
                CommandFFMPEGPath = "ffmpeg";
                CommandProbePath = "ffprobe";
            }
            else if (Found)
            {
                CommandFFMPEGPath = FfmpegPath;
                CommandProbePath = FfprobePath;
            }
        }
    }
    public static class VideoInfo
    {
        public static string GetFPS(string videoFile)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = FfmpegInfo.CommandProbePath;
            psi.UseShellExecute = false;

            psi.Arguments = $"-v error -select_streams v -of default=noprint_wrappers=1:nokey=1 -show_entries stream=r_frame_rate \"{videoFile}\"";

            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process process = Process.Start(psi);
            StreamReader stream = process.StandardOutput;
            string ret = stream.ReadLine();
            stream.Close();
            process.Kill();

            return ret;
        }
        public static string GetSize(string videoFile)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = FfmpegInfo.CommandProbePath;
            psi.UseShellExecute = false;

            psi.Arguments = $"-v error -select_streams v -show_entries stream=width,height -of csv=p=0:s=x \"{videoFile}\"";

            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process process = Process.Start(psi);
            StreamReader stream = process.StandardOutput;
            string ret = stream.ReadLine();
            stream.Close();
            try
            {
                process.Kill();
            }
            catch
            {

            }

            return ret;
        }
        public static bool IsVideoFile(string file)
        {
            try
            {
                string s = GetFPS(file);
                if (IsImageFile(file)) return false;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsAudioFile(string file)
        {
            try
            {
                AudioFileReader afr = new AudioFileReader(file);
                afr.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsImageFile(string file)
        {
            try
            {
                Bitmap b = new Bitmap(file);
                b.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
    public class CMDVideoReader
    {
        public int FpsN { get; private set; } = 30;
        public int FpsD { get; private set; } = 1;
        public int Width { get; private set; } = 1920;
        public int Height { get; private set; } = 1080;
        public byte[] Buffer = new byte[0];

        Stream stream;
        Process process;

        private int StartTime = 0;
        private int FramesRead = 0;
        private string currentFile;

        public CMDVideoReader()
        {
        }

        public void RePlay()
        {
            CloseFile();

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "CMD.exe";
            psi.UseShellExecute = false;

            if (StartTime > 2000) psi.Arguments = $"/C {FfmpegInfo.CommandFFMPEGPath} -ss {StartTime / 1000 - 2}.{StartTime % 1000:000} -i \"{currentFile}\" -ss 2.000 -s {Width}x{Height} -f rawvideo -pix_fmt bgr24 -";
            else psi.Arguments = $"/C {FfmpegInfo.CommandFFMPEGPath} -i \"{currentFile}\" -ss {StartTime / 1000}.{StartTime % 1000:000} -s {Width}x{Height} -f rawvideo -pix_fmt bgr24 -";

            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process process = Process.Start(psi);
            stream = process.StandardOutput.BaseStream;
        }

        public void OpenFile(string videoFile, int timeInMSec)
        {
            //try
            //{
                CloseFile();
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = FfmpegInfo.CommandFFMPEGPath;
                psi.UseShellExecute = false;

                if (timeInMSec > 2000) psi.Arguments = $"-ss {timeInMSec / 1000 - 2}.{timeInMSec % 1000:000} -i \"{videoFile}\" -ss 2.000 -f rawvideo -pix_fmt bgr24 -";
                else psi.Arguments = $"-i \"{videoFile}\" -ss {timeInMSec / 1000}.{timeInMSec % 1000:000} -f rawvideo -pix_fmt bgr24 -";

                psi.RedirectStandardInput = true;
                psi.RedirectStandardOutput = true;
                psi.CreateNoWindow = true;
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                Process process = Process.Start(psi);
                stream = process.StandardOutput.BaseStream;

                string[] wh = VideoInfo.GetSize(videoFile).Split('x');
                string[] nd = VideoInfo.GetFPS(videoFile).Split('/');

                Width = int.Parse(wh[0]);
                Height = int.Parse(wh[1]);
                FpsN = int.Parse(nd[0]);
                FpsD = int.Parse(nd[1]);

                currentFile = videoFile;
                StartTime = timeInMSec;
                FramesRead = 0;

                ReinitBuffer();
            //}
            /*catch
            {
                CloseFile();
            }*/
        }
        public void OpenFile(string videoFile, int timeInMSec, int width, int height)
        {
            try
            {
                CloseFile();

                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = FfmpegInfo.CommandFFMPEGPath;
                psi.UseShellExecute = false;

                if (timeInMSec > 2000) psi.Arguments = $"-ss {timeInMSec / 1000 - 2}.{timeInMSec % 1000:000} -i \"{videoFile}\" -ss 2.000 -s {width}x{height} -f rawvideo -pix_fmt bgr24 -";
                else psi.Arguments = $"-i \"{videoFile}\" -ss {timeInMSec / 1000}.{timeInMSec % 1000:000} -s {width}x{height} -f rawvideo -pix_fmt bgr24 -";

                psi.RedirectStandardInput = true;
                psi.RedirectStandardOutput = true;
                psi.CreateNoWindow = true;
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                Process process = Process.Start(psi);
                stream = process.StandardOutput.BaseStream;

                string[] nd = VideoInfo.GetFPS(videoFile).Split('/');

                Width = width;
                Height = height;
                FpsN = int.Parse(nd[0]);
                FpsD = int.Parse(nd[1]);

                currentFile = videoFile;
                StartTime = timeInMSec;
                FramesRead = 0;

                ReinitBuffer();
            }
            catch
            {
                CloseFile();
            }
        }
        public void SetFPS(int newNumerator, int newDenominator)
        {
            FpsN = newNumerator;
            FpsD = newDenominator;
        }
        public void ReinitBuffer()
        {
            Buffer = new byte[((Width * Height * 3) / 4096) * 4096 + 4096];
        }

        public void Read()
        {
            if (stream != null)
            {
                for (int pos = 0; pos < Width * Height * 3; pos += 4096) stream.Read(Buffer, pos, 4096);
                FramesRead++;
            }
        }
        public bool ReadTimeAbsolute(float time)
        {
            if (stream == null) return false;
            float relative = time - StartTime / 1000f;
            return ReadTimeRelative(relative);
        }
        public bool ReadTimeRelative(float time)
        {
            if (stream == null) return false;
            int frameNumber = (int)(time * FpsN / FpsD);
            return ReadFrameIndex(frameNumber);
        }
        public bool ReadFrameIndex(int frameNumber)
        {
            if (stream == null) return false;
            if (frameNumber < 1) frameNumber = 1;

            bool readAnything = false;

            while (FramesRead < frameNumber)
            {
                Read();
                readAnything = true;
            }

            return readAnything;
        }

        public void CloseFile()
        {
            if (process != null)
            {
                process.Kill();
                process = null;
            }
            if (stream != null)
            {
                stream.Dispose();
                stream = null;
            }
        }
    }
}
