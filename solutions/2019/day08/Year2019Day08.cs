using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2019._8;

[UsedImplicitly]
public class Year2019Day08 : ISolution
{
    public object Part1(IEnumerable<string> input)
    {
        var image = input.First().Select(x => x.ToString()).Select(int.Parse).ToArray();
        const int pixelWide = 25;
        const int pixelTall = 6;

        var layerCount = image.Length / (pixelWide * pixelTall);
        var layers = new List<List<List<int>>>();
        var index = 0;
        var fewestZero = int.MaxValue;
        var fewestZeroLayer = -1;
        for (var i = 0; i < layerCount; i++)
        {
            var col = new List<List<int>>();
            var zeroCount = 0;
            for (var j = 0; j < pixelTall; j++)
            {
                var row = new List<int>();
                for (var k = 0; k < pixelWide; k++)
                {
                    if (image[index] == 0)
                        zeroCount++;
                    row.Add(image[index]);
                    index++;
                }
                col.Add(row);
            }

            if (zeroCount < fewestZero)
            {
                fewestZero = zeroCount;
                fewestZeroLayer = i;
            }

            layers.Add(col);
        }

        var onesCount = 0;
        var twosCount = 0;
        for (var i = 0; i < layers[fewestZeroLayer].Count; i++)
        {
            for (var j = 0; j < layers[fewestZeroLayer][i].Count; j++)
            {
                if (layers[fewestZeroLayer][i][j] == 1)
                    onesCount++;
                if (layers[fewestZeroLayer][i][j] == 2)
                    twosCount++;
            }
        }
        
        return onesCount * twosCount;
    }


    public object Part2(IEnumerable<string> input)
    {
        var image = input.First().Select(x => x.ToString()).Select(int.Parse).ToArray();
        const int pixelWide = 25;
        const int pixelTall = 6;

        var layerCount = image.Length / (pixelWide * pixelTall);
        var layers = new List<List<int>>();
        var index = 0;
        for (var i = 0; i < layerCount; i++)
        {
            var col = new List<int>();
            for (var j = 0; j < pixelTall * pixelWide; j++)
            {
                col.Add(image[index]);
                index++;
            }

            layers.Add(col);
        }

        var final = new List<int>();
        for (int i = 0; i < pixelWide * pixelTall; i++)
        {
            foreach (var layer in layers)
            {
                if (layer[i] == 2)
                    continue;
                final.Add(layer[i]);
                break;
            }
        }

        var sb = new StringBuilder();
        sb.AppendLine();
        for (var i = 0; i < final.Count; i++)
        {
            if (final[i] == 1)
                sb.Append(final[i]);
            else
                sb.Append(' ');

            if ((i + 1) % pixelWide == 0 && i != 0)
                sb.AppendLine();
        }
        
        return sb.ToString();
    }
}