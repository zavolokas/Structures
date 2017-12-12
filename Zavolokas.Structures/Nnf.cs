
namespace Zavolokas.Structures
{
    public class Nnf
    {
        private readonly double[] _nnf;

        public Nnf(int dstWidth, int dstHeight, int srcWidth, int srcHeight)
            : this(dstWidth, dstHeight, srcWidth, srcHeight, 5)
        {
        }

        public Nnf(int dstWidth, int dstHeight, int srcWidth, int srcHeight, int patchSize)
        {
            DstWidth = dstWidth;
            DstHeight = dstHeight;
            SourceWidth = srcWidth;
            SourceHeight = srcHeight;
            PatchSize = patchSize;

            _nnf = new double[dstWidth * dstHeight * 2];
        }

        public int PatchSize { get; }

        public int SourceWidth { get; }
        public int SourceHeight { get; }

        public int DstWidth { get; }

        public int DstHeight { get; }

        public double[] GetNnfItems()
        {
            return _nnf;
        }

        public Nnf Clone()
        {
            var clone = new Nnf(DstWidth, DstHeight, SourceWidth, SourceHeight, PatchSize);
            _nnf.CopyTo(clone._nnf,0);

            return clone;
        }
    }
}