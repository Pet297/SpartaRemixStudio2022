using SpartaRemixStudio2022.Properties;
using SpartaRemixStudio2022;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaRemixStudio2022
{
    partial class Project : IComplexObject
    {
        private Dictionary<int, VideoSource> Sources { get; set; }
        private int NextSourceIndex { get; set; }
        private Dictionary<int, AVSample> Samples { get; set; }
        private int NextSampleIndex { get; set; }
        private Dictionary<int, Pattern> Patterns { get; set; }
        private int NextPatternIndex { get; set; }
        public Timeline timeline { get; private set; }

        public bool AcceptVariable(uint id, Stream s, int lenght)
        {
            switch (id)
            {
                case 0x100: Sources = StreamHelper.LoadDictionary<int, VideoSource>(s, (ss) => { return StreamHelper.LoadUnmanaged<int>(ss); }, (ss) => { return UniLoad.CreateObject<VideoSource>(ss); }); return true;
                case 0x101: NextSourceIndex = StreamHelper.LoadUnmanaged<int>(s); return true;
                case 0x110: Samples = StreamHelper.LoadDictionary<int, AVSample>(s, (ss) => { return StreamHelper.LoadUnmanaged<int>(ss); }, (ss) => { return UniLoad.CreateObject<AVSample>(ss); }); return true;
                case 0x111: NextSampleIndex = StreamHelper.LoadUnmanaged<int>(s); return true;
                case 0x120: Patterns = StreamHelper.LoadDictionary<int, Pattern>(s, (ss) => { return StreamHelper.LoadUnmanaged<int>(ss); }, (ss) => { return UniLoad.CreateObject<Pattern>(ss); }); return true;
                case 0x121: NextPatternIndex = StreamHelper.LoadUnmanaged<int>(s); return true;
                case 0x200: timeline = UniLoad.CreateObject<Timeline>(s); return true;
                default: return false;
            }
        }
        public void SetDefaultState()
        {
            Sources = new Dictionary<int, VideoSource>();
            NextSourceIndex = 0;
            Samples = new Dictionary<int, AVSample>();
            NextSampleIndex = 0;
            Patterns = new Dictionary<int, Pattern>();
            NextPatternIndex = 0;
            timeline = new Timeline();
        }
        public void SaveVariable(uint id, Stream s)
        {
            switch (id)
            {
                case 0x100: StreamHelper.SaveDictionary(s, Sources, (ss, Sources0) => { StreamHelper.SaveUnmanaged<int>(ss, Sources0); }, (ss, Sources0) => { UniLoad.Save(ss, Sources0); }); break;
                case 0x101: StreamHelper.SaveUnmanaged<int>(s, NextSourceIndex); break;
                case 0x110: StreamHelper.SaveDictionary(s, Samples, (ss, Samples0) => { StreamHelper.SaveUnmanaged<int>(ss, Samples0); }, (ss, Samples0) => { UniLoad.Save(ss, Samples0); }); break;
                case 0x111: StreamHelper.SaveUnmanaged<int>(s, NextSampleIndex); break;
                case 0x120: StreamHelper.SaveDictionary(s, Patterns, (ss, Patterns0) => { StreamHelper.SaveUnmanaged<int>(ss, Patterns0); }, (ss, Patterns0) => { UniLoad.Save(ss, Patterns0); }); break;
                case 0x121: StreamHelper.SaveUnmanaged<int>(s, NextPatternIndex); break;
                case 0x200: UniLoad.Save(s, timeline); break;
            }
        }
        public int ReportLenghtOfVariable(uint id)
        {
            switch (id)
            {
                case 0x100: return StreamHelper.GetLenght<int, VideoSource>(Sources, Sources0 => { return StreamHelper.GetUnmanagedLenght<int>(Sources0); }, Sources0 => { return UniLoad.GetLenght(Sources0); });
                case 0x101: return StreamHelper.GetUnmanagedLenght<int>(NextSourceIndex);
                case 0x110: return StreamHelper.GetLenght<int, AVSample>(Samples, Samples0 => { return StreamHelper.GetUnmanagedLenght<int>(Samples0); }, Samples0 => { return UniLoad.GetLenght(Samples0); });
                case 0x111: return StreamHelper.GetUnmanagedLenght<int>(NextSampleIndex);
                case 0x120: return StreamHelper.GetLenght<int, Pattern>(Patterns, Patterns0 => { return StreamHelper.GetUnmanagedLenght<int>(Patterns0); }, Patterns0 => { return UniLoad.GetLenght(Patterns0); });
                case 0x121: return StreamHelper.GetUnmanagedLenght<int>(NextPatternIndex);
                case 0x200: return UniLoad.GetLenght(timeline);
                default: return 0;
            }
        }
        public List<uint> GetVarNamesToSave()
        {
            return new List<uint>() { 0x100, 0x101, 0x110, 0x111, 0x120, 0x121, 0x200 };
        }
        public Project()
        {
            SetDefaultState();
        }
    }

    partial class VideoSource : IComplexObject
    {
        public string File { get; set; }
        public PreviewWave PreviewWave { get; set; }
        public int Index { get; private set; }

        public bool AcceptVariable(uint id, Stream s, int lenght)
        {
            switch (id)
            {
                case 0x100: File = StreamHelper.LoadString(s); return true;
                case 0x101: PreviewWave = UniLoad.CreateObject<PreviewWave>(s); return true;
                case 0x102: Index = StreamHelper.LoadUnmanaged<int>(s); return true;
                default: return false;
            }
        }
        public void SetDefaultState()
        {
            File = "";
            PreviewWave = new PreviewWave();
            Index = 0;
        }
        public void SaveVariable(uint id, Stream s)
        {
            switch (id)
            {
                case 0x100: StreamHelper.SaveString(s, File); break;
                case 0x101: UniLoad.Save(s, PreviewWave); break;
                case 0x102: StreamHelper.SaveUnmanaged<int>(s, Index); break;
            }
        }
        public int ReportLenghtOfVariable(uint id)
        {
            switch (id)
            {
                case 0x100: return StreamHelper.GetLenght(File);
                case 0x101: return UniLoad.GetLenght(PreviewWave);
                case 0x102: return StreamHelper.GetUnmanagedLenght<int>(Index);
                default: return 0;
            }
        }
        public List<uint> GetVarNamesToSave()
        {
            return new List<uint>() { 0x100, 0x101, 0x102 };
        }
        public VideoSource()
        {
            SetDefaultState();
        }
    }

    partial class PreviewWave : IComplexObject
    {
        private float[] Values { get; set; }
        private float PointsPerSecond { get; set; }

        public bool AcceptVariable(uint id, Stream s, int lenght)
        {
            switch (id)
            {
                case 0x100: Values = StreamHelper.LoadArray<float>(s, (ss) => { return StreamHelper.LoadUnmanaged<float>(ss); }); return true;
                case 0x101: PointsPerSecond = StreamHelper.LoadUnmanaged<float>(s); return true;
                default: return false;
            }
        }
        public void SetDefaultState()
        {
            Values = new float[1];
            PointsPerSecond = 1.0f;
        }
        public void SaveVariable(uint id, Stream s)
        {
            switch (id)
            {
                case 0x100: StreamHelper.SaveArray(s, Values, (ss, Values0) => { StreamHelper.SaveUnmanaged<float>(ss, Values0); }); break;
                case 0x101: StreamHelper.SaveUnmanaged<float>(s, PointsPerSecond); break;
            }
        }
        public int ReportLenghtOfVariable(uint id)
        {
            switch (id)
            {
                case 0x100: return StreamHelper.GetLenght<float>(Values, Values0 => { return StreamHelper.GetUnmanagedLenght<float>(Values0); });
                case 0x101: return StreamHelper.GetUnmanagedLenght<float>(PointsPerSecond);
                default: return 0;
            }
        }
        public List<uint> GetVarNamesToSave()
        {
            return new List<uint>() { 0x100, 0x101 };
        }
        public PreviewWave()
        {
            SetDefaultState();
        }
    }

    partial class AVSample : IComplexObject
    {
        public int Index { get; private set; }
        public NameColor NameColor { get; set; }
        public long AudioId { get; private set; }
        public IAudioSample AudioSample { get; private set; }
        public long VideoId { get; private set; }
        public IVideoSample VideoSample { get; private set; }

        public bool AcceptVariable(uint id, Stream s, int lenght)
        {
            switch (id)
            {
                case 0x100: Index = StreamHelper.LoadUnmanaged<int>(s); return true;
                case 0x101: NameColor = UniLoad.CreateObject<NameColor>(s); return true;
                case 0x1000: AudioId = StreamHelper.LoadUnmanaged<long>(s); return true;
                case 0x2000: AudioSample = /*TODO*/null; AudioSample.SetDefaultState(); AudioSample.LoadObject(s); return true;
                case 0x1001: VideoId = StreamHelper.LoadUnmanaged<long>(s); return true;
                case 0x2001: VideoSample = /*TODO*/null; VideoSample.SetDefaultState(); VideoSample.LoadObject(s); return true;
                default: return false;
            }
        }
        public void SetDefaultState()
        {
            Index = 0;
            NameColor = new NameColor();
            AudioId = 0;
            AudioSample = null;
            VideoId = 0;
            VideoSample = null;
        }
        public void SaveVariable(uint id, Stream s)
        {
            switch (id)
            {
                case 0x100: StreamHelper.SaveUnmanaged<int>(s, Index); break;
                case 0x101: UniLoad.Save(s, NameColor); break;
                case 0x1000: StreamHelper.SaveUnmanaged<long>(s, AudioId); break;
                case 0x2000: UniLoad.Save(s, AudioSample); break;
                case 0x1001: StreamHelper.SaveUnmanaged<long>(s, VideoId); break;
                case 0x2001: UniLoad.Save(s, VideoSample); break;
            }
        }
        public int ReportLenghtOfVariable(uint id)
        {
            switch (id)
            {
                case 0x100: return StreamHelper.GetUnmanagedLenght<int>(Index);
                case 0x101: return UniLoad.GetLenght(NameColor);
                case 0x1000: return StreamHelper.GetUnmanagedLenght<long>(AudioId);
                case 0x2000: return UniLoad.GetLenght(AudioSample);
                case 0x1001: return StreamHelper.GetUnmanagedLenght<long>(VideoId);
                case 0x2001: return UniLoad.GetLenght(VideoSample);
                default: return 0;
            }
        }
        public List<uint> GetVarNamesToSave()
        {
            return new List<uint>() { 0x100, 0x101, 0x1000, 0x2000, 0x1001, 0x2001 };
        }
        public AVSample()
        {
            SetDefaultState();
        }
    }

    partial class NameColor : IComplexObject
    {
        public string Name { get; set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public bool AcceptVariable(uint id, Stream s, int lenght)
        {
            switch (id)
            {
                case 0x100: Name = StreamHelper.LoadString(s); return true;
                case 0x200: R = StreamHelper.LoadUnmanaged<byte>(s); return true;
                case 0x201: G = StreamHelper.LoadUnmanaged<byte>(s); return true;
                case 0x202: B = StreamHelper.LoadUnmanaged<byte>(s); return true;
                default: return false;
            }
        }
        public void SetDefaultState()
        {
            Name = "";
            R = 0;
            G = 0;
            B = 0;
        }
        public void SaveVariable(uint id, Stream s)
        {
            switch (id)
            {
                case 0x100: StreamHelper.SaveString(s, Name); break;
                case 0x200: StreamHelper.SaveUnmanaged<byte>(s, R); break;
                case 0x201: StreamHelper.SaveUnmanaged<byte>(s, G); break;
                case 0x202: StreamHelper.SaveUnmanaged<byte>(s, B); break;
            }
        }
        public int ReportLenghtOfVariable(uint id)
        {
            switch (id)
            {
                case 0x100: return StreamHelper.GetLenght(Name);
                case 0x200: return StreamHelper.GetUnmanagedLenght<byte>(R);
                case 0x201: return StreamHelper.GetUnmanagedLenght<byte>(G);
                case 0x202: return StreamHelper.GetUnmanagedLenght<byte>(B);
                default: return 0;
            }
        }
        public List<uint> GetVarNamesToSave()
        {
            return new List<uint>() { 0x100, 0x200, 0x201, 0x202 };
        }
        public NameColor()
        {
            SetDefaultState();
        }
    }

    partial class Timeline : IComplexObject
    {
        public List<Track> Tracks { get; set; }
        public string Name { get; set; }
        public List<TimelineGridline> Gridlines { get; set; }

        public bool AcceptVariable(uint id, Stream s, int lenght)
        {
            switch (id)
            {
                case 0x100: Tracks = StreamHelper.LoadList<Track>(s, (ss) => { return UniLoad.CreateObject<Track>(ss); }); return true;
                case 0x101: Name = StreamHelper.LoadString(s); return true;
                case 0x200: Gridlines = StreamHelper.LoadList<TimelineGridline>(s, (ss) => { return UniLoad.CreateObject<TimelineGridline>(ss); }); return true;
                default: return false;
            }
        }
        public void SetDefaultState()
        {
            Tracks = new List<Track>();
            Name = "";
            Gridlines = new List<TimelineGridline>();
        }
        public void SaveVariable(uint id, Stream s)
        {
            switch (id)
            {
                case 0x100: StreamHelper.SaveList(s, Tracks, (ss, Tracks0) => { UniLoad.Save(ss, Tracks0); }); break;
                case 0x101: StreamHelper.SaveString(s, Name); break;
                case 0x200: StreamHelper.SaveList(s, Gridlines, (ss, Gridlines0) => { UniLoad.Save(ss, Gridlines0); }); break;
            }
        }
        public int ReportLenghtOfVariable(uint id)
        {
            switch (id)
            {
                case 0x100: return StreamHelper.GetLenght<Track>(Tracks, Tracks0 => { return UniLoad.GetLenght(Tracks0); });
                case 0x101: return StreamHelper.GetLenght(Name);
                case 0x200: return StreamHelper.GetLenght<TimelineGridline>(Gridlines, Gridlines0 => { return UniLoad.GetLenght(Gridlines0); });
                default: return 0;
            }
        }
        public List<uint> GetVarNamesToSave()
        {
            return new List<uint>() { 0x100, 0x101, 0x200 };
        }
        public Timeline()
        {
            SetDefaultState();
        }
    }

    partial class TimelineGridline : IComplexObject
    {
        public long Position { get; private set; }
        public float MaxSamplesPerPixel { get; private set; }
        public string Name { get; private set; }
        public byte R { get; private set; }
        public byte G { get; private set; }
        public byte B { get; private set; }

        public bool AcceptVariable(uint id, Stream s, int lenght)
        {
            switch (id)
            {
                case 0x100: Position = StreamHelper.LoadUnmanaged<long>(s); return true;
                case 0x101: MaxSamplesPerPixel = StreamHelper.LoadUnmanaged<float>(s); return true;
                case 0x102: Name = StreamHelper.LoadString(s); return true;
                case 0x200: R = StreamHelper.LoadUnmanaged<byte>(s); return true;
                case 0x201: G = StreamHelper.LoadUnmanaged<byte>(s); return true;
                case 0x202: B = StreamHelper.LoadUnmanaged<byte>(s); return true;
                default: return false;
            }
        }
        public void SetDefaultState()
        {
            Position = 0;
            MaxSamplesPerPixel = 10;
            Name = "";
            R = 0;
            G = 0;
            B = 0;
        }
        public void SaveVariable(uint id, Stream s)
        {
            switch (id)
            {
                case 0x100: StreamHelper.SaveUnmanaged<long>(s, Position); break;
                case 0x101: StreamHelper.SaveUnmanaged<float>(s, MaxSamplesPerPixel); break;
                case 0x102: StreamHelper.SaveString(s, Name); break;
                case 0x200: StreamHelper.SaveUnmanaged<byte>(s, R); break;
                case 0x201: StreamHelper.SaveUnmanaged<byte>(s, G); break;
                case 0x202: StreamHelper.SaveUnmanaged<byte>(s, B); break;
            }
        }
        public int ReportLenghtOfVariable(uint id)
        {
            switch (id)
            {
                case 0x100: return StreamHelper.GetUnmanagedLenght<long>(Position);
                case 0x101: return StreamHelper.GetUnmanagedLenght<float>(MaxSamplesPerPixel);
                case 0x102: return StreamHelper.GetLenght(Name);
                case 0x200: return StreamHelper.GetUnmanagedLenght<byte>(R);
                case 0x201: return StreamHelper.GetUnmanagedLenght<byte>(G);
                case 0x202: return StreamHelper.GetUnmanagedLenght<byte>(B);
                default: return 0;
            }
        }
        public List<uint> GetVarNamesToSave()
        {
            return new List<uint>() { 0x100, 0x101, 0x102, 0x200, 0x201, 0x202 };
        }
        public TimelineGridline()
        {
            SetDefaultState();
        }
    }

    partial class Pattern : IComplexObject
    {
        public List<SimpleTrack> Tracks { get; set; }
        public string Name { get; set; }
        public int Index { get; private set; }

        public bool AcceptVariable(uint id, Stream s, int lenght)
        {
            switch (id)
            {
                case 0x100: Tracks = StreamHelper.LoadList<SimpleTrack>(s, (ss) => { return UniLoad.CreateObject<SimpleTrack>(ss); }); return true;
                case 0x101: Name = StreamHelper.LoadString(s); return true;
                case 0x102: Index = StreamHelper.LoadUnmanaged<int>(s); return true;
                default: return false;
            }
        }
        public void SetDefaultState()
        {
            Tracks = new List<SimpleTrack>();
            Name = "";
            Index = 0;
        }
        public void SaveVariable(uint id, Stream s)
        {
            switch (id)
            {
                case 0x100: StreamHelper.SaveList(s, Tracks, (ss, Tracks0) => { UniLoad.Save(ss, Tracks0); }); break;
                case 0x101: StreamHelper.SaveString(s, Name); break;
                case 0x102: StreamHelper.SaveUnmanaged<int>(s, Index); break;
            }
        }
        public int ReportLenghtOfVariable(uint id)
        {
            switch (id)
            {
                case 0x100: return StreamHelper.GetLenght<SimpleTrack>(Tracks, Tracks0 => { return UniLoad.GetLenght(Tracks0); });
                case 0x101: return StreamHelper.GetLenght(Name);
                case 0x102: return StreamHelper.GetUnmanagedLenght<int>(Index);
                default: return 0;
            }
        }
        public List<uint> GetVarNamesToSave()
        {
            return new List<uint>() { 0x100, 0x101, 0x102 };
        }
        public Pattern()
        {
            SetDefaultState();
        }
    }

    partial class SimpleTrack : IComplexObject
    {
        private List<Media> TrackMedia { get; set; }
        public string Name { get; set; }

        public bool AcceptVariable(uint id, Stream s, int lenght)
        {
            switch (id)
            {
                case 0x100: TrackMedia = StreamHelper.LoadList<Media>(s, (ss) => { return UniLoad.CreateObject<Media>(ss); }); return true;
                case 0x300: Name = StreamHelper.LoadString(s); return true;
                default: return false;
            }
        }
        public void SetDefaultState()
        {
            TrackMedia = new List<Media>();
            Name = "";
        }
        public void SaveVariable(uint id, Stream s)
        {
            switch (id)
            {
                case 0x100: StreamHelper.SaveList(s, TrackMedia, (ss, TrackMedia0) => { UniLoad.Save(ss, TrackMedia0); }); break;
                case 0x300: StreamHelper.SaveString(s, Name); break;
            }
        }
        public int ReportLenghtOfVariable(uint id)
        {
            switch (id)
            {
                case 0x100: return StreamHelper.GetLenght<Media>(TrackMedia, TrackMedia0 => { return UniLoad.GetLenght(TrackMedia0); });
                case 0x300: return StreamHelper.GetLenght(Name);
                default: return 0;
            }
        }
        public List<uint> GetVarNamesToSave()
        {
            return new List<uint>() { 0x100, 0x300 };
        }
        public SimpleTrack()
        {
            SetDefaultState();
        }
    }

    partial class Track : IComplexObject
    {
        private List<Media> TrackMedia { get; set; }
        private List<AudioFX> AudioEffects { get; set; }
        private List<VideoFX> VideoEffects { get; set; }
        private List<Track> Children { get; set; }
        public string Name { get; set; }
        public long ExtId { get; private set; }
        public ITrackType ExtType { get; private set; }

        public bool AcceptVariable(uint id, Stream s, int lenght)
        {
            switch (id)
            {
                case 0x100: TrackMedia = StreamHelper.LoadList<Media>(s, (ss) => { return UniLoad.CreateObject<Media>(ss); }); return true;
                case 0x101: AudioEffects = StreamHelper.LoadList<AudioFX>(s, (ss) => { return UniLoad.CreateObject<AudioFX>(ss); }); return true;
                case 0x200: VideoEffects = StreamHelper.LoadList<VideoFX>(s, (ss) => { return UniLoad.CreateObject<VideoFX>(ss); }); return true;
                case 0x201: Children = StreamHelper.LoadList<Track>(s, (ss) => { return UniLoad.CreateObject<Track>(ss); }); return true;
                case 0x300: Name = StreamHelper.LoadString(s); return true;
                case 0x1000: ExtId = StreamHelper.LoadUnmanaged<long>(s); return true;
                case 0x2000: ExtType = ExtensionManager.GetTrackType(this, ExtId); ExtType.SetDefaultState(); ExtType.LoadObject(s); return true;
                default: return false;
            }
        }
        public void SetDefaultState()
        {
            TrackMedia = new List<Media>();
            AudioEffects = new List<AudioFX>();
            VideoEffects = new List<VideoFX>();
            Children = new List<Track>();
            Name = "";
            ExtId = 0;
            ExtType = null;
        }
        public void SaveVariable(uint id, Stream s)
        {
            switch (id)
            {
                case 0x100: StreamHelper.SaveList(s, TrackMedia, (ss, TrackMedia0) => { UniLoad.Save(ss, TrackMedia0); }); break;
                case 0x101: StreamHelper.SaveList(s, AudioEffects, (ss, AudioEffects0) => { UniLoad.Save(ss, AudioEffects0); }); break;
                case 0x200: StreamHelper.SaveList(s, VideoEffects, (ss, VideoEffects0) => { UniLoad.Save(ss, VideoEffects0); }); break;
                case 0x201: StreamHelper.SaveList(s, Children, (ss, Children0) => { UniLoad.Save(ss, Children0); }); break;
                case 0x300: StreamHelper.SaveString(s, Name); break;
                case 0x1000: StreamHelper.SaveUnmanaged<long>(s, ExtId); break;
                case 0x2000: UniLoad.Save(s, ExtType); break;
            }
        }
        public int ReportLenghtOfVariable(uint id)
        {
            switch (id)
            {
                case 0x100: return StreamHelper.GetLenght<Media>(TrackMedia, TrackMedia0 => { return UniLoad.GetLenght(TrackMedia0); });
                case 0x101: return StreamHelper.GetLenght<AudioFX>(AudioEffects, AudioEffects0 => { return UniLoad.GetLenght(AudioEffects0); });
                case 0x200: return StreamHelper.GetLenght<VideoFX>(VideoEffects, VideoEffects0 => { return UniLoad.GetLenght(VideoEffects0); });
                case 0x201: return StreamHelper.GetLenght<Track>(Children, Children0 => { return UniLoad.GetLenght(Children0); });
                case 0x300: return StreamHelper.GetLenght(Name);
                case 0x1000: return StreamHelper.GetUnmanagedLenght<long>(ExtId);
                case 0x2000: return UniLoad.GetLenght(ExtType);
                default: return 0;
            }
        }
        public List<uint> GetVarNamesToSave()
        {
            return new List<uint>() { 0x100, 0x101, 0x200, 0x201, 0x300, 0x1000, 0x2000 };
        }
        public Track()
        {
            SetDefaultState();
        }
    }

    partial class Media : IComplexObject
    {
        public long Position { get; set; }
        public long Length { get; set; }
        public double StartTime { get; set; }
        public double Speed { get; set; }
        public double Pitch { get; set; }
        public double Formant { get; set; }
        public double Opacity { get; set; }
        public double Volume { get; set; }
        public double Pan { get; set; }
        public double ModX { get; set; }
        public double ModY { get; set; }
        public uint Index { get; set; }
        public long ExtId { get; private set; }
        public IMediaType ExtType { get; private set; }

        public bool AcceptVariable(uint id, Stream s, int lenght)
        {
            switch (id)
            {
                case 0x100: Position = StreamHelper.LoadUnmanaged<long>(s); return true;
                case 0x101: Length = StreamHelper.LoadUnmanaged<long>(s); return true;
                case 0x200: StartTime = StreamHelper.LoadUnmanaged<double>(s); return true;
                case 0x201: Speed = StreamHelper.LoadUnmanaged<double>(s); return true;
                case 0x202: Pitch = StreamHelper.LoadUnmanaged<double>(s); return true;
                case 0x203: Formant = StreamHelper.LoadUnmanaged<double>(s); return true;
                case 0x204: Opacity = StreamHelper.LoadUnmanaged<double>(s); return true;
                case 0x205: Volume = StreamHelper.LoadUnmanaged<double>(s); return true;
                case 0x206: Pan = StreamHelper.LoadUnmanaged<double>(s); return true;
                case 0x300: ModX = StreamHelper.LoadUnmanaged<double>(s); return true;
                case 0x301: ModY = StreamHelper.LoadUnmanaged<double>(s); return true;
                case 0x400: Index = StreamHelper.LoadUnmanaged<uint>(s); return true;
                case 0x1000: ExtId = StreamHelper.LoadUnmanaged<long>(s); return true;
                case 0x2000: ExtType = /*TODO*/null; ExtType.SetDefaultState(); ExtType.LoadObject(s); return true;
                default: return false;
            }
        }
        public void SetDefaultState()
        {
            Position = 0;
            Length = 48000;
            StartTime = 0.0;
            Speed = 1.0;
            Pitch = 0.0;
            Formant = 0.0;
            Opacity = 1.0;
            Volume = 1.0;
            Pan = 0.0;
            ModX = 0.0;
            ModY = 0.0;
            Index = 0;
            ExtId = 0;
            ExtType = null;
        }
        public void SaveVariable(uint id, Stream s)
        {
            switch (id)
            {
                case 0x100: StreamHelper.SaveUnmanaged<long>(s, Position); break;
                case 0x101: StreamHelper.SaveUnmanaged<long>(s, Length); break;
                case 0x200: StreamHelper.SaveUnmanaged<double>(s, StartTime); break;
                case 0x201: StreamHelper.SaveUnmanaged<double>(s, Speed); break;
                case 0x202: StreamHelper.SaveUnmanaged<double>(s, Pitch); break;
                case 0x203: StreamHelper.SaveUnmanaged<double>(s, Formant); break;
                case 0x204: StreamHelper.SaveUnmanaged<double>(s, Opacity); break;
                case 0x205: StreamHelper.SaveUnmanaged<double>(s, Volume); break;
                case 0x206: StreamHelper.SaveUnmanaged<double>(s, Pan); break;
                case 0x300: StreamHelper.SaveUnmanaged<double>(s, ModX); break;
                case 0x301: StreamHelper.SaveUnmanaged<double>(s, ModY); break;
                case 0x400: StreamHelper.SaveUnmanaged<uint>(s, Index); break;
                case 0x1000: StreamHelper.SaveUnmanaged<long>(s, ExtId); break;
                case 0x2000: UniLoad.Save(s, ExtType); break;
            }
        }
        public int ReportLenghtOfVariable(uint id)
        {
            switch (id)
            {
                case 0x100: return StreamHelper.GetUnmanagedLenght<long>(Position);
                case 0x101: return StreamHelper.GetUnmanagedLenght<long>(Length);
                case 0x200: return StreamHelper.GetUnmanagedLenght<double>(StartTime);
                case 0x201: return StreamHelper.GetUnmanagedLenght<double>(Speed);
                case 0x202: return StreamHelper.GetUnmanagedLenght<double>(Pitch);
                case 0x203: return StreamHelper.GetUnmanagedLenght<double>(Formant);
                case 0x204: return StreamHelper.GetUnmanagedLenght<double>(Opacity);
                case 0x205: return StreamHelper.GetUnmanagedLenght<double>(Volume);
                case 0x206: return StreamHelper.GetUnmanagedLenght<double>(Pan);
                case 0x300: return StreamHelper.GetUnmanagedLenght<double>(ModX);
                case 0x301: return StreamHelper.GetUnmanagedLenght<double>(ModY);
                case 0x400: return StreamHelper.GetUnmanagedLenght<uint>(Index);
                case 0x1000: return StreamHelper.GetUnmanagedLenght<long>(ExtId);
                case 0x2000: return UniLoad.GetLenght(ExtType);
                default: return 0;
            }
        }
        public List<uint> GetVarNamesToSave()
        {
            return new List<uint>() { 0x100, 0x101, 0x200, 0x201, 0x202, 0x203, 0x204, 0x205, 0x206, 0x300, 0x301, 0x400, 0x1000, 0x2000 };
        }
        public Media()
        {
            SetDefaultState();
        }
    }

    partial class VideoFX : IComplexObject
    {
        public FXFloatParam[] MainParams { get; private set; }
        public long ExtId { get; private set; }
        public IVFXType ExtType { get; private set; }

        public bool AcceptVariable(uint id, Stream s, int lenght)
        {
            switch (id)
            {
                case 0x100: MainParams = StreamHelper.LoadArray<FXFloatParam>(s, (ss) => { return UniLoad.CreateObject<FXFloatParam>(ss); }); return true;
                case 0x1000: ExtId = StreamHelper.LoadUnmanaged<long>(s); return true;
                case 0x2000: ExtType = /*TODO*/null; ExtType.SetDefaultState(); ExtType.LoadObject(s); return true;
                default: return false;
            }
        }
        public void SetDefaultState()
        {
            MainParams = new FXFloatParam[0];
            ExtId = 0;
            ExtType = null;
        }
        public void SaveVariable(uint id, Stream s)
        {
            switch (id)
            {
                case 0x100: StreamHelper.SaveArray(s, MainParams, (ss, MainParams0) => { UniLoad.Save(ss, MainParams0); }); break;
                case 0x1000: StreamHelper.SaveUnmanaged<long>(s, ExtId); break;
                case 0x2000: UniLoad.Save(s, ExtType); break;
            }
        }
        public int ReportLenghtOfVariable(uint id)
        {
            switch (id)
            {
                case 0x100: return StreamHelper.GetLenght<FXFloatParam>(MainParams, MainParams0 => { return UniLoad.GetLenght(MainParams0); });
                case 0x1000: return StreamHelper.GetUnmanagedLenght<long>(ExtId);
                case 0x2000: return UniLoad.GetLenght(ExtType);
                default: return 0;
            }
        }
        public List<uint> GetVarNamesToSave()
        {
            return new List<uint>() { 0x100, 0x1000, 0x2000 };
        }
        public VideoFX()
        {
            SetDefaultState();
        }
    }

    partial class AudioFX : IComplexObject
    {
        public FXFloatParam[] MainParams { get; private set; }
        public long ExtId { get; private set; }
        public IAFXType ExtType { get; private set; }

        public bool AcceptVariable(uint id, Stream s, int lenght)
        {
            switch (id)
            {
                case 0x100: MainParams = StreamHelper.LoadArray<FXFloatParam>(s, (ss) => { return UniLoad.CreateObject<FXFloatParam>(ss); }); return true;
                case 0x1000: ExtId = StreamHelper.LoadUnmanaged<long>(s); return true;
                case 0x2000: ExtType = /*TODO*/null; ExtType.SetDefaultState(); ExtType.LoadObject(s); return true;
                default: return false;
            }
        }
        public void SetDefaultState()
        {
            MainParams = new FXFloatParam[0];
            ExtId = 0;
            ExtType = null;
        }
        public void SaveVariable(uint id, Stream s)
        {
            switch (id)
            {
                case 0x100: StreamHelper.SaveArray(s, MainParams, (ss, MainParams0) => { UniLoad.Save(ss, MainParams0); }); break;
                case 0x1000: StreamHelper.SaveUnmanaged<long>(s, ExtId); break;
                case 0x2000: UniLoad.Save(s, ExtType); break;
            }
        }
        public int ReportLenghtOfVariable(uint id)
        {
            switch (id)
            {
                case 0x100: return StreamHelper.GetLenght<FXFloatParam>(MainParams, MainParams0 => { return UniLoad.GetLenght(MainParams0); });
                case 0x1000: return StreamHelper.GetUnmanagedLenght<long>(ExtId);
                case 0x2000: return UniLoad.GetLenght(ExtType);
                default: return 0;
            }
        }
        public List<uint> GetVarNamesToSave()
        {
            return new List<uint>() { 0x100, 0x1000, 0x2000 };
        }
        public AudioFX()
        {
            SetDefaultState();
        }
    }

    partial class FXFloatParam : IComplexObject
    {
        public float MinValue { get; private set; }
        public float MaxValue { get; private set; }
        public float DefaultIncrement { get; private set; }
        public float CurrentValue { get; private set; }
        public long AutomationId { get; private set; }
        public IAutomation AutomationType { get; private set; }

        public bool AcceptVariable(uint id, Stream s, int lenght)
        {
            switch (id)
            {
                case 0x100: MinValue = StreamHelper.LoadUnmanaged<float>(s); return true;
                case 0x101: MaxValue = StreamHelper.LoadUnmanaged<float>(s); return true;
                case 0x102: DefaultIncrement = StreamHelper.LoadUnmanaged<float>(s); return true;
                case 0x200: CurrentValue = StreamHelper.LoadUnmanaged<float>(s); return true;
                case 0x1000: AutomationId = StreamHelper.LoadUnmanaged<long>(s); return true;
                case 0x2000: AutomationType = /*TODO*/null; AutomationType.SetDefaultState(); AutomationType.LoadObject(s); return true;
                default: return false;
            }
        }
        public void SetDefaultState()
        {
            MinValue = 0f;
            MaxValue = 1f;
            DefaultIncrement = 0.001f;
            CurrentValue = 0f;
            AutomationId = 0;
            AutomationType = null;
        }
        public void SaveVariable(uint id, Stream s)
        {
            switch (id)
            {
                case 0x100: StreamHelper.SaveUnmanaged<float>(s, MinValue); break;
                case 0x101: StreamHelper.SaveUnmanaged<float>(s, MaxValue); break;
                case 0x102: StreamHelper.SaveUnmanaged<float>(s, DefaultIncrement); break;
                case 0x200: StreamHelper.SaveUnmanaged<float>(s, CurrentValue); break;
                case 0x1000: StreamHelper.SaveUnmanaged<long>(s, AutomationId); break;
                case 0x2000: UniLoad.Save(s, AutomationType); break;
            }
        }
        public int ReportLenghtOfVariable(uint id)
        {
            switch (id)
            {
                case 0x100: return StreamHelper.GetUnmanagedLenght<float>(MinValue);
                case 0x101: return StreamHelper.GetUnmanagedLenght<float>(MaxValue);
                case 0x102: return StreamHelper.GetUnmanagedLenght<float>(DefaultIncrement);
                case 0x200: return StreamHelper.GetUnmanagedLenght<float>(CurrentValue);
                case 0x1000: return StreamHelper.GetUnmanagedLenght<long>(AutomationId);
                case 0x2000: return UniLoad.GetLenght(AutomationType);
                default: return 0;
            }
        }
        public List<uint> GetVarNamesToSave()
        {
            return new List<uint>() { 0x100, 0x101, 0x102, 0x200, 0x1000, 0x2000 };
        }
        public FXFloatParam()
        {
            SetDefaultState();
        }
    }



}
