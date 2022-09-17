﻿using NAudio.Wave;
using OpenTK;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;

namespace SpartaRemixStudio2022
{
    // TODO apply FX
    // regular track
    [Export(typeof(ITrackFactory))]
    [ExportMetadata("ID", 8531611222441738669L)]
    [ExportMetadata("Name", "Regular track")]
    [ExportMetadata("Author", "SRS-default")]
    [ExportMetadata("Description", "Default track type.\nMedia that caries video and/or audio information is played as expected.\nBefore processing the main track, all child tracks are processed.\nAudio and video effects are applied as expected.\nFor rendering, both 2D and 3D modes are supported, as well as any blend mode that can trivialy be achived with OpenGL.")]
    class RegularTrackFactory : ITrackFactory
    {
        public ITrackType CreateNewInstance(Track parent)
        {
            return new RegularTrack(parent);
        }
    }
    partial class RegularTrack : ITrackType
    {
        private Track parent;

        public bool HasVideo => true;
        public bool HasAudio => true;
        public bool HasVideoEffects => true;
        public bool HasAudioEffects => true;
        public bool CanHaveChildren => true;
        public ITrackAudioReader GetAudio(long time)
        {
            return new RegularTrackAudioReader(parent, time);
        }
        public ITrackVideoReader GetVideo(long time)
        {
            return new RegularTrackVideoReader(parent, time);
        }

        public RegularTrack(Track parent)
        {
            this.parent = parent;
        }
    }
    class RegularTrackVideoReader : ITrackVideoReader
    {
        Media currentMedia = null;
        readonly Track readTrack = null;
        IVideoSampleReader currentReader = null;
        int mediaPos = -1;

        public RegularTrackVideoReader(Track track, long position)
        {
            Tuple<Media, int> current = track.BeginAt(position);
            currentMedia = current.Item1;
            mediaPos = current.Item2;
            readTrack = track;

            if (currentMedia != null) StartReadingMedia(currentMedia, position);
        }

        void StartReadingMedia(Media m, long currentPos)
        {
            if (m.ExtType.HasVideo)
            {
                currentReader = m.GetVideoReaderDefault((long)currentPos);
            }
            else
            {
                currentReader = null;
            }
        }
        void CheckForNewMedia(long currentPos)
        {
            Tuple<Media, int> newm = readTrack.CheckForNewMediaFast(mediaPos, (long)currentPos);
            if (newm.Item1 != null)
            {
                currentMedia = newm.Item1;
                mediaPos = newm.Item2;
                StartReadingMedia(currentMedia, currentPos);
            }
        }
        void CheckCurrentMediaStoped(long currentPos)
        {
            if (currentMedia.Position + currentMedia.Length < currentPos)
            {
                currentMedia = null;
                currentReader = null;
                currentReader = null;
            }
        }

        public TextureInfo Read(long position)
        {
            CheckForNewMedia(position);
            CheckCurrentMediaStoped(position);
            if (currentReader != null) return currentReader.ReadOne(position - (float)currentMedia.StartTime, currentMedia.Pitch, currentMedia.Speed, currentMedia.Formant, currentMedia.ModX, currentMedia.ModY);
            else return TextureInfo.None;
        }
    }
    class RegularTrackAudioReader : ITrackAudioReader
    {
        Media currentMedia = null;
        readonly Track readTrack = null;
        IAudioSampleReader currentReader = null;
        int mediaPos = -1;
        long currentPos = 0;
        float[] tempBuffer = new float[100]; 

        public RegularTrackAudioReader(Track track, long position)
        {
            Tuple<Media, int> current = track.BeginAt(position);
            currentMedia = current.Item1;
            mediaPos = current.Item2;
            currentPos = position;
            readTrack = track;

            if (currentMedia != null) StartReadingMedia(currentMedia, position);
        }

        void StartReadingMedia(Media m, float currentPos)
        {
            if (m.ExtType.HasAudio)
            {
                currentReader = m.GetAudioReaderDefault((long)currentPos);
            }
            else
            {
                currentReader = null;
            }
        }
        void CheckForNewMedia()
        {
            Tuple<Media, int> newm = readTrack.CheckForNewMediaFast(mediaPos, currentPos);
            if (newm.Item1 != null)
            {
                currentMedia = newm.Item1;
                mediaPos = newm.Item2;
                StartReadingMedia(currentMedia, currentPos);
            }
        }
        void CheckCurrentMediaStoped()
        {
            if (currentMedia != null && currentMedia.Position + currentMedia.Length < currentPos)
            {
                currentMedia = null;
                currentReader = null;
                currentReader = null;
            }
        }

        public void Read(float[] buffer, int count, long position)
        {

            // TODO: Audio precision quality constant (50 * 2)
            int left = count;
            int part = 0;

            while (left > 0)
            {
                CheckForNewMedia();
                CheckCurrentMediaStoped();
                if (currentReader != null)
                {
                    int toRead = Math.Min(left, 100);
                    currentReader.ReadMore(tempBuffer, toRead, position, 0, 1, 0, 0, 0);
                    for (int i = 0; i < toRead; i++)
                    {
                        buffer[i + 100 * part] = tempBuffer[i];
                    }
                }
                left -= 100;
                this.currentPos += 100 / 2;
                part++;
            }
        }
    }

    // video cut
    [Export(typeof(IVideoSample))]
    [ExportMetadata("ID", 2063460045581655728L)]
    [ExportMetadata("Name", "Video cut")]
    [ExportMetadata("Author", "SRS-default")]
    [ExportMetadata("Description", "Default video sample type.\nIt contains preloaded frames to bypass the need to read video by reusing frames.")]
    partial class VideoCutSample : IVideoSample
    {
        public long FactoryID { get; } = 2063460045581655728L;

        readonly int width;
        readonly int height;
        readonly int fpsN;
        readonly int fpsD;

        List<int> OpenGLFrames = new List<int>();
        public VideoCutSample(VideoSource vs, double sourceTimeSec, int framesToPreload)
        {
            OpenGLFrames = vs.GetFrames(sourceTimeSec, framesToPreload);
            SourceFile = vs.File;
            SourceTime = sourceTimeSec;
            SourceIndex = vs.Index;

            width = vs.Width;
            height = vs.Height;
            fpsN = vs.Fpsn;
            fpsD = vs.Fpsd;
        }

        public IVideoSampleReader GetReader(double position, double pitch, double speed, double formant, double modx, double mody)
        {
            throw new NotImplementedException();
        }

        public TextureInfo GetFrameAt(double timeSec)
        {
            int frameNumber = (int)(timeSec * fpsN / fpsD);
            frameNumber = Math.Min(OpenGLFrames.Count, Math.Max(0, frameNumber));
            return new TextureInfo() { RelativeHeight = height, RelativeWidth = width, TextureIndex = OpenGLFrames[frameNumber], Transformation = Matrix4.Identity };
        }
    }
    class VideoCutReader : IVideoSampleReader
    {
        VideoCutSample vcs;

        public VideoCutReader(VideoCutSample vcs)
        {
            this.vcs = vcs;
        }
        public TextureInfo ReadOne(double position, double pitch, double speed, double formant, double modx, double mody)
        {
            return vcs.GetFrameAt(position);
        }
    }

    // audio cut
    [Export(typeof(IAudioSample))]
    [ExportMetadata("ID", 7735279469303714768L)]
    [ExportMetadata("Name", "Audio cut")]
    [ExportMetadata("Author", "SRS-default")]
    [ExportMetadata("Description", "Default audio sample type.\nIt contains a preloaded section of audio and can be pitch-shifted or stretched.")]
    partial class AudioCutSample : IAudioSample
    {
        public long FactoryID { get; } = 7735279469303714768L;

        public AudioCutSample(int channels, float sampleFreq, float[] audio)
        {
            Channels = channels;
            SampleFreq = sampleFreq;
            Audio = audio;
            DefaultPitch = 0;
            DefaultSpeed = 1;
            DefaultVolume = 1;

            if (Audio == null) Audio = new float[0];
        }
        public AudioCutSample(int channels, float sampleFreq, float[] audio, VideoSource vs, float timeSec)
        {
            Channels = channels;
            SampleFreq = sampleFreq;
            Audio = audio;
            DefaultPitch = 0;
            DefaultSpeed = 1;
            DefaultVolume = 1;
            SourceFile = vs.File;
            SourceIndex = vs.Index;
            SourceTime = timeSec;

            if (Audio == null) Audio = new float[0];
        }
        
        // TODO: shift stretch position
        public IAudioSampleReader GetReader(double position, double pitch, double speed, double formant, double modx, double mody)
        {
            return new AudioCutReader(this);
        }
    }
    class AudioCutReader : IAudioSampleReader
    {
        AudioCutSample acs;
        int position = 0;

        public AudioCutReader(AudioCutSample acs)
        {
            this.acs = acs;
        }

        public void ReadMore(float[] buffer, int count, double position, double pitch, double speed, double formant, double modx, double mody)
        {
            int toRead = Math.Min(count, acs.Audio.Length - this.position);

            for (int i = 0; i < toRead; i++)
            {
                buffer[i] = acs.Audio[this.position + i];
            }
            this.position += toRead;
        }
        public float ReadOne(double position, double pitch, double speed, double formant, double modx, double mody)
        {
            if (this.position < acs.Audio.Length)
            {
                this.position++;
                return acs.Audio[this.position - 1];
            }
            return 0;
        }
    }

    [Export(typeof(ISampleDefiner))]
    [ExportMetadata("ID", 7735279469303714768L)]
    [ExportMetadata("Name", "Define cut sample")]
    [ExportMetadata("Author", "SRS-default")]
    [ExportMetadata("Description", "Default function, that given start and end position in a source, defines a sample that can be shifted using general pitch technique and that has its frames preloaded.")]
    class CutDefiner : ISampleDefiner
    {
        public void Open(Project p, VideoSource vs, float timeFromSec, float timeToSec)
        {
            IAudioSample acs = null;
            IVideoSample vcs = null;

            ISampleProvider isp = vs.GetAudioReader(timeFromSec);
            if (vs.hasAudio)
            {
                float lengthSec = timeToSec - timeFromSec;
                int length = (int)(96000 * (lengthSec));
                length += length % 2;
                float[] audio = new float[length];
                isp.Read(audio, 0, audio.Length);
                acs = new AudioCutSample(2, 48000, audio, vs, timeFromSec);
            }

            if (vs.hasVideo)
            {
                vcs = new VideoCutSample(vs, timeFromSec, 16);
            }
            
            if (acs != null || vcs != null)
            {
                p.AddSample(acs, vcs);
            }
        }
    }

    class SampleMedia : IMediaType
    {
        readonly int sampleID;
        public SampleMedia(int sampleID)
        {
            this.sampleID = sampleID;
        }

        //TODO: fix
        public bool HasVideo => true;
        public bool HasAudio => true;

        public IAudioSample RepresentedAudio { get; private set; }
        public IVideoSample RepresentedVideo { get; private set; }
        NameColor SampleName = new NameColor();

        public void Init(Project p)
        {
            AVSample avs = p.GetSampleByID(sampleID);
            if (avs != null)
            {
                RepresentedAudio = avs.AudioSample;
                RepresentedVideo = avs.VideoSample;
                SampleName = avs.NameColor;
            }
        }

        public IAudioSampleReader GetAudioReader(long position)
        {
            return RepresentedAudio?.GetReader(position / 48000f, 1, 1, 1, 0, 0);
        }
        public IVideoSampleReader GetVideoReader(long position)
        {
            return RepresentedVideo?.GetReader(position / 48000f, 1, 1, 1, 0, 0);
        }

        public NameColor GetNameColor()
        {
            return new NameColor()
            {
                Name = $"[{sampleID}] {SampleName.Name}",
                R = SampleName.R,
                G = SampleName.G,
                B = SampleName.B
            };
        }

        public bool AcceptVariable(uint id, Stream s, int lenght)
        {
            throw new NotImplementedException();
        }    
        public List<uint> GetVarNamesToSave()
        {
            throw new NotImplementedException();
        }
        public int ReportLenghtOfVariable(uint id)
        {
            throw new NotImplementedException();
        }
        public void SaveVariable(uint id, Stream s)
        {
            throw new NotImplementedException();
        }
        public void SetDefaultState()
        {
            throw new NotImplementedException();
        }
    }

    // audio cut
    // shifting?
    // speed up?
    // UAP, nebo (RT)psola?

    // GENERAL SCRIPT - ASI NE
    // VFX - HODNE - ve vlastnim souboru
    // AFX - HODNE - VVS
    // TRACK SCRIPT - ASI NE
    // MEDIA SCRIPT - ASI NE
    // !!! SAMPLE DEFINER

    // TODO SMP library
}
