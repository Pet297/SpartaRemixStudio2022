using NAudio.MediaFoundation;
using NAudio.Wave;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SpartaRemixStudio2022
{
    public partial class Project
    {
        // Sources
        public void AddSource(string File)
        {
            //TODO: Already included?
            Sources.Add(NextSourceIndex, new VideoSource(File, NextSourceIndex));

            EventHandler handler = SourceAdded;
            handler?.Invoke(this, new EventArgs());

            NextSourceIndex++;
        }
        public event EventHandler SourceAdded;
        public IEnumerable<VideoSource> GetSources => Sources.Values;
        public void AddSample(IAudioSample audio, IVideoSample video)
        {
            Samples.Add(NextSampleIndex, new AVSample(audio, video, NextSampleIndex));

            EventHandler handler = SampleAdded;
            handler?.Invoke(this, new EventArgs());

            NextSampleIndex++;
        }
        public event EventHandler SampleAdded;
        public IEnumerable<AVSample> GetSamples => Samples.Values;

        // Settings
        public int SampleRate = 48000;

        // Other Media

        // Extenstions
        public ExtensionManager extensions = new ExtensionManager();
    }
    public partial class VideoSource
    {
        // Derived values
        public readonly bool hasAudio;
        public readonly bool hasVideo;
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

                string nd = VideoInfo.GetSize(File);
                string[] nd2 = nd.Split('/');
                Fpsn = int.Parse(nd2[0]);
                Fpsd = int.Parse(nd2[1]);
            }
        }

        public List<int> GetFrames(double timeSec, int frameCount)
        {
            CMDVideoReader cmdv = new CMDVideoReader();
            cmdv.OpenFile(File, (int)(timeSec * 1000));


            int[] frames = new int[frameCount];
            GL.CreateTextures(TextureTarget.Texture2D, 1, frames);

            for (int i = 0; i < frameCount; i++)
            {
                cmdv.Read();
                GL.BindTexture(TextureTarget.Texture2D, frames[i]);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, Width, Height, 0, PixelFormat.Bgr, PixelType.UnsignedByte, cmdv.Buffer);
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
    }

    public partial class Timeline
    {
    }
    public partial class TimelineGridline
    {
        public TimelineGridline(long position, float maxSamplesPerPixel, string name)
        {
            Position = position;
            MaxSamplesPerPixel = maxSamplesPerPixel;
            Name = name;
        }
    }
    public partial class Pattern
    {

    }
    public partial class SimpleTrack
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

    public partial class Track
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
        public ReadOnlyCollection<Media> GetMedia => TrackMedia.AsReadOnly();

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
            while (currentIndex + 1 < TrackMedia.Count && TrackMedia[currentIndex + 1].StartTime < newTimeStamp)
            {
                newMedia = new Tuple<Media, int>(TrackMedia[currentIndex + 1], currentIndex + 1);
                currentIndex++;
            }
            return newMedia;
        }
        public Tuple<Media, int> BeginAt(long timeStamp) => CheckForNewMediaFast(-1, timeStamp);
    }
    public partial class Media
    {
        public Media(IMediaType mt)
        {
            ExtType = mt;
            //TODO: factory ID
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

    public static class DefaultExtensions
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
}
