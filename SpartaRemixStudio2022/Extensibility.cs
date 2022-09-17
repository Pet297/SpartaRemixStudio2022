using OpenTK;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Windows.Forms;

namespace SpartaRemixStudio2022
{
    public interface ISRSExtensionData
    {
        long ID { get; }
        string Name { get; }
        string Author { get; }
        string Description { get; }
    }
    public class ExtensionManager
    {
        private CompositionContainer container;
        [ImportMany] public IEnumerable<Lazy<ITrackFactory, ISRSExtensionData>> TrackTypes;
        [ImportMany] public IEnumerable<Lazy<VideoFXFactory, ISRSExtensionData>> VideoEffects;
        [ImportMany] public IEnumerable<Lazy<AudioFXFactory, ISRSExtensionData>> AudioEffects;
        [ImportMany] public IEnumerable<Lazy<ITrackScript, ISRSExtensionData>> TrackScripts;
        [ImportMany] public IEnumerable<Lazy<IMediaScript, ISRSExtensionData>> MediaScripts;
        [ImportMany] public IEnumerable<Lazy<ISampleDefiner, ISRSExtensionData>> SampleDefiners;
        [ImportMany] public IEnumerable<Lazy<IVideoSample, ISRSExtensionData>> AudioSampleTypes;
        [ImportMany] public IEnumerable<Lazy<IAudioSample, ISRSExtensionData>> VideoSampleTypes;
        [ImportMany] public IEnumerable<Lazy<IGeneralScript, ISRSExtensionData>> GeneralScripts;

        public ExtensionManager()
        {
            if (!Directory.Exists(@"extensions"))
            {
                Directory.CreateDirectory(@"extensions");
            }
            //Extension finding code
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(Program).Assembly));
            catalog.Catalogs.Add(new DirectoryCatalog(@"extensions"));
            container = new CompositionContainer(catalog);
            try
            {
                container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
            }
        }
    }

    // scripting
    public interface IGeneralScript
    {
        bool Applicable(Project p);
        void Run(Project p);
    }
    public interface ISampleDefiner
    {
        void Open(Project p, VideoSource vs, float timeFromSec, float timeToSec);
    }
    public interface IMediaScript
    {
        bool Applicable(List<Media> selectedMedia);
        void Run(List<Media> selectedMedia);
    }
    public interface ITrackScript
    {
        bool Applicable(List<Track> selectedTracks);
        void Run(List<Track> selectedTracks);
    }

    // sampling
    public interface IAudioSample : IComplexObject
    {
        long FactoryID { get; }
        IAudioSampleReader GetReader(double position, double pitch, double speed, double formant, double modx, double mody);
    }
    public interface IAudioSampleReader
    {
        float ReadOne(double position, double pitch, double speed, double formant, double modx, double mody);
        void ReadMore(float[] buffer, int count, double position, double pitch, double speed, double formant, double modx, double mody);
    }
    public interface IVideoSample : IComplexObject
    {
        long FactoryID { get; }
        IVideoSampleReader GetReader(double position, double pitch, double speed, double formant, double modx, double mody);
    }
    public interface IVideoSampleReader
    {
        TextureInfo ReadOne(double position, double pitch, double speed, double formant, double modx, double mody);
    }
    public struct TextureInfo
    {
        public int TextureIndex;
        public int RelativeWidth;
        public int RelativeHeight;
        public Matrix4 Transformation;
        public int VS0;
        public int VS1;
        public int VS2;
        public int VS3;

        public static TextureInfo None
        {
            get
            {
                return new TextureInfo() { RelativeHeight = 1, RelativeWidth = 1, TextureIndex = -1, Transformation = Matrix4.Identity, VS0 = 0, VS1 = 0, VS2 = 0, VS3 = 0 };
            }
        }
        public bool IsNone => TextureIndex == -1;
    }

    // tracks
    public interface ITrackFactory
    {
        ITrackType CreateNewInstance(Track parent);
    }
    public interface ITrackType : IComplexObject
    {
        bool HasVideo { get; }
        bool HasAudio { get; }
        bool HasVideoEffects { get; }
        bool HasAudioEffects { get; }
        bool CanHaveChildren { get; }
        ITrackAudioReader GetAudio(long time);
        ITrackVideoReader GetVideo(long time);
    }
    public interface IMediaType : IComplexObject
    {
        bool HasVideo { get; }
        bool HasAudio { get; }

        IAudioSample RepresentedAudio { get; }
        IVideoSample RepresentedVideo { get; }
        IAudioSampleReader GetAudioReader(long position);
        IVideoSampleReader GetVideoReader(long position);

        NameColor GetNameColor();

        void Init(Project p);
    }
    public interface ITrackAudioReader
    {
        void Read(float[] buffer, int count, long position);
    }
    public interface ITrackVideoReader
    {
        TextureInfo Read(long position);
    }

    // media
    public abstract class MediaLibraryTab : UserControl
    {
        public abstract Media GetMedia(Project p);
    }

    // effects
    public abstract class VideoFXFactory
    {
        public abstract VideoFX CreateNewInstance();
    }
    public abstract class AudioFXFactory
    {
        public abstract AudioFX CreateNewInstance();
    }
    public interface IVFXType : IComplexObject
    {

    }
    public interface IAFXType : IComplexObject
    {

    }

    // automation
    public abstract class AutomationFactory
    {
        public abstract IAutomation CreateNewInstance();
    }
    public interface IAutomation : IComplexObject
    {
        float GetValue(float time);

        bool HasForm { get; }
        void DisplayForm();
    }
}
