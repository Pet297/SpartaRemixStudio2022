using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaRemixStudio2022
{
    partial class RegularTrack : IComplexObject
    {

        public bool AcceptVariable(uint id, Stream s, int lenght)
        {
            switch (id)
            {
                default: return false;
            }
        }
        public void SetDefaultState()
        {
        }
        public void SaveVariable(uint id, Stream s)
        {
            switch (id)
            {
            }
        }
        public int ReportLenghtOfVariable(uint id)
        {
            switch (id)
            {
                default: return 0;
            }
        }
        public List<uint> GetVarNamesToSave()
        {
            return new List<uint>() { };
        }
        public RegularTrack()
        {
            SetDefaultState();
        }
    }

    partial class VideoCutSample : IComplexObject
    {
        private int SourceIndex { get; set; }
        private string SourceFile { get; set; }
        private double SourceTime { get; set; }

        public bool AcceptVariable(uint id, Stream s, int lenght)
        {
            switch (id)
            {
                case 0x100: SourceIndex = StreamHelper.LoadUnmanaged<int>(s); return true;
                case 0x101: SourceFile = StreamHelper.LoadString(s); return true;
                case 0x102: SourceTime = StreamHelper.LoadUnmanaged<double>(s); return true;
                default: return false;
            }
        }
        public void SetDefaultState()
        {
            SourceIndex = -1;
            SourceFile = "";
            SourceTime = 0;
        }
        public void SaveVariable(uint id, Stream s)
        {
            switch (id)
            {
                case 0x100: StreamHelper.SaveUnmanaged<int>(s, SourceIndex); break;
                case 0x101: StreamHelper.SaveString(s, SourceFile); break;
                case 0x102: StreamHelper.SaveUnmanaged<double>(s, SourceTime); break;
            }
        }
        public int ReportLenghtOfVariable(uint id)
        {
            switch (id)
            {
                case 0x100: return StreamHelper.GetUnmanagedLenght<int>(SourceIndex);
                case 0x101: return StreamHelper.GetLenght(SourceFile);
                case 0x102: return StreamHelper.GetUnmanagedLenght<double>(SourceTime);
                default: return 0;
            }
        }
        public List<uint> GetVarNamesToSave()
        {
            return new List<uint>() { 0x100, 0x101, 0x102 };
        }
        public VideoCutSample()
        {
            SetDefaultState();
        }
    }

    partial class AudioCutSample : IComplexObject
    {
        private int SourceIndex { get; set; }
        private string SourceFile { get; set; }
        private double SourceTime { get; set; }
        public int Channels { get; private set; }
        public float SampleFreq { get; private set; }
        public float[] Audio { get; private set; }
        public double DefaultPitch { get; set; }
        public double DefaultSpeed { get; set; }
        public double DefaultVolume { get; set; }

        public bool AcceptVariable(uint id, Stream s, int lenght)
        {
            switch (id)
            {
                case 0x100: SourceIndex = StreamHelper.LoadUnmanaged<int>(s); return true;
                case 0x101: SourceFile = StreamHelper.LoadString(s); return true;
                case 0x102: SourceTime = StreamHelper.LoadUnmanaged<double>(s); return true;
                case 0x200: Channels = StreamHelper.LoadUnmanaged<int>(s); return true;
                case 0x201: SampleFreq = StreamHelper.LoadUnmanaged<float>(s); return true;
                case 0x202: Audio = StreamHelper.LoadArray<float>(s, (ss) => { return StreamHelper.LoadUnmanaged<float>(ss); }); return true;
                case 0x300: DefaultPitch = StreamHelper.LoadUnmanaged<double>(s); return true;
                case 0x301: DefaultSpeed = StreamHelper.LoadUnmanaged<double>(s); return true;
                case 0x302: DefaultVolume = StreamHelper.LoadUnmanaged<double>(s); return true;
                default: return false;
            }
        }
        public void SetDefaultState()
        {
            SourceIndex = -1;
            SourceFile = "";
            SourceTime = 0;
            Channels = 1;
            SampleFreq = 48000;
            Audio = new float[0];
            DefaultPitch = 0;
            DefaultSpeed = 1;
            DefaultVolume = 1;
        }
        public void SaveVariable(uint id, Stream s)
        {
            switch (id)
            {
                case 0x100: StreamHelper.SaveUnmanaged<int>(s, SourceIndex); break;
                case 0x101: StreamHelper.SaveString(s, SourceFile); break;
                case 0x102: StreamHelper.SaveUnmanaged<double>(s, SourceTime); break;
                case 0x200: StreamHelper.SaveUnmanaged<int>(s, Channels); break;
                case 0x201: StreamHelper.SaveUnmanaged<float>(s, SampleFreq); break;
                case 0x202: StreamHelper.SaveArray(s, Audio, (ss, Audio0) => { StreamHelper.SaveUnmanaged<float>(ss, Audio0); }); break;
                case 0x300: StreamHelper.SaveUnmanaged<double>(s, DefaultPitch); break;
                case 0x301: StreamHelper.SaveUnmanaged<double>(s, DefaultSpeed); break;
                case 0x302: StreamHelper.SaveUnmanaged<double>(s, DefaultVolume); break;
            }
        }
        public int ReportLenghtOfVariable(uint id)
        {
            switch (id)
            {
                case 0x100: return StreamHelper.GetUnmanagedLenght<int>(SourceIndex);
                case 0x101: return StreamHelper.GetLenght(SourceFile);
                case 0x102: return StreamHelper.GetUnmanagedLenght<double>(SourceTime);
                case 0x200: return StreamHelper.GetUnmanagedLenght<int>(Channels);
                case 0x201: return StreamHelper.GetUnmanagedLenght<float>(SampleFreq);
                case 0x202: return StreamHelper.GetLenght<float>(Audio, Audio0 => { return StreamHelper.GetUnmanagedLenght<float>(Audio0); });
                case 0x300: return StreamHelper.GetUnmanagedLenght<double>(DefaultPitch);
                case 0x301: return StreamHelper.GetUnmanagedLenght<double>(DefaultSpeed);
                case 0x302: return StreamHelper.GetUnmanagedLenght<double>(DefaultVolume);
                default: return 0;
            }
        }
        public List<uint> GetVarNamesToSave()
        {
            return new List<uint>() { 0x100, 0x101, 0x102, 0x200, 0x201, 0x202, 0x300, 0x301, 0x302 };
        }
        public AudioCutSample()
        {
            SetDefaultState();
        }
    }

    partial class SampleMedia : IComplexObject
    {
        public int SampleID { get; private set; }

        public bool AcceptVariable(uint id, Stream s, int lenght)
        {
            switch (id)
            {
                case 0x100: SampleID = StreamHelper.LoadUnmanaged<int>(s); return true;
                default: return false;
            }
        }
        public void SetDefaultState()
        {
            SampleID = 0;
        }
        public void SaveVariable(uint id, Stream s)
        {
            switch (id)
            {
                case 0x100: StreamHelper.SaveUnmanaged<int>(s, SampleID); break;
            }
        }
        public int ReportLenghtOfVariable(uint id)
        {
            switch (id)
            {
                case 0x100: return StreamHelper.GetUnmanagedLenght<int>(SampleID);
                default: return 0;
            }
        }
        public List<uint> GetVarNamesToSave()
        {
            return new List<uint>() { 0x100 };
        }
        public SampleMedia()
        {
            SetDefaultState();
        }
    }
}
