using NAudio.Wave;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SpartaRemixStudio2022
{
    public interface IEditableTimeline<T> where T : IEditableTrack
    {
        float Tempo { get; }
        List<T> Tracks { get; }
    }
    public interface IEditableTrack
    {
        void AddMedia(Media m);
        void RemoveMedia(Media m);
        IEnumerable<Media> GetMedia { get; }
    }

    public partial class Project
    {
        // Sources
        public int AddSource(string File)
        {
            //TODO: Already included?
            Sources.Add(NextSourceIndex, new VideoSource(File, NextSourceIndex));

            EventHandler handler = SourcesChanged;
            handler?.Invoke(this, new EventArgs());

            NextSourceIndex++;

            return NextSourceIndex - 1;
        }
        public int AddSample(IAudioSample audio, IVideoSample video)
        {
            Samples.Add(NextSampleIndex, new AVSample(audio, video, NextSampleIndex) { NameColor = new NameColor() { R = 36, G = 72, B = 0, Name = $"Sample {NextSampleIndex}" } });

            EventHandler handler = SamplesChanged;
            handler?.Invoke(this, new EventArgs());

            NextSampleIndex++;

            return NextSampleIndex - 1;
        }
        public int AddPattern()
        {
            Patterns.Add(NextPatternIndex, new Pattern(NextPatternIndex));

            EventHandler handler = PatternsChanged;
            handler?.Invoke(this, new EventArgs());

            NextPatternIndex++;

            return NextPatternIndex - 1;
        }

        public void UpdateSources()
        {
            EventHandler handler = SourcesChanged;
            handler?.Invoke(this, new EventArgs());
        }
        public void UpdateSamples()
        {
            EventHandler handler = SamplesChanged;
            handler?.Invoke(this, new EventArgs());
        }
        public void UpdatePatterns()
        {
            EventHandler handler = PatternsChanged;
            handler?.Invoke(this, new EventArgs());
        }

        public event EventHandler SourcesChanged;
        public IEnumerable<VideoSource> GetSources => Sources.Values;
        public event EventHandler SamplesChanged;
        public IEnumerable<AVSample> GetSamples
        {
            get
            {
                List<AVSample> samples = new List<AVSample>(Samples.Values);
                samples.Sort((a, b) => a.DisplayIndex - b.DisplayIndex);
                return samples;
            }
        }
        public event EventHandler PatternsChanged;
        public IEnumerable<Pattern> GetPatterns
        {
            get
            {
                List<Pattern> patterns = new List<Pattern>(Patterns.Values);
                patterns.Sort((a, b) => a.DisplayIndex - b.DisplayIndex);
                return patterns;
            }
        }

        public AVSample GetSampleByID(int id) => Samples.ContainsKey(id) ? Samples[id] : null;
        public VideoSource GetSourceByID(int id) => Sources.ContainsKey(id) ? Sources[id] : null;
        public Pattern GetPatternByID(int id) => Patterns.ContainsKey(id) ? Patterns[id] : null;

        // Settings
        public int SampleRate = 48000;

        public void DoPostLoadActions()
        {
            foreach (Track t in timeline.Tracks)
            {
                foreach (Media m in t.GetMedia)
                {
                    m.ExtType.Init(this);
                }
                t.Init(this);
            }
            foreach (AVSample avs in GetSamples)
            {
                avs.VideoSample?.Init(this);
            }
            foreach (VideoSource vs in GetSources)
            {
                vs.Init(this);
            }
        }
    }
    public partial class VideoSource
    {
        // Derived values
        public bool hasAudio { get; private set; }
        public bool hasVideo { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Fpsn { get; private set; }
        public int Fpsd { get; private set; }

        public VideoSource(string file, int index)
        {
            File = file;
            hasAudio = VideoInfo.IsAudioFile(file);
            hasVideo = VideoInfo.IsVideoFile(file);
            GeneratePreviewWave();
            GetVideoInfo();
            Index = index;
        }

        public ISampleProvider GetAudioReader(float timeSec)
        {
            if (!hasAudio) return null;
            AudioFileReader afr = new AudioFileReader(File);

            long pos = (long)(timeSec * afr.WaveFormat.AverageBytesPerSecond);
            pos = (pos * 32) / 32;
            afr.Position = pos;

            ISampleProvider isp = afr;

            if (isp.WaveFormat.SampleRate != 48000) isp = new NAudio.Wave.SampleProviders.WdlResamplingSampleProvider(isp, 48000);
            if (isp.WaveFormat.Channels != 2) isp = isp.ToStereo();

            return isp;
        }
        public void GeneratePreviewWave()
        {
            if (hasAudio) PreviewWave = PreviewWave.FromProvider(new AudioFileReader(File), 400);
        }
        public void GetVideoInfo()
        {
            if (hasVideo)
            {
                string wh = VideoInfo.GetSize(File);
                string[] wh2 = wh.Split('x');
                Width = int.Parse(wh2[0]);
                Height = int.Parse(wh2[1]);

                string nd = VideoInfo.GetFPS(File);
                string[] nd2 = nd.Split('/');
                Fpsn = int.Parse(nd2[0]);
                Fpsd = int.Parse(nd2[1]);
            }
        }

        public void Init(Project p)
        {
            hasAudio = VideoInfo.IsAudioFile(File);
            hasVideo = VideoInfo.IsVideoFile(File);
            GetVideoInfo();
        }

        public List<int> GetFrames(double timeSec, int frameCount)
        {
            CMDVideoReader cmdv = new CMDVideoReader();
            cmdv.OpenFile(File, (int)(timeSec * 1000));


            int[] frames = new int[frameCount];
            GL.CreateTextures(TextureTarget.Texture2D, frameCount, frames);

            for (int i = 0; i < frameCount; i++)
            {
                cmdv.Read();
                GL.BindTexture(TextureTarget.Texture2D, frames[i]);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, Width, Height, 0, PixelFormat.Bgr, PixelType.UnsignedByte, cmdv.Buffer);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);
            }

            return frames.ToList();
        }
    }
    public partial class PreviewWave
    {
        public PreviewWave(float[] values, float pointsPerSecond)
        {
            this.Values = values;
            this.PointsPerSecond = pointsPerSecond;
        }
        public float GetPoint(float timeSec)
        {
            int next = (int)(timeSec * PointsPerSecond) + 1;
            int cur = (int)(timeSec * PointsPerSecond);
            float p = timeSec % 1f;
            float vc = 0;
            float vn = 0;

            if (cur >= 0 && cur < Values.Length) vc = Values[cur];
            if (next >= 0 && next < Values.Length) vn = Values[next];

            return (1 - p) * vc + p * vn;
        }

        public static PreviewWave FromProvider(ISampleProvider isp, int resolution)
        {
            float[] temp = new float[resolution];
            List<float> values = new List<float>();
            while (true)
            {
                int c = isp.Read(temp, 0, resolution);
                if (c == 0) break;
                float max = 0;
                for (int i = 0; i < resolution; i++)
                {
                    if (Math.Abs(temp[i]) > max)
                    {
                        max = Math.Abs(temp[i]);
                    }
                }
                values.Add(max);
            }
            return new PreviewWave(values.ToArray(), (float)isp.WaveFormat.SampleRate * isp.WaveFormat.Channels / resolution);
        }
    }
    public partial class AVSample
    {
        public AVSample(IAudioSample audioSample, IVideoSample videoSample, int index)
        {
            AudioSample = audioSample;
            if (AudioSample != null) AudioId = audioSample.FactoryID;
            else AudioId = 0;
            VideoSample = videoSample;
            if (VideoSample != null) VideoId = videoSample.FactoryID;
            else VideoId = 0;
            Index = index;
        }

        public override string ToString()
        {
            return NameColor.Name;
        }
    }

    public partial class NameColor
    {

    }

    public partial class Timeline : IEditableTimeline<Track>
    {
    }
    public partial class Pattern : IEditableTimeline<SimpleTrack>
    {
        public Pattern(int index)
        {
            this.Index = index;
            this.NameColor = new NameColor() { R = 20, G = 20, B = 20, Name = "Pattern" };
            this.DisplayIndex = index;
            this.Tracks = new List<SimpleTrack>();
            this.Tracks.Add(new SimpleTrack());
        }
    }
    public partial class SimpleTrack : IEditableTrack
    {
        public void SortMedia() => TrackMedia.Sort((a, b) => { return a.Position.CompareTo(b.Position); });
        public void AddMedia(Media m)
        {
            TrackMedia.Add(m);
            SortMedia();
        }
        public void RemoveMedia(Media m)
        {
            TrackMedia.Remove(m);
        }
        public IEnumerable<Media> GetMedia => TrackMedia.AsReadOnly();

        public Tuple<Media, int> CheckForNewMediaFast(int currentIndex, long newTimeStamp)
        {
            Tuple<Media, int> newMedia = new Tuple<Media, int>(null, currentIndex);
            while (currentIndex + 1 < TrackMedia.Count && TrackMedia[currentIndex + 1].StartTime < newTimeStamp)
            {
                newMedia = new Tuple<Media, int>(TrackMedia[currentIndex + 1], currentIndex + 1);
                currentIndex++;
            }
            return newMedia;
        }
        public Tuple<Media, int> BeginAt(long timeStamp) => CheckForNewMediaFast(-1, timeStamp);
    }

    public partial class Track : IEditableTrack
    {
        public void SortMedia()
        {
            TrackMedia.Sort((a, b) => { return a.Position.CompareTo(b.Position); });
            trackMediaInterpreted.Sort((a, b) => { return a.Position.CompareTo(b.Position); });
        }
        public void AddMedia(Media m)
        {
            TrackMedia.Add(m);
            SortMedia();
        }
        public void RemoveMedia(Media m)
        {
            TrackMedia.Remove(m);
        }
        public IEnumerable<Media> GetMedia => TrackMedia.AsReadOnly();

        public void SetType(ITrackType trackType)
        {
            ExtType = trackType;
            ExtId = trackType.FactoryID;
        }

        public Media ColideMedia(long cursor)
        {
            foreach(Media m in TrackMedia)
            {
                if (m.Position <= cursor && m.Position + m.Length >= cursor)
                {
                    return m;
                }
            }
            return null;
        }

        public Tuple<Media, int> CheckForNewMediaFast(int currentIndex, long newTimeStamp)
        {
            Tuple<Media, int> newMedia = new Tuple<Media, int>(null, currentIndex);
            while (currentIndex + 1 < trackMediaInterpreted.Count && trackMediaInterpreted[currentIndex + 1].Position <= newTimeStamp)
            {
                newMedia = new Tuple<Media, int>(trackMediaInterpreted[currentIndex + 1], currentIndex + 1);
                currentIndex++;
            }
            return newMedia;
        }
        public Tuple<Media, int> BeginAt(long timeStamp) => CheckForNewMediaFast(-1, timeStamp);

        private readonly List<Media> trackMediaInterpreted = new List<Media>();
        private void Reinterpret()
        {
            trackMediaInterpreted.Clear();
            foreach (Media m in TrackMedia)
            {
                if (m.ExtType is PatternMedia pm)
                {
                    Pattern pat = p.GetPatternByID(pm.PatternID);
                    if (pat != null && pm.PatternTrack >= 0 && pm.PatternTrack < pat.Tracks.Count)
                    {
                        SimpleTrack st = new SimpleTrack();
                        foreach (Media m2 in st.GetMedia)
                        {
                            Media n = new Media(m.ExtType);

                            if (!(n.ExtType is PatternMedia))
                            {
                                // TODO: Examine the math and be more precise about stretching.
                                n.Formant = m2.Formant + m.Formant;
                                n.Index = m2.Index;
                                n.Length = (long)(m2.Length / m.Speed);
                                n.ModX = m2.ModX;
                                n.ModY = m2.ModY;
                                n.Opacity = m2.Opacity * m.Opacity;
                                n.Pan = m2.Pan + m.Pan;
                                n.Pitch = m2.Pitch + m.Pitch;
                                n.Position = m2.Position;
                                n.Speed = m2.Speed * m.Speed;
                                n.StartTime = m.StartTime + m.StartTime * m.Speed;

                                trackMediaInterpreted.Add(n);
                            }
                        }
                    }
                }
                else trackMediaInterpreted.Add(m);
            }
        }

        Project p;
        public void Init(Project p)
        {
            this.p = p;
            Reinterpret();
        }
    }
    public partial class Media
    {
        public Media(IMediaType mt)
        {
            ExtType = mt;
            ExtId = mt.FactoryID;
        }

        public NameColor GetNameColor()
        {
            return ExtType.GetNameColor();
        }
    }

    public partial class VideoFX
    {
        public int ParamCt => MainParams == null ? 0 : MainParams.Length;
        public FXFloatParam GetParam(int id) => (id >= 0 && id < ParamCt) ? MainParams[id] : null;

        /*public abstract bool HasForm { get; }
        public abstract void DisplayForm();

        public abstract float ApplyOne(TextureInfo texture);*/
    }
    public partial class AudioFX
    {
        public int ParamCt => MainParams == null ? 0 : MainParams.Length;
        public FXFloatParam GetParam(int id) => (id >= 0 && id < ParamCt) ? MainParams[id] : null;

        /*public bool HasForm { get; }
        public void DisplayForm();

        public void ApplyMore(float[] audio);
        public float ApplyOne(float audioSample);*/
    }
    public partial class FXFloatParam
    {
    }

    public static class ExtensionMethods
    {
        public static IAudioSampleReader GetAudioReaderDefault(this Media m, long position)
        {
            long positionIn = position - m.Position;
            return m.ExtType?.RepresentedAudio?.GetReader(m.StartTime + positionIn / 48_000f, m.Pitch, m.Speed, m.Formant, m.ModX, m.ModY);
        }
        public static IVideoSampleReader GetVideoReaderDefault(this Media m, long position)
        {
            long positionIn = position - m.Position;
            return m.ExtType?.RepresentedVideo?.GetReader(m.StartTime + positionIn / 48_000f, m.Pitch, m.Speed, m.Formant, m.ModX, m.ModY);
        }
    }
    public static class GridlineHelper
    {
        public static List<TimelineGridline> GetGridlines(long minimalDistanceSamples, float BPM, long timeFrom, long timeTo)
        {
            long resolution = 3;

            while (resolution * 60000 / BPM < minimalDistanceSamples)
            {
                resolution *= 2;
            }

            double timeStepSmp = resolution * 60000 / BPM;
            double indexFrom = timeFrom / timeStepSmp;
            double indexTo = timeTo / timeStepSmp;
            long indexFromR = (long)Math.Floor(indexFrom);
            long indexToR = (long)Math.Floor(indexTo);

            List<TimelineGridline> gridlines = new List<TimelineGridline>();
            for (long i = indexFromR; i <= indexToR; i++)
            {
                long trueIndex = resolution * i;
                Tuple<byte, byte, byte> color = GetGridlineColor(trueIndex);
                string text = GetGridlineText(trueIndex);

                gridlines.Add(new TimelineGridline((int)(i * timeStepSmp), text, color.Item1, color.Item2, color.Item3));
            }
            return gridlines;
        }
        private static Tuple<byte,byte,byte> GetGridlineColor(long trueIndex)
        {
            byte color = 20;
            if (trueIndex / 768 % 2 == 1) color = 15;
            if (trueIndex / 96 % 2 == 1) color += 6;
            if (trueIndex / 48 % 2 == 1) color += 3;
            if (trueIndex / 24 % 2 == 1) color += 3;
            if (trueIndex / 12 % 2 == 1) color += 3;
            if (trueIndex / 6 % 2 == 1) color += 3;
            return new Tuple<byte, byte, byte>(color, color, color);
        }
        private static string GetGridlineText(long trueIndex)
        {
            if (trueIndex % 48 == 0) return $"{trueIndex / 192}.{trueIndex / 48 % 4}";
            if (trueIndex % 12 == 0) return $"{trueIndex / 192}.{trueIndex / 48 % 4} {trueIndex / 12 % 4}/4";
            return "";
        }
    }
    public class TimelineGridline
    {
        public long Position { get; private set; }
        public string Name { get; private set; }
        public byte R { get; private set; }
        public byte G { get; private set; }
        public byte B { get; private set; }

        public TimelineGridline(long positionSamples, string text, byte r, byte g, byte b)
        {
            Position = positionSamples;
            Name = text;
            R = r;
            G = g;
            B = b;
        }
    }
}
