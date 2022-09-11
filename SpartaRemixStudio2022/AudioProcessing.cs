using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaRemixStudio2022
{
    public class TrackReaderISP : ISampleProvider
    {
        public WaveFormat WaveFormat => WaveFormat.CreateIeeeFloatWaveFormat(48000, 2);
        ITrackAudioReader reader = null;
        long currentPosition = 0;

        public TrackReaderISP(Track t, long position)
        {
            reader = t.ExtType?.GetAudio(position / 48000f);
            currentPosition = position;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            Array.Clear(buffer, offset, count);

            if (reader == null) return count;

            reader.Read(buffer, count, currentPosition / 48000f, 0, 1, 0, 0, 0);
            currentPosition += count / 2;

            // shift if required by offset
            if (offset != 0)
            {
                for (int i = count - 1; i >= 0; i--)
                {
                    buffer[offset + i] = buffer[i];
                }
                for (int i = 0; i < offset; i++) buffer[i] = 0;
            }

            return count;
        }
    }
}
